using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Semver;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace Stamp.CLI.Template
{
    class Template : ITemplate
    {
        public string Name { get; }

        public SemVersion Version { get; }

        public IReadOnlyList<IParameter> Parameters { get; }

        internal static ITemplate CreateFromManifest( string manifestPath )
        {
            using( var reader = File.OpenText( manifestPath ) )
                return CreateFromReader( reader );
        }

        internal static ITemplate CreateFromReader( TextReader reader )
        {
            var deserializer = new DeserializerBuilder()
                    .WithNamingConvention( new CamelCaseNamingConvention() )
                    .WithTypeConverter( new TypeCodeTypeConverter() )
                    .WithTagMapping( Builders.ChoiceValidatorBuilder.Tag, typeof(Builders.ChoiceValidatorBuilder) )
                    .Build();

            return deserializer.Deserialize<Builders.TemplateBuilder>( reader ).Build();
        }

        internal Template( string name, SemVersion version, IList<IParameter> parameters )
        {
            this.Name = name;
            this.Version = version;
            this.Parameters = new ReadOnlyCollection<IParameter>( parameters );
        }
    }
}
