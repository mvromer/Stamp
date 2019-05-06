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
    }
}
