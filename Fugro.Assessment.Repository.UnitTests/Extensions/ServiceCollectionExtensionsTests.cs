using Fugro.Assessment.Repository.Extensions;
using Fugro.Assessment.Repository.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fugro.Assessment.Repository.UnitTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<ILogger<CsvFilePointsRepository>> _loggerMock = new();


    [Fact]
    public void AddRepositoryDependencies_ShouldAddRightServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddRepositoryDependencies();
        serviceCollection.AddSingleton(_configurationMock.Object);
        serviceCollection.AddSingleton(_loggerMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var pointsRepository = serviceProvider.GetRequiredService<IPointsRepository>();
        var fileContentProvider = serviceProvider.GetRequiredService<IFileContentProvider>();

        Assert.NotNull(pointsRepository);
        Assert.NotNull(fileContentProvider);
    }
}
