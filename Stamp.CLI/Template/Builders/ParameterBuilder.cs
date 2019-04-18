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
            return new Parameter<T>( name, required, validators );
        }
    }
}
