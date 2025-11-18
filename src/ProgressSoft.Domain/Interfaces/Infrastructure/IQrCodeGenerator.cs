namespace ProgressSoft.Domain.Interfaces.Infrastructure;

public interface IQrCodeGenerator
{
    // returns the image as a byte array (typically PNG)
    byte[] GenerateQrCode(string content);
}
