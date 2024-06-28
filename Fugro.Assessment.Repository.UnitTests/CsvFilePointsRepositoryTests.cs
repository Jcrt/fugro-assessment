using Fugro.Assessment.Repository.Providers;
using Moq;
using Moq.AutoMock;

namespace Fugro.Assessment.Repository.UnitTests
{
    public class CsvFilePointsRepositoryTests
    {
        private readonly AutoMocker _autoMocker = new();
        private readonly Mock<IFileContentProvider> _fileContentProvider;

        public CsvFilePointsRepositoryTests()
        {
            _fileContentProvider = _autoMocker.GetMock<IFileContentProvider>();
        }

        [Fact]
        public async Task GetPoints_ShouldRetrieveAvailablePoints()
        {
            var readSequence = new List<string>()
            {
                "1,2", 
                "3,4"
            };

            _fileContentProvider.Setup(x => x.ReadNext()).Returns(readSequence.AsEnumerable());

            var pointsRepository = _autoMocker.CreateInstance<CsvFilePointsRepository>();
            var pointList = await pointsRepository.GetPoints();
            
            Assert.Equal(2, pointList.Count);
            Assert.Equal(1, pointList[0].X);
            Assert.Equal(2, pointList[0].Y);
            Assert.Equal(3, pointList[1].X);
            Assert.Equal(4, pointList[1].Y);
        }

        [Fact]
        public async Task GetPoints_ShouldThrowException()
        {
            var readSequence = new List<string>()
            {
                "a,b"
            };

            _fileContentProvider.Setup(x => x.ReadNext()).Returns(readSequence.AsEnumerable());

            var pointsRepository = _autoMocker.CreateInstance<CsvFilePointsRepository>();

            await Assert.ThrowsAsync<InvalidCastException>(() => pointsRepository.GetPoints());
        }
    }
}