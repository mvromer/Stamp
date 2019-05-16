using System.Linq;

using FluentAssertions;
using Moq;
using PathLib;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

using Stamp.CLI.Config;
using Stamp.CLI.Repository;

namespace Stamp.Tests
{
    public class RepositoryLoaderTests
    {
        [Fact]
        public void ItCanLoadDefaultRepositories()
        {
            var fileSystem = new MockFileSystem();
            var stampConfig = Mock.Of<IStampConfig>(
                c => c.RootDir == PurePath.Create( "/opt/stamp" )
            );

            var repositoryLoader = new RepositoryLoader( fileSystem, stampConfig );
            var repositories = repositoryLoader.LoadRepositories().ToList();
            repositories.Should().NotBeNull();
            repositories.Count.Should().Be( 1 );
            repositories.First().Name.Should().Be( ".local" );
            repositories.First().Description.Should().Be( "Local repository" );
        }
    }
}
