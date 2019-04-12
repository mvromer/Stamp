using System;

namespace Stamp.CLI.Template.Builders
{
    class ParameterBuilder
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public bool? Required { get; set; }

        public Parameter Build()
        {
            return new Parameter( this.Name,
                this.Type,
                this.Required.HasValue ? this.Required.Value : true );
        }
    }
}
