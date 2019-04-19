using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Semver;

namespace Stamp.CLI.Template.Builders
{
    class TemplateBuilder
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public SemVersion Version { get; set; }

        public List<ParameterBuilder> Parameters { get; set; }

        public List<FileBuilder> Files { get; set; }

        internal ITemplate Build()
        {
            var parameters = (this.Parameters ?? new List<ParameterBuilder>()).Select( p => p.Build() ).ToList();
            var files = (this.Files ?? new List<FileBuilder>()).Select( f => f.Build() ).ToList();

            return new Template(
                this.Name,
                this.Version,
                parameters,
                files
            );
        }
    }
}
