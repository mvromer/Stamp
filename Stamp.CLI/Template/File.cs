using Stamp.CLI.Script;

namespace Stamp.CLI.Template
{
    class File : IFile
    {
        public string Path { get; }

        public bool Computed { get; }

        public ComputedString OutputDirectory { get; }

        public ComputedString OutputName { get; }

        internal File( string path, bool computed, ComputedString outputDirectory, ComputedString outputName  )
        {
            this.Path = path;
            this.Computed = computed;
            this.OutputDirectory = outputDirectory;
            this.OutputName = outputName;
        }
    }
}
