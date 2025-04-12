using NAudio.Wave;

namespace MailRuDownloader.Helpers;

public static class VideoConverterHelper
{
    public static void ConvertToWav(string videoPath, string outputWavPath)
    {
        try
        {
            // MediaFoundationReader может открыть .mp4 и .m4a файлы
            using var reader = new MediaFoundationReader(videoPath);
            WaveFileWriter.CreateWaveFile(outputWavPath, reader);

            Console.WriteLine($"Аудио сохранено в .wav: {outputWavPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при извлечении аудио: {ex.Message}");
        }
    }
}
