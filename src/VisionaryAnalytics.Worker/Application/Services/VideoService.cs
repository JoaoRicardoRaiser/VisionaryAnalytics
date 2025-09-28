using MongoDB.Bson;
using OpenCvSharp;
using System.Collections.Concurrent;
using VisionaryAnalytics.Worker.Application.Interfaces.Repositories;
using VisionaryAnalytics.Worker.Application.Interfaces.Services;
using VisionaryAnalytics.Worker.Domain.Entities;
using VisionaryAnalytics.Worker.Domain.Enums;

namespace VisionaryAnalytics.Worker.Application.Services;

public class VideoService(ILogger<VideoService> logger, IVideoRepository videoRepository) : IVideoService
{
    public async Task ProcessVideo(ObjectId? videoId)
    {
        if (videoId is null) return;

        var video = await videoRepository.GetAsync(videoId);
        if (video is null)
        {
            logger.LogInformation("Video not found. id: {Id}", videoId);
            return;
        }

        using var capture = new VideoCapture(video.Path);
        if (!capture.IsOpened())
        {
            logger.LogInformation("Video already open, cannot process.");
            return;
        }

        logger.LogInformation($"Starting analysis to video: {video.Name}");
        await videoRepository.UpdateStatusAsync(videoId.Value, VideoStatus.Processing);

        var qrCodeContents = new ConcurrentDictionary<string, bool>();
        var qrCodesToSave = new ConcurrentBag<QRCode>();
        var frameQueue = new BlockingCollection<(Mat frame, int index)>(boundedCapacity: 50);

        var producer = Task.Run(() =>
        {
            int frameCount = 0;
            using var frame = new Mat();
            while (capture.Read(frame) && !frame.Empty())
            {
                frameQueue.Add((frame.Clone(), frameCount));
                frameCount++;
            }
            frameQueue.CompleteAdding();
        });

        var consumers = Enumerable.Range(0, Environment.ProcessorCount).Select(_ => Task.Run(() =>
        {
            var qrDecoder = new QRCodeDetector();
            foreach (var (frame, index) in frameQueue.GetConsumingEnumerable())
            {
                string decodedText = qrDecoder.DetectAndDecode(frame, out Point2f[] points);

                if (!string.IsNullOrEmpty(decodedText) && qrCodeContents.TryAdd(decodedText, true))
                {
                    double timestampMs = capture.Get(VideoCaptureProperties.PosMsec);
                    var ts = TimeSpan.FromMilliseconds(timestampMs);
                    qrCodesToSave.Add(new QRCode(index, decodedText, ts));
                }

                frame.Dispose();
            }
        })).ToArray();

        await producer;
        await Task.WhenAll(consumers);

        await SaveQrCodeInfosAsync(qrCodesToSave.ToList(), video);
        await videoRepository.UpdateStatusAsync(videoId.Value, VideoStatus.Finished);

        capture.Release();
        logger.LogInformation($"Finishing analysis to video: {video.Name}");
    }

    private async Task SaveQrCodeInfosAsync(IList<QRCode> foundQrCodes, Video video)
    {
        if (foundQrCodes.Count == 0)
        {
            logger.LogInformation("QR codes not found for video: {videoPath}", video.Path);
            return;
        }

        await videoRepository.UpsertQrCodesAsync(video.Id, foundQrCodes);

    }
}
