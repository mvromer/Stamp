using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;
using YamlDotNet.Core;

using Stamp.CLI.Template;
using Stamp.CLI.Template.Validators;

namespace Stamp.Tests
{
    public class TemplateTests
    {
        [Fact]
        public void TestItCanReadMinimalManifest()
        {
            var manifest = @"
name: FooTemplate
version: 1.0.0
";

            using( var reader = new StringReader( manifest ) )
            {
                var t = Template.CreateFromReader( reader );
                t.Name.Should().Be( "FooTemplate" );
                t.Version.Should().Be( "1.0.0" );
                t.Parameters.Any().Should().BeFalse();
            }
        }

        [Fact]
        public void TestItFailsWhenMissingTemplateName()
        {
            var manifest = @"
version: 1.0.0
";

            using( var reader = new StringReader( manifest ) )
            {
                Action act = () => Template.CreateFromReader( reader );
                act.Should().Throw<YamlException>()
                    .WithInnerException<ValidationException>();
            }
        }

        [Fact]
        public void TestItFailsWhenMissingTemplateVersion()
        {
            var manifest = @"
name: FooTemplate
";

            using( var reader = new StringReader( manifest ) )
            {
                Action act = () => Template.CreateFromReader( reader );
                act.Should().Throw<YamlException>()
                    .WithInnerException<ValidationException>();
            }
        }

        [Fact]
        public void TestItCanReadSimpleIntParameterFromManifest()
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
                var t = Template.CreateFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<int>>();

                var p = (Parameter<int>)t.Parameters.First();
                p.Name.Should().Be( "intParam" );
                p.Required.Should().BeTrue();
            }
        }

        [Fact]
        public void TestItCanReadIntParameterWithValidChoiceValidatorFromManifest()
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
                var t = Template.CreateFromReader( reader );
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
        public void TestItFailsWhenChoiceValidatorValuesDoNotMatchParameterType()
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
                Action act = () => Template.CreateFromReader( reader );
                act.Should().Throw<InvalidCastException>();
            }
        }

        [Fact]
        public void TestItCanReadSimpleStringParameterFromManifest()
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
                var t = Template.CreateFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<string>>();

                var p = (Parameter<string>)t.Parameters.First();
                p.Name.Should().Be( "stringParam" );
                p.Required.Should().BeTrue();
            }
        }

        [Fact]
        public void TestItCanReadSimpleFloatParameterFromManifest()
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
                var t = Template.CreateFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<float>>();

                var p = (Parameter<float>)t.Parameters.First();
                p.Name.Should().Be( "floatParam" );
                p.Required.Should().BeTrue();
            }
        }

        [Fact]
        public void TestItCanReadSimpleBoolParameterFromManifest()
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
                var t = Template.CreateFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<bool>>();

                var p = (Parameter<bool>)t.Parameters.First();
                p.Name.Should().Be( "boolParam" );
                p.Required.Should().BeTrue();
            }
        }

        [Fact]
        public void TestItCanReadRequiredParameterFieldFromManifest()
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
                var t = Template.CreateFromReader( reader );
                t.Parameters.Count.Should().Be( 1 );
                t.Parameters.First().Should().BeOfType<Parameter<int>>();

                var p = (Parameter<int>)t.Parameters.First();
                p.Required.Should().BeFalse();
            }
        }
    }
}
