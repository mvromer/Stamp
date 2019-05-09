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
        public void TestItCanLoadRepositories()
        {
            var stampConfig = StampConfig.Load();
            var repositories = stampConfig.LoadRepositories();
            repositories.Should().NotBeNull();
            repositories.Count.Should().Be( 0 );
        }
    }
}
