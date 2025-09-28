using Microsoft.Extensions.Logging;
using Moq;

namespace VisionaryAnalytics.Worker.UnitTests.Helpers;

public static class LoggerMoqExtensions
{
    public static void VerifyLog<T>(
        this Mock<ILogger<T>> logger,
        LogLevel level,
        string expectedMessage,
        Times times)
    {
        logger.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            times);
    }
}
