using System.Collections.Generic;
using System.Linq;
using Semver;

namespace Stamp.CLI.Template.Builders
{
    class TemplateBuilder
    {
        public string Name { get; set; }

        public SemVersion Version { get; set; }

        public List<ParameterBuilder> Parameters { get; set; }

        public Template Build()
        {
            return new Template( this.Name,
                this.Version,
                this.Parameters.Select( p => p.Build() ).ToList() );
        }
    }
}
