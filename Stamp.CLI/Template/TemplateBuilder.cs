using Semver;

namespace Stamp.CLI.Template
{
    class TemplateBuilder
    {
        public string Name { get; set; }

        public SemVersion Version { get; set; }

        public Template Build()
        {
            return new Template( this.Name, this.Version );
        }
    }
}
