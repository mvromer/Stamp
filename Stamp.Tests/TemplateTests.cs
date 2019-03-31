using System;
using System.IO;
using FluentAssertions;
using Xunit;

using Stamp.CLI.Template;

namespace Stamp.Tests
{
    public class TemplateTests
    {
        [Fact]
        public void TestItCanCreateManifestFromValidYaml()
        {
            var manifest = @"
name: FooTemplate
version: 1.2.3
";
            using( var reader = new StringReader( manifest ) )
            {
                var t = Template.CreateFromReader( reader );
                t.Name.Should().Be( "FooTemplate" );
                t.Version.Should().Be( "1.2.3" );
            }
        }
    }
}
