using System;
using System.Linq;
using FluentAssertions;
using Moq;
using PathLib;
using SystemEnvironment.Abstractions;
using Xunit;

using Stamp.CLI.Config;

namespace Stamp.Tests
{
    public class StampConfigTests
    {
        [Fact]
        public void TestItLoadStandardStampConfig()
        {
            IPurePath mockedFolderPath = PurePath.Create( "/opt" );
            var mockedEnvironment = Mock.Of<ISystemEnvironment>(
                e => e.GetFolderPath( It.IsAny<Environment.SpecialFolder>(),
                    It.IsAny<Environment.SpecialFolderOption>() ) == mockedFolderPath.ToString()
            );

            IStampConfig stampConfig = new StampConfig( mockedEnvironment );
            var expectedRootDir = mockedFolderPath.Join( StampConfig.RootDirName );
            stampConfig.RootDir.Should().Be( expectedRootDir );
        }
    }
}
