using MongoDB.Bson;
using OpenCvSharp;
using VisionaryAnalytics.Worker.Application.Interfaces.Repositories;
using VisionaryAnalytics.Worker.Application.Interfaces.Services;
using VisionaryAnalytics.Worker.Domain.Entities;
using VisionaryAnalytics.Worker.Domain.Enums;

namespace VisionaryAnalytics.Worker.Application.Services;

public class VideoService(ILogger<VideoService> logger, IVideoRepository videoRepository) : IVideoService
{
    public async Task ProcessVideo(ObjectId? videoId)
    {
        try
        {
            var video = await videoRepository.GetAsync(videoId);
            if (video is null)
            {
                logger.LogInformation("Video not found. id: {Id}", videoId);
                return;
            }

            using var capture = new VideoCapture(video.Path);
            if (!capture.IsOpened())
            {
                Console.WriteLine("Video already open, cannot process.");
                return;
            }

            logger.LogInformation($"Starting analysis to video: {video.Name}");

            await videoRepository.UpdateStatusAsync(videoId!.Value, VideoStatus.Processing);

            var qrCodeContents = new HashSet<string>();
            var qrCodesToSave = new List<QRCode>();
            using var frame = new Mat();
            var qrDecoder = new QRCodeDetector();
            int frameCount = 0;

            while (true)
            {
                if (!capture.Read(frame) || frame.Empty())
                    break;

                string decodedText = qrDecoder.DetectAndDecode(frame, out Point2f[] points);

                if (!string.IsNullOrEmpty(decodedText) && !qrCodeContents.Contains(decodedText))
                {

                    double timestampMs = capture.Get(VideoCaptureProperties.PosMsec);
                    var ts = TimeSpan.FromMilliseconds(timestampMs);
                    qrCodeContents.Add(decodedText);
                    qrCodesToSave.Add(new(frameCount, decodedText, ts));
                }

                frameCount++;
            }

            await SaveQrCodeInfosAsync(qrCodesToSave, video);

            await videoRepository.UpdateStatusAsync(videoId!.Value, VideoStatus.Finished);

            capture.Release();

            logger.LogInformation($"Finishing analysis to video: {video.Name}");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
        
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
