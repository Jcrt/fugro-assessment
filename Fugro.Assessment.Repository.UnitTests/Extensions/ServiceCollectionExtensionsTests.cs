using Fugro.Assessment.Geometry.Sources;
using Fugro.Assessment.Repository.Extensions;
using Fugro.Assessment.Repository.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Fugro.Assessment.Repository.UnitTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRepositoryDependencies_ShouldAddRightServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddRepositoryDependencies();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var pointsRepository = serviceProvider.GetRequiredService<IPointsRepository>();
        var fileContentProvider = serviceProvider.GetRequiredService<IFileContentProvider>();

        Assert.NotNull(pointsRepository);
        Assert.NotNull(fileContentProvider);
    }
}
