using System.Collections.Generic;

namespace Stamp.CLI.Repository
{
    interface IRepositoryLoader
    {
        /// <summary>
        /// Loads and returns all configured remote repositories.
        /// </summary>
        /// <returns>
        /// Enumerable of all configured remote repositories.
        /// </returns>
        IEnumerable<IRepository> LoadRepositories();

        /// <summary>
        /// Load and return the local repository.
        /// </summary>
        /// <returns>
        /// Local repository object.
        /// </returns>
        IRepository LoadLocalRepository();
    }
}
