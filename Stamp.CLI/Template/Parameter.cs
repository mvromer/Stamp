using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Stamp.CLI.Template
{
    class Parameter<T> : IParameter
    {
        public string Name { get; }

        public bool Required { get; }

        public IReadOnlyList<IValidator> Validators { get; }

        internal Parameter( string name, bool required, IList<IValidator> validators )
        {
            this.Name = name;
            this.Required = required;
            this.Validators = new ReadOnlyCollection<IValidator>( validators );
        }
    }
}
