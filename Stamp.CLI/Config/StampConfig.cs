using System;
using PathLib;

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

        internal StampConfig( IPath rootDir )
        {
            this.RootDir = rootDir;
        }

        private IPath RootDir { get; }
    }
}
