using System.Collections.Generic;

namespace Stamp.CLI.Repository
{
    interface IRepositoryLoader
    {
        IEnumerable<IRepository> LoadRepositories();
    }
}
