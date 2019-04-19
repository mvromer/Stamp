using Stamp.CLI.Script;

namespace Stamp.CLI.Template
{
    interface IFile
    {
        string Path { get; }

        bool Computed { get; }

        ComputedString OutputDirectory { get; }

        ComputedString OutputName { get; }
    }
}
