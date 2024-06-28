using Fugro.Assessment.Geometry.Extensions;
using Fugro.Assessment.Geometry.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Fugro.Assessment.Geometry.UnitTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddGeometryDependencies_ShouldAddRightServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddGeometryDependencies();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mathService = serviceProvider.GetRequiredService<IGeometryUtility>();

        Assert.NotNull(mathService);
    }
}
