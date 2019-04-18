using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Stamp.CLI.Template.Validators
{
    class ChoiceValidator<T> : IValidator<T>
    {
        internal ChoiceValidator( IList<T> validValues )
        {
            this.ValidValues = new ReadOnlyCollection<T>( validValues );
        }

        public bool Validate( T value )
        {
            return this.ValidValues.Contains( value );
        }

        private IReadOnlyList<T> ValidValues { get; }
    }
}
