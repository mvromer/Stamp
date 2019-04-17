using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Stamp.CLI.Template.Builders
{
    class ChoiceValidatorBuilder : IValidatorBuilder
    {
        internal static readonly string Tag = "!choice";

        [YamlMember( Alias = "values" )]
        public List<string> ValidValues { get; set; }

        public IValidator Build<T>()
        {
            var validValues = new List<T>();
            foreach( var value in this.ValidValues )
            {
                try
                {
                    validValues.Add( (T)Convert.ChangeType( value, typeof(T) ) );
                }
                catch( Exception ex )
                {
                    throw new InvalidCastException(
                        $"Cannot convert choice value '{value}' to parameter type '{typeof(T).Name}'.", ex );
                }
            }

            return new Validators.ChoiceValidator<T>( validValues );
        }
    }
}
