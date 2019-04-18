using System.Collections.Generic;
using Semver;

namespace Stamp.CLI.Template
{
    interface ITemplate
    {
        string Name { get; }

        SemVersion Version { get; }

        IReadOnlyCollection<IParameter> Parameters { get; }
    }
}
