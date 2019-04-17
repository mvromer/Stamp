using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Stamp.CLI.Template.Validators
{
    class ChoiceValidator<T> : IValidator
    {
        internal ChoiceValidator( IList<T> validValues )
        {
            this.ValidValues = new ReadOnlyCollection<T>( validValues );
        }

        public bool Validate()
        {
            throw new System.NotImplementedException();
        }

        private IReadOnlyList<T> ValidValues { get; }
    }
}
