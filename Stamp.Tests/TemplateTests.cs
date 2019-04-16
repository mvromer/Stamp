using System;
using System.Collections.Generic;
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

parameters:
- name: intVar
  type: int
  required: false

- name: floatVar
  type: float
  required: true

- name: boolVar
  type: bool

- name: stringVar
  type: string
";
            using( var reader = new StringReader( manifest ) )
            {
                var t = Template.CreateFromReader( reader );
                t.Name.Should().Be( "FooTemplate" );
                t.Version.Should().Be( "1.2.3" );
                t.Parameters.Count.Should().Be( 4 );

                var expectedNames = new List<string> { "intVar", "floatVar", "boolVar", "stringVar" };

                foreach( var p in t.Parameters )
                {
                    expectedNames.Remove( p.Name ).Should().Be( true );

                    switch( p.Name )
                    {
                        case "intVar":
                            p.Required.Should().Be( false );
                            break;

                        case "floatVar":
                            p.Required.Should().Be( true );
                            break;

                        case "boolVar":
                            p.Required.Should().Be( true );
                            break;

                        case "stringVar":
                            p.Required.Should().Be( true );
                            break;
                    }
                }

                expectedNames.Count.Should().Be( 0 );
            }
        }
    }
}
