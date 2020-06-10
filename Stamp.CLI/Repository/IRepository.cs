using System.Collections.Immutable;

using PathLib;

using Stamp.CLI.Template;

namespace Stamp.CLI.Repository
{
    interface IRepository
    {
        string Name { get; }

        string Description { get; }

        IPurePath RootPath { get; }

        ImmutableList<ITemplate> Templates { get; }
    }
}
