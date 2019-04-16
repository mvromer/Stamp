using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Stamp.CLI.Template
{
    /// <summary>
    /// Converter used for converting YAML values to System.TypeCode values.
    /// </summary>
    /// <remarks>
    /// Stamp supports converting to the following values to their corresponding System.TypeCode
    /// value:
    ///
    ///   int - TypeCode.Int32
    ///   float - TypeCode.Single
    ///   string - TypeCode.String
    ///   bool - TypeCode.Boolean
    ///
    /// This only supports deserialization. Serialization support is not implemented.
    /// </remarks>
    class TypeCodeTypeConverter : IYamlTypeConverter
    {
        public bool Accepts( Type type )
        {
            return type == typeof( TypeCode );
        }

        public object ReadYaml( IParser parser, Type type )
        {
            TypeCode parsedTypeCode = TypeCode.Empty;

            if( parser.Current is Scalar scalar )
            {
                switch( scalar.Value )
                {
                    case "int":
                        parsedTypeCode = TypeCode.Int32;
                        break;

                    case "float":
                        parsedTypeCode = TypeCode.Single;
                        break;

                    case "string":
                        parsedTypeCode = TypeCode.String;
                        break;

                    case "bool":
                        parsedTypeCode = TypeCode.Boolean;
                        break;

                    default:
                        throw new NotSupportedException( $"Unknown or unsupported type '{scalar.Value}' found." );
                }
            }

            parser.MoveNext();
            return parsedTypeCode;
        }

        public void WriteYaml( IEmitter emitter, object value, Type type )
        {
            throw new NotImplementedException();
        }
    }
}
