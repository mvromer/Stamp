using System.Collections.Generic;
using System.IO;

using PathLib;
using System.IO.Abstractions;

using Stamp.CLI.Config;

namespace Stamp.CLI.Repository
{
    class RepositoryLoader : IRepositoryLoader
    {
        public RepositoryLoader( IFileSystem fileSystem,
            IStampConfig stampConfig )
        {
            this.FileSystem = fileSystem;
            this.StampConfig = stampConfig;
        }

        public IEnumerable<IRepository> LoadRepositories()
        {
            foreach( var repo in GetDefaultRepositories() )
                yield return repo;

            try
            {
                const string RepositoryConfigFileName = "repositories.yml";
                var repoConfigPath = this.StampConfig.RootDir.Join( RepositoryConfigFileName );

                using( var repoConfigFile = this.FileSystem.File.Open( repoConfigPath.ToString(), FileMode.Open ) )
                {
                    // TODO: Implement repository config file parsing.
                }
            }
            catch( IOException ex ) when (
                ex is DirectoryNotFoundException ||
                ex is FileNotFoundException
            )
            {
                // Ignore these exceptions. When the repository config doesn't exist for whatever
                // reason, we'll just return the default collection which only contains the local
                // repository.
            }
        }

        private IEnumerable<IRepository> GetDefaultRepositories()
        {
            yield return new Repository( name: ".local", description: "Local repository" );
        }

        private IFileSystem FileSystem { get; }
        private IStampConfig StampConfig { get; }
    }
}
