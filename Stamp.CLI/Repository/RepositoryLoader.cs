using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using PathLib;
using System.IO.Abstractions;

using Stamp.CLI.Config;

namespace Stamp.CLI.Repository
{
    class RepositoryLoader : IRepositoryLoader
    {
        internal RepositoryLoader( IFileSystem fileSystem,
            IStampConfig stampConfig )
        {
            this.FileSystem = fileSystem;
            this.StampConfig = stampConfig;
        }

        public IReadOnlyCollection<IRepository> LoadRepositories()
        {
            var repositories = GetDefaultRepositories();
            try
            {
                const string RepositoryConfigFileName = "repositories.yml";
                var repoConfigPath = this.StampConfig.RootDir.Join( RepositoryConfigFileName );

                using( var repoConfigFile = this.FileSystem.File.Open( repoConfigPath.ToString(), FileMode.Open ) )
                {
                    // TODO: Implement repository config file parsing.
                }
            }
            catch( FileNotFoundException )
            {
                // Ignore this exception. We'll just return the default collection only contains the
                // local repository.
            }

            return new ReadOnlyCollection<IRepository>( repositories );
        }

        private IList<IRepository> GetDefaultRepositories()
        {
            return new List<IRepository>
            {
                new Repository( name: ".local", description: "Local repository" )
            };
        }

        private IFileSystem FileSystem { get; }
        private IStampConfig StampConfig { get; }
    }
}
