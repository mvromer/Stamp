using System;
using System.IO;
using System.IO.Abstractions;
using PathLib;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace Stamp.CLI.Template
{
    class TemplateLoader : ITemplateLoader
    {
        public TemplateLoader( IFileSystem fileSystem )
        {
            this.FileSystem = fileSystem;
        }

        public ITemplate LoadFromTemplateDirectory( IPurePath templatePath )
        {
            const string ManifestFileName = "manifest.yml";
            var manifestPath = templatePath.Join( ManifestFileName );
            using var reader = this.FileSystem.File.OpenText( manifestPath.ToString() );

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention( CamelCaseNamingConvention.Instance )
                .WithNodeDeserializer( inner => new ValidatingNodeDeserializer( inner ),
                    s => s.InsteadOf<ObjectNodeDeserializer>() )
                .WithTypeConverter( new TypeCodeTypeConverter() )
                .WithTagMapping( Builders.ChoiceValidatorBuilder.Tag,
                    typeof(Builders.ChoiceValidatorBuilder) )
                .Build();

            return deserializer.Deserialize<Builders.TemplateBuilder>( reader ).Build();
        }

        private IFileSystem FileSystem { get; }
    }
}
