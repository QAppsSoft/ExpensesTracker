namespace TestsCommons;

public sealed class TemporalStorage : IDisposable
{
    public string TempDirPath { get; }

    public TemporalStorage()
    {
        TempDirPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(TempDirPath);
    }
    
    public string GetTemporalFileName(string? fileName = null, string? extension = null)
    {
        return Path.Combine(TempDirPath, $"{fileName ?? Guid.NewGuid().ToString()}{extension ?? string.Empty}");
    }
    
    public string GetTemporalDirectory()
    {
        var path = Path.Combine(TempDirPath, $"{Guid.NewGuid()}");
        Directory.CreateDirectory(path);
        return path;
    }

    public string GetEmptyFile(string? extension = null)
    {
        var emptyFile = GetTemporalFileName(extension: extension);
        File.Create(emptyFile).Dispose();
        return emptyFile;
    }

    public void Dispose()
    {
        for (var i = 0; i < 10; i++)
        {
            try
            {
                new DirectoryInfo(TempDirPath).Delete(true);
                break;
            }
            catch
            {
                Thread.Sleep(500 * i * i);
            }
        }
    }
}