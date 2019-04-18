using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using YamlDotNet.Serialization;

namespace Stamp.CLI.Template.Builders
{
    class ParameterBuilder
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [YamlMember( Alias = "type" )]
        public TypeCode TypeCode { get; set; }

        public bool? Required { get; set; }

        [YamlMember( Alias = "default" )]
        public string DefaultValue { get; set; }

        public List<IValidatorBuilder> Validators { get; set; }

        public IParameter Build()
        {
            switch( this.TypeCode )
            {
                case TypeCode.Int32:
                    return BuildParameter<int>();

                case TypeCode.Single:
                    return BuildParameter<float>();

                case TypeCode.String:
                    return BuildParameter<string>();

                case TypeCode.Boolean:
                    return BuildParameter<bool>();

                default:
                    var codeName = Enum.GetName( typeof(TypeCode), this.TypeCode );
                    throw new NotSupportedException( $"Unexpected type code '{codeName}'." );
            }
        }

        private IParameter BuildParameter<T>()
        {
            string name = this.Name;
            bool required = this.Required.HasValue ? this.Required.Value : true;
            var validators = (this.Validators ?? new List<IValidatorBuilder>()).Select( v => v.Build<T>() ).ToList();
            T defaultValue;

            // If a default value was specified, we want to make sure that it's valid with respect
            // to the data type declared for the associated parameter.
            if( this.DefaultValue != null )
            {
                try
                {
                    defaultValue = (T)Convert.ChangeType( this.DefaultValue, typeof(T) );
                }
                catch( Exception ex )
                {
                    throw new InvalidCastException(
                        $"Cannot convert default value '{this.DefaultValue}' to parameter type '{typeof(T).Name}'.", ex );
                }
            }
            else
            {
                defaultValue = default(T);
            }

            return new Parameter<T>( name, required, defaultValue, validators );
        }
    }
}
