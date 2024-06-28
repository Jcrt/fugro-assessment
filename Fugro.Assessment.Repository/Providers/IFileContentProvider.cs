using System.Runtime.CompilerServices;

namespace Fugro.Assessment.Repository.Providers;

public interface IFileContentProvider
{
    public IEnumerable<string> ReadNext();
}

public class FileContentProvider : IFileContentProvider
{
    //TODO: Add config provider to inform file path
    const string _fileName = "polyline sample.csv";
    readonly string _filePath;

    public FileContentProvider()
    {
        _filePath = Path.Combine(AppContext.BaseDirectory, "assets", _fileName);

        if (!File.Exists(_filePath))
            throw new FileNotFoundException(nameof(_filePath));
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
