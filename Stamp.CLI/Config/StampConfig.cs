using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using PathLib;
using SystemEnvironment.Abstractions;

using Stamp.CLI.Repository;

namespace Stamp.CLI.Config
{
    class StampConfig : IStampConfig
    {
        public IPurePath RootDir
        {
            get { return m_rootDir.Value; }
        }
        private Lazy<IPurePath> m_rootDir;

        internal static string RootDirName { get; } = "stamp";

        internal StampConfig( ISystemEnvironment systemEnvironment )
        {
            this.SystemEnvironment = systemEnvironment;

            m_rootDir = new Lazy<IPurePath>( () => {
                var appDataPath = this.SystemEnvironment.GetFolderPath( Environment.SpecialFolder.ApplicationData,
                    Environment.SpecialFolderOption.Create );
                return PurePath.Create( appDataPath, StampConfig.RootDirName );
            } );
        }

        private ISystemEnvironment SystemEnvironment { get; }
    }
}
