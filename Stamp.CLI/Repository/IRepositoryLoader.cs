using System.Collections.Generic;

namespace Stamp.CLI.Repository
{
    interface IRepositoryLoader
    {
        /// <summary>
        /// Loads and returns the local repository and all configured remote repositories.
        /// </summary>
        /// <returns>
        /// Enumerable containing the local repository and all configured remote repositories.
        /// </returns>
        IEnumerable<IRepository> LoadRepositories();
    }
}
