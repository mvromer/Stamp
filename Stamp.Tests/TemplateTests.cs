using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

using FluentAssertions;
using Moq;
using PathLib;
using Xunit;
using YamlDotNet.Core;

using Stamp.CLI.Repository;
using Stamp.CLI.Template;
using Stamp.CLI.Template.Validators;

namespace Stamp.Tests
{
    public class TemplateTests
    {
        public TemplateTests()
        {
            this.TemplateLoader = new TemplateLoader( new MockFileSystem(),
                Mock.Of<IRepositoryLoader>() );
        }

        private TemplateLoader TemplateLoader { get; }

        [Fact]
        public void ItCanReadMinimalManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Name.Should().Be( "FooTemplate" );
                t.Version.Should().Be( "1.0.0" );
                t.Parameters.Any().Should().BeFalse();
            }
        }

        [Fact]
        public void ItFailsWhenMissingTemplateName()
        {
            var manifest = @"
version: 1.0.0
";

            using( var reader = new StringReader( manifest ) )
            {
                Action act = () => this.TemplateLoader.LoadFromReader( reader );
                act.Should().Throw<YamlException>()
                    .WithInnerException<ValidationException>();
            }
        }

        [Fact]
        public void ItFailsWhenMissingTemplateVersion()
        {
            var manifest = @"
name: FooTemplate
";

            using( var reader = new StringReader( manifest ) )
            {
                Action act = () => this.TemplateLoader.LoadFromReader( reader );
                act.Should().Throw<YamlException>()
                    .WithInnerException<ValidationException>();
            }
        }

        [Fact]
        public void ItCanReadSimpleIntParameterFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

parameters:
- name: intParam
  type: int
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<int>>();

                var p = (Parameter<int>)t.Parameters.First();
                p.Name.Should().Be( "intParam" );
                p.Required.Should().BeTrue();
                p.DefaultValue.Should().Be( 0 );
            }
        }

        [Fact]
        public void ItCanReadIntParameterWithValidChoiceValidatorFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

parameters:
- name: intParam
  type: int
  validators:
  - !choice
    values:
    - 0
    - 1
    - 10
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<int>>();

                var p = (Parameter<int>)t.Parameters.First();
                p.Name.Should().Be( "intParam" );
                p.Required.Should().BeTrue();
                p.Validators.Count.Should().Be( 1 );
                p.Validators.First().Should().BeOfType<ChoiceValidator<int>>();

                var v = (ChoiceValidator<int>)p.Validators.First();
                v.Validate( 0 ).Should().BeTrue();
                v.Validate( 1 ).Should().BeTrue();
                v.Validate( 10 ).Should().BeTrue();
                v.Validate( 10000 ).Should().BeFalse();
            }
        }

        [Fact]
        public void ItFailsWhenChoiceValidatorValuesDoNotMatchParameterType()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

parameters:
- name: intParam
  type: int
  validators:
  - !choice
    values:
    - 0
    - badValue
";

            using( var reader = new StringReader( manifest ) )
            {
                Action act = () => this.TemplateLoader.LoadFromReader( reader );
                act.Should().Throw<InvalidCastException>();
            }
        }

        [Fact]
        public void ItCanReadSimpleStringParameterFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

parameters:
- name: stringParam
  type: string
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<string>>();

                var p = (Parameter<string>)t.Parameters.First();
                p.Name.Should().Be( "stringParam" );
                p.Required.Should().BeTrue();
                p.DefaultValue.Should().BeNull();
            }
        }

        [Fact]
        public void ItCanReadSimpleFloatParameterFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

parameters:
- name: floatParam
  type: float
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<float>>();

                var p = (Parameter<float>)t.Parameters.First();
                p.Name.Should().Be( "floatParam" );
                p.Required.Should().BeTrue();
                p.DefaultValue.Should().Be( 0.0f );
            }
        }

        [Fact]
        public void ItCanReadSimpleBoolParameterFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

parameters:
- name: boolParam
  type: bool
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<bool>>();

                var p = (Parameter<bool>)t.Parameters.First();
                p.Name.Should().Be( "boolParam" );
                p.Required.Should().BeTrue();
                p.DefaultValue.Should().BeFalse();
            }
        }

        [Fact]
        public void ItCanReadRequiredParameterFieldFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

parameters:
- name: notRequiredParam
  type: int
  required: false
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<int>>();

                var p = (Parameter<int>)t.Parameters.First();
                p.Required.Should().BeFalse();
            }
        }

        [Fact]
        public void ItCanReadDefaultParameterFieldFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

parameters:
- name: explicitDefaultParam
  type: int
  default: 100
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<int>>();

                var p = (Parameter<int>)t.Parameters.First();
                p.DefaultValue.Should().Be( 100 );
            }
        }

        [Fact]
        public void ItCanReadSimpleFileFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

files:
- path: path/to/file.txt
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Files.Count.Should().Be( 1 );

                var f = t.Files.First();
                f.Path.Should().Be( "path/to/file.txt" );
                f.Computed.Should().BeFalse();
                f.OutputDirectory.Value.Should().Be( "path/to" );
                f.OutputName.Value.Should().Be( "file.txt" );

                var outputPath = new PurePosixPath( f.OutputDirectory.Value, f.OutputName.Value );
                f.Path.Should().Be( outputPath.ToString() );
            }
        }

        [Fact]
        public void ItCanReadComputedFileFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

files:
- path: path/to/file.txt
  computed: true
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Files.Count.Should().Be( 1 );

                var f = t.Files.First();
                f.Computed.Should().BeTrue();
            }
        }

        [Fact]
        public void ItCanReadOutputDirectoryFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

files:
- path: path/to/file.txt
  outputDir: new/path/to
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Files.Count.Should().Be( 1 );

                var f = t.Files.First();
                f.OutputDirectory.Value.Should().Be( "new/path/to" );
            }
        }

        [Fact]
        public void ItCanReadOutputNameFromManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

files:
- path: path/to/file.txt
  outputName: newFile.txt
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = this.TemplateLoader.LoadFromReader( reader );
                t.Files.Count.Should().Be( 1 );

                var f = t.Files.First();
                f.OutputName.Value.Should().Be( "newFile.txt" );
            }
        }

        [Fact]
        public void ItFailsWhenOutputNameContainsDirectory()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0

files:
- path: path/to/file.txt
  outputName: new/path/to/newFile.txt
";

            using( var reader = new StringReader( manifest ) )
            {
                Action act = () => this.TemplateLoader.LoadFromReader( reader );
                act.Should().Throw<NotSupportedException>();
            }
        }
    }
}
