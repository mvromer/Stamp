using System;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Stamp.CLI.Template
{
    /// <summary>
    /// Decorator that validates the ValidationAttributes attached to a deserialized YAML node.
    /// </summary>
    /// <remarks>
    /// This is based on the YamlDotNet sample here:
    /// https://github.com/aaubry/YamlDotNet/wiki/Samples.ValidatingDuringDeserialization
    /// </remarks>
    class ValidatingNodeDeserializer : INodeDeserializer
    {
        public bool Deserialize( IParser reader,
            Type expectedType,
            Func<IParser, Type, object> nestedObjectDeserializer,
            out object value )
        {
            if( this.InnerDeserializer.Deserialize( reader, expectedType, nestedObjectDeserializer, out value ) )
            {
                var context = new ValidationContext( value );
                Validator.ValidateObject( value, context, validateAllProperties: true );
                return true;
            }

            return false;
        }

        internal ValidatingNodeDeserializer( INodeDeserializer innerDeserializer )
        {
            this.InnerDeserializer = innerDeserializer;
        }

        private INodeDeserializer InnerDeserializer { get; }
    }
}
