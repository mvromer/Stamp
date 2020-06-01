using System;
using System.IO;

using PathLib;
using System.IO.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

using Stamp.CLI.Repository;

namespace Stamp.CLI.Template
{
    class TemplateLoader : ITemplateLoader
    {
        public TemplateLoader( IFileSystem fileSystem,
            IRepositoryLoader repositoryLoader )
        {
            this.FileSystem = fileSystem;
            this.RepositoryLoader = repositoryLoader;
        }


        public ITemplate LoadFromManifest( IPurePath manifestPath )
        {
            using( var reader = this.FileSystem.File.OpenText( manifestPath.ToString() ) )
                return LoadFromReader( reader );
        }

        public ITemplate LoadFromReader( TextReader reader )
        {
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

        public ITemplate FindTemplate( string templateName )
        {
            if( string.IsNullOrEmpty( templateName ) )
                throw new ArgumentException( "Template name cannot be null or empty.", nameof(templateName) );
            return null;
        }

        private IFileSystem FileSystem { get; }
        private IRepositoryLoader RepositoryLoader { get; }
    }
}
