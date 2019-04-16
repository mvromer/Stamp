using System;
using YamlDotNet.Serialization;

namespace Stamp.CLI.Template.Builders
{
    class ParameterBuilder
    {
        public string Name { get; set; }

        [YamlMember( Alias = "type" )]
        public TypeCode TypeCode { get; set; }

        public bool? Required { get; set; }

        public IParameter Build()
        {
            string name = this.Name;
            bool required = this.Required.HasValue ? this.Required.Value : true;

            switch( this.TypeCode )
            {
                case TypeCode.Int32:
                    return new Parameter<int>( name, required );

                case TypeCode.Single:
                    return new Parameter<float>( name, required );

                case TypeCode.String:
                    return new Parameter<string>( name, required );

                case TypeCode.Boolean:
                    return new Parameter<bool>( name, required );

                default:
                    var codeName = Enum.GetName( typeof(TypeCode), this.TypeCode );
                    throw new NotSupportedException( $"Unexpected type code '{codeName}'." );
            }
        }
    }
}
