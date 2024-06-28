using Microsoft.Extensions.Configuration;

namespace Fugro.Assessment.Repository.Providers;

public class FileContentProvider : IFileContentProvider
{
    private const string _defaultFileName = "polyline sample.csv";
    private readonly string _filePath;

    public FileContentProvider(IConfiguration configuration)
    {
        var configPath = configuration.GetRequiredSection("FileContent:FullPath").Value;

        _filePath = (string.IsNullOrWhiteSpace(configPath))
            ? Path.Combine(AppContext.BaseDirectory, "assets", _defaultFileName)
            : configPath;

        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"The given file doesn't exist: {_filePath}");
    }

    public IEnumerable<string> ReadNext()
    {
        string row;
        using var fileStream = File.OpenRead(_filePath);
        using var reader = new StreamReader(fileStream);

        while (!string.IsNullOrEmpty(row = reader.ReadLine() ?? string.Empty))
           yield return row;
    }
}
