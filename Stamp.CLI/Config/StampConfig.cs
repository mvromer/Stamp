using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using PathLib;

using Stamp.CLI.Repository;

namespace Stamp.CLI.Config
{
    class StampConfig : IStampConfig
    {
        internal static IStampConfig Load()
        {
            const string DefaultStampConfigFolderName = "stamp";
            var appDataPath = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create );
            return new StampConfig( Paths.Create( appDataPath, DefaultStampConfigFolderName ) );
        }

        public IReadOnlyCollection<IRepository> LoadRepositories()
        {
            var repositories = GetDefaultRepositories();
            try
            {
                const string RepositoryConfigFileName = "repositories.yml";
                var repoConfigPath = this.RootDir.WithFilename( RepositoryConfigFileName );

                using( var repoConfigFile = repoConfigPath.Open( FileMode.Open ) )
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

        internal StampConfig( IPath rootDir )
        {
            this.RootDir = rootDir;
        }

        private IList<IRepository> GetDefaultRepositories()
        {
            return new List<IRepository>
            {
                new Repository.Repository( name: ".local", description: "Local repository" )
            };
        }

        private IPath RootDir { get; }
    }
}
