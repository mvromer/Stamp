using System;

namespace Stamp.CLI.Template
{
    class Parameter<T> : IParameter
    {
        public string Name { get; }

        public bool Required { get; }

        internal Parameter( string name, bool required )
        {
            this.Name = name;
            this.Required = required;
        }
    }
}
