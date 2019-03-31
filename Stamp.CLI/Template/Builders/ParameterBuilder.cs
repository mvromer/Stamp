using System;

namespace Stamp.CLI.Template.Builders
{
    class ParameterBuilder
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public bool? Required { get; set; }
    }
}
