using System.Collections.Generic;

using Stamp.CLI.Repository;

namespace Stamp.CLI.Config
{
    interface IStampConfig
    {
        IReadOnlyCollection<IRepository> LoadRepositories();
    }
}
