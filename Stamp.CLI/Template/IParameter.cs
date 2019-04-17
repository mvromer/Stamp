using System.Collections.Generic;

namespace Stamp.CLI.Template
{
    interface IParameter
    {
        string Name { get; }

        bool Required { get; }

        IReadOnlyList<IValidator> Validators { get; }
    }
}
