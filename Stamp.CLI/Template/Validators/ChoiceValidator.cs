using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            throw new System.NotImplementedException();
        }

        private IReadOnlyList<T> ValidValues { get; }
    }
}
