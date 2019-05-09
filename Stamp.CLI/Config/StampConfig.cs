using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var repositories = new List<IRepository>();
            return new ReadOnlyCollection<IRepository>( repositories );
        }

        internal StampConfig( IPath rootDir )
        {
            this.RootDir = rootDir;
        }

        private IPath RootDir { get; }
    }
}
