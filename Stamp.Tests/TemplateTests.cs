using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Abstractions;
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
            this.FileSystem = new MockFileSystem();
            this.TemplateLoader = new TemplateLoader( this.FileSystem );
        }

        private MockFileSystem FileSystem { get; }
        private TemplateLoader TemplateLoader { get; }

        private static readonly IPurePath TestTemplatePath = PurePath.Create( "/opt/template" );
        private static readonly IPurePath TestTemplateManifestPath = TestTemplatePath.Join( "manifest.yml" );

        private void SetManifest( string manifestContents )
        {
            this.FileSystem.AddFile( TestTemplateManifestPath.ToString(), new MockFileData( manifestContents ) );
        }

        [Fact]
        public void ItCanReadMinimalManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Name.Should().Be( "FooTemplate" );
            t.Version.Should().Be( "1.0.0" );
            t.Parameters.Any().Should().BeFalse();
        }

        [Fact]
        public void ItFailsWhenMissingTemplateName()
        {
            SetManifest( @"
version: 1.0.0
" );

            Action act = () => this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            act.Should().Throw<YamlException>()
                .WithInnerException<ValidationException>();
        }

        [Fact]
        public void ItFailsWhenMissingTemplateVersion()
        {
            SetManifest( @"
name: FooTemplate
" );

                Action act = () => this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
                act.Should().Throw<YamlException>()
                    .WithInnerException<ValidationException>();
        }

        [Fact]
        public void ItCanReadSimpleIntParameterFromManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

parameters:
- name: intParam
  type: int
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Parameters.Count.Should().Be( 1 );
            t.Parameters.First().Should().BeOfType<Parameter<int>>();

            var p = (Parameter<int>)t.Parameters.First();
            p.Name.Should().Be( "intParam" );
            p.Required.Should().BeTrue();
            p.DefaultValue.Should().Be( 0 );
        }

        [Fact]
        public void ItCanReadIntParameterWithValidChoiceValidatorFromManifest()
        {
            SetManifest( @"
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
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
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

        [Fact]
        public void ItFailsWhenChoiceValidatorValuesDoNotMatchParameterType()
        {
            SetManifest( @"
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
" );

            Action act = () => this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            act.Should().Throw<InvalidCastException>();
        }

        [Fact]
        public void ItCanReadSimpleStringParameterFromManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

parameters:
- name: stringParam
  type: string
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Parameters.Count.Should().Be( 1 );
            t.Parameters.First().Should().BeOfType<Parameter<string>>();

            var p = (Parameter<string>)t.Parameters.First();
            p.Name.Should().Be( "stringParam" );
            p.Required.Should().BeTrue();
            p.DefaultValue.Should().BeNull();
        }

        [Fact]
        public void ItCanReadSimpleFloatParameterFromManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

parameters:
- name: floatParam
  type: float
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Parameters.Count.Should().Be( 1 );
            t.Parameters.First().Should().BeOfType<Parameter<float>>();

            var p = (Parameter<float>)t.Parameters.First();
            p.Name.Should().Be( "floatParam" );
            p.Required.Should().BeTrue();
            p.DefaultValue.Should().Be( 0.0f );
        }

        [Fact]
        public void ItCanReadSimpleBoolParameterFromManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

parameters:
- name: boolParam
  type: bool
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Parameters.Count.Should().Be( 1 );
            t.Parameters.First().Should().BeOfType<Parameter<bool>>();

            var p = (Parameter<bool>)t.Parameters.First();
            p.Name.Should().Be( "boolParam" );
            p.Required.Should().BeTrue();
            p.DefaultValue.Should().BeFalse();
        }

        [Fact]
        public void ItCanReadRequiredParameterFieldFromManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

parameters:
- name: notRequiredParam
  type: int
  required: false
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Parameters.Count.Should().Be( 1 );
            t.Parameters.First().Should().BeOfType<Parameter<int>>();

            var p = (Parameter<int>)t.Parameters.First();
            p.Required.Should().BeFalse();
        }

        [Fact]
        public void ItCanReadDefaultParameterFieldFromManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

parameters:
- name: explicitDefaultParam
  type: int
  default: 100
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Parameters.Count.Should().Be( 1 );
            t.Parameters.First().Should().BeOfType<Parameter<int>>();

            var p = (Parameter<int>)t.Parameters.First();
            p.DefaultValue.Should().Be( 100 );
        }

        [Fact]
        public void ItCanReadSimpleFileFromManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

files:
- path: path/to/file.txt
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Files.Count.Should().Be( 1 );

            var f = t.Files.First();
            f.Path.Should().Be( "path/to/file.txt" );
            f.Computed.Should().BeFalse();
            f.OutputDirectory.Value.Should().Be( "path/to" );
            f.OutputName.Value.Should().Be( "file.txt" );

            var outputPath = new PurePosixPath( f.OutputDirectory.Value, f.OutputName.Value );
            f.Path.Should().Be( outputPath.ToString() );
        }

        [Fact]
        public void ItCanReadComputedFileFromManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

files:
- path: path/to/file.txt
  computed: true
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Files.Count.Should().Be( 1 );

            var f = t.Files.First();
            f.Computed.Should().BeTrue();
        }

        [Fact]
        public void ItCanReadOutputDirectoryFromManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

files:
- path: path/to/file.txt
  outputDir: new/path/to
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Files.Count.Should().Be( 1 );

            var f = t.Files.First();
            f.OutputDirectory.Value.Should().Be( "new/path/to" );
        }

        [Fact]
        public void ItCanReadOutputNameFromManifest()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

files:
- path: path/to/file.txt
  outputName: newFile.txt
" );

            var t = this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            t.Files.Count.Should().Be( 1 );

            var f = t.Files.First();
            f.OutputName.Value.Should().Be( "newFile.txt" );
        }

        [Fact]
        public void ItFailsWhenOutputNameContainsDirectory()
        {
            SetManifest( @"
name: FooTemplate
version: 1.0.0

files:
- path: path/to/file.txt
  outputName: new/path/to/newFile.txt
" );

            Action act = () => this.TemplateLoader.LoadFromTemplateDirectory( TestTemplatePath );
            act.Should().Throw<NotSupportedException>();
        }
    }
}
