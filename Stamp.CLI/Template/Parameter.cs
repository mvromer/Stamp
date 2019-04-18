using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Stamp.CLI.Template
{
    class Parameter<T> : IParameter<T>
    {
        public string Name { get; }

        public bool Required { get; }

        public IReadOnlyCollection<IValidator<T>> Validators { get; }

        internal Parameter( string name, bool required, IList<IValidator<T>> validators )
        {
            this.Name = name;
            this.Required = required;
            this.Validators = new ReadOnlyCollection<IValidator<T>>( validators );
        }
    }
}
