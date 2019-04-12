using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Stamp.CLI.Template
{
    /// <summary>
    /// Converter used for converting YAML values to System.Type objects.
    /// </summary>
    /// <remarks>
    /// Stamp supports converting to the following values to their corresponding System.Type object:
    ///
    ///   int - System.Int32
    ///   float - System.Single
    ///   string - System.String
    ///   bool - System.Boolean
    ///
    /// This only supports deserialization. Serialization support is not implemented.
    /// </remarks>
    class TypeTypeConverter : IYamlTypeConverter
    {
        public bool Accepts( Type type )
        {
            return type == typeof( Type );
        }

        public object ReadYaml( IParser parser, Type type )
        {
            object parsedType = null;

            if( parser.Current is Scalar scalar )
            {
                switch( scalar.Value )
                {
                    case "int":
                        parsedType = typeof( int );
                        break;

                    case "float":
                        parsedType = typeof( float );
                        break;

                    case "string":
                        parsedType = typeof( string );
                        break;

                    case "bool":
                        parsedType = typeof( bool );
                        break;

                    default:
                        throw new NotSupportedException( $"Unknown or unsupported type '{scalar.Value}' found." );
                }
            }

            parser.MoveNext();
            return parsedType;
        }

        public void WriteYaml( IEmitter emitter, object value, Type type )
        {
            throw new NotImplementedException();
        }
    }
}
