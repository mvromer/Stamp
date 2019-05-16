using System;
using System.Linq;

using FluentAssertions;
using Moq;
using PathLib;
using System.IO.Abstractions.TestingHelpers;
using SystemEnvironment.Abstractions;
using Xunit;

using Stamp.CLI.Config;

namespace Stamp.Tests
{
    public class StampConfigTests
    {
        [Fact]
        public void ItLoadStandardStampConfig()
        {
            var mockedFolderPath = PurePath.Create( "/opt" );
            var expectedRootPath = mockedFolderPath.Join( StampConfigConstants.ConfigDirectoryName );

            var environment = Mock.Of<ISystemEnvironment>(
                e => e.GetFolderPath( It.IsAny<Environment.SpecialFolder>(),
                    It.IsAny<Environment.SpecialFolderOption>() ) == mockedFolderPath.ToString()
            );

            var fileSystem = new MockFileSystem();

            IStampConfig stampConfig = new StampConfig( environment, fileSystem );
            stampConfig.RootPath.Should().Be( expectedRootPath );
        }

        [Fact]
        public void ItGetsExistingRepositoryPath()
        {
            const string repoName = "TestRepo";
            var mockedFolderPath = PurePath.Create( "/opt" );
            var expectedRepoPath = mockedFolderPath.Join(
                StampConfigConstants.ConfigDirectoryName,
                StampConfigConstants.RepositoriesDirectoryName,
                repoName
            );

            var environment = Mock.Of<ISystemEnvironment>(
                e => e.GetFolderPath( It.IsAny<Environment.SpecialFolder>(),
                    It.IsAny<Environment.SpecialFolderOption>() ) == mockedFolderPath.ToString()
            );

            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory( expectedRepoPath.ToString() );

            IStampConfig stampConfig = new StampConfig( environment, fileSystem );
            stampConfig.GetRepositoryPath( repoName ).Should().Be( expectedRepoPath );
        }
    }
}
