namespace Fugro.Assessment.Repository.Providers;

public interface IFileContentProvider
{
    public IEnumerable<string> ReadNext();
}
