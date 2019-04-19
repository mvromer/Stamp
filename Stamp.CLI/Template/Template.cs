using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Semver;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace Stamp.CLI.Template
{
    class Template : ITemplate
    {
        public string Name { get; }

        public SemVersion Version { get; }

        public IReadOnlyCollection<IParameter> Parameters { get; }

        public IReadOnlyCollection<IFile> Files { get; }

        internal static ITemplate CreateFromManifest( string manifestPath )
        {
            using( var reader = System.IO.File.OpenText( manifestPath ) )
                return CreateFromReader( reader );
        }

        internal static ITemplate CreateFromReader( TextReader reader )
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention( new CamelCaseNamingConvention() )
                .WithNodeDeserializer( inner => new ValidatingNodeDeserializer( inner ),
                    s => s.InsteadOf<ObjectNodeDeserializer>() )
                .WithTypeConverter( new TypeCodeTypeConverter() )
                .WithTagMapping( Builders.ChoiceValidatorBuilder.Tag,
                    typeof(Builders.ChoiceValidatorBuilder) )
                .Build();

            return deserializer.Deserialize<Builders.TemplateBuilder>( reader ).Build();
        }

        internal Template( string name, SemVersion version, IList<IParameter> parameters, IList<IFile> files )
        {
            this.Name = name;
            this.Version = version;
            this.Parameters = new ReadOnlyCollection<IParameter>( parameters );
            this.Files = new ReadOnlyCollection<IFile>( files );
        }
    }
}
