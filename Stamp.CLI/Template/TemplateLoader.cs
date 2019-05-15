using System;
using System.IO;

using PathLib;
using System.IO.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

using Stamp.CLI.Repository;

namespace Stamp.CLI.Template
{
    class TemplateLoader : ITemplateLoader
    {
        internal TemplateLoader( IFileSystem fileSystem,
            IRepositoryLoader repositoryLoader,
            IDeserializer deserializer )
        {
            this.FileSystem = fileSystem;
            this.RepositoryLoader = repositoryLoader;
            this.Deserializer = deserializer;
        }


        public ITemplate LoadFromManifest( IPurePath manifestPath )
        {
            using( var reader = this.FileSystem.File.OpenText( manifestPath.ToString() ) )
                return LoadFromReader( reader );
        }

        public ITemplate LoadFromReader( TextReader reader )
        {
            return this.Deserializer
                .Deserialize<Builders.TemplateBuilder>( reader )
                .Build();
        }

        public ITemplate FindTemplate( string templateName )
        {
            if( string.IsNullOrEmpty( templateName ) )
                throw new ArgumentException( "Template name cannot be null or empty.", nameof(templateName) );
            return null;
        }

        private IFileSystem FileSystem { get; }
        private IRepositoryLoader RepositoryLoader { get; }
        private IDeserializer Deserializer { get; }
    }
}
