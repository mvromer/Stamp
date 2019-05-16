using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using PathLib;
using System.IO.Abstractions;
using SystemEnvironment.Abstractions;

using Stamp.CLI.Repository;

namespace Stamp.CLI.Config
{
    class StampConfig : IStampConfig
    {
        public IPurePath RootPath
        {
            get { return m_rootPath.Value; }
        }
        private Lazy<IPurePath> m_rootPath;

        public IPurePath GetRepositoryPath( string repoName )
        {
            var repoPath = this.RootPath.Join( StampConfigConstants.RepositoriesDirectoryName, repoName );
            return this.FileSystem.Directory.Exists( repoPath.ToString() ) ? repoPath : null;
        }

        public StampConfig( ISystemEnvironment systemEnvironment, IFileSystem fileSystem )
        {
            this.SystemEnvironment = systemEnvironment;
            this.FileSystem = fileSystem;

            m_rootPath = new Lazy<IPurePath>( () => {
                var appDataPath = this.SystemEnvironment.GetFolderPath( Environment.SpecialFolder.ApplicationData,
                    Environment.SpecialFolderOption.Create );
                return PurePath.Create( appDataPath, StampConfigConstants.ConfigDirectoryName );
            } );
        }

        private ISystemEnvironment SystemEnvironment { get; }
        private IFileSystem FileSystem { get; }
    }
}
