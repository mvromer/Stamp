using System.Linq;
using FluentAssertions;
using Xunit;

using Stamp.CLI.Config;

namespace Stamp.Tests
{
    public class StampConfigTests
    {
        [Fact]
        public void TestItLoadStandardStampConfig()
        {
            var stampConfig = StampConfig.Load();
            stampConfig.Should().NotBeNull();
        }

        [Fact]
        public void TestItCanLoadDefaultRepositories()
        {
            var stampConfig = StampConfig.Load();
            var repositories = stampConfig.LoadRepositories();
            repositories.Should().NotBeNull();
            repositories.Count.Should().Be( 1 );
            repositories.First().Name.Should().Be( ".local" );
            repositories.First().Description.Should().Be( "Local repository" );
        }
    }
}
