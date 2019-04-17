using System.Collections.Generic;

namespace Stamp.CLI.Template
{
    interface IParameter
    {
        string Name { get; }

        bool Required { get; }
    }

    interface IParameter<T> : IParameter
    {
        IReadOnlyList<IValidator<T>> Validators { get; }
    }
}
