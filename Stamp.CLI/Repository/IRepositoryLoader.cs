using System.Collections.Generic;

namespace Stamp.CLI.Repository
{
    interface IRepositoryLoader
    {
        IReadOnlyCollection<IRepository> LoadRepositories();
    }
}
