using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Semver;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace Stamp.CLI.Template
{
    class Template
    {
        internal string Name { get; }

        internal SemVersion Version { get; }

        internal IReadOnlyList<Parameter> Parameters { get; }

        internal static Template CreateFromManifest( string manifestPath )
        {
            using( var reader = File.OpenText( manifestPath ) )
                return CreateFromReader( reader );
        }

        internal static Template CreateFromReader( TextReader reader )
        {
            // Remove SystemTypeConverter because it depends on a YAML value being the type's
            // assembly qualified name, and Stamp only needs to support a limited set of types
            // specified using simple names (e.g., string, float, int, etc.).
            var deserializer = new DeserializerBuilder()
                    .WithNamingConvention( new CamelCaseNamingConvention() )
                    .WithoutTypeConverter<SystemTypeConverter>()
                    .WithTypeConverter( new TypeTypeConverter() )
                    .Build();

            return deserializer.Deserialize<Builders.TemplateBuilder>( reader ).Build();
        }

        internal Template( string name, SemVersion version, IList<Parameter> parameters )
        {
            this.Name = name;
            this.Version = version;
            this.Parameters = new ReadOnlyCollection<Parameter>( parameters );
        }
    }
}
