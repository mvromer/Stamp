using System;

namespace Stamp.CLI.Template
{
    class Parameter
    {
        internal string Name { get; }

        internal Type Type { get; }

        internal bool Required { get; }

        internal Parameter( string name, Type type, bool required )
        {
            this.Name = name;
            this.Type = type;
            this.Required = required;
        }
    }
}
