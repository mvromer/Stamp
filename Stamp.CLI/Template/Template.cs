using System.IO;
using Semver;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Stamp.CLI.Template
{
    class Template
    {
        internal string Name { get; }

        internal SemVersion Version { get; }

        internal static Template CreateFromManifest( string manifestPath )
        {
            using( var reader = File.OpenText( manifestPath ) )
                return CreateFromReader( reader );
        }

        internal static Template CreateFromReader( TextReader reader )
        {
            var deserializer = new DeserializerBuilder()
                    .WithNamingConvention( new CamelCaseNamingConvention() )
                    .Build();

            return deserializer.Deserialize<TemplateBuilder>( reader ).Build();
        }

        internal Template( string name, SemVersion version )
        {
            this.Name = name;
            this.Version = version;
        }
    }
}
