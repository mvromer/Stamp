using System.IO;

using PathLib;
using System.IO.Abstractions;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace Stamp.CLI.Template
{
    class TemplateFactory : ITemplateFactory
    {
        internal TemplateFactory( IFileSystem fileSystem, IDeserializer deserializer )
        {
            this.FileSystem = fileSystem;
            this.Deserializer = deserializer;
        }


        public ITemplate CreateFromManifest( IPurePath manifestPath )
        {
            using( var reader = this.FileSystem.File.OpenText( manifestPath.ToString() ) )
                return CreateFromReader( reader );
        }

        public ITemplate CreateFromReader( TextReader reader )
        {
            return this.Deserializer
                .Deserialize<Builders.TemplateBuilder>( reader )
                .Build();
        }

        private IFileSystem FileSystem { get; }
        private IDeserializer Deserializer { get; }
    }
}
