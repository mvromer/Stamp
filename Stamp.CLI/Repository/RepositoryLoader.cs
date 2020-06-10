using System;
using System.Collections.Generic;
using System.IO;

using PathLib;
using System.IO.Abstractions;

using Stamp.CLI.Config;
using Stamp.CLI.Template;
using Microsoft.Extensions.Logging;

namespace Stamp.CLI.Repository
{
    class RepositoryLoader : IRepositoryLoader
    {
        public RepositoryLoader( IFileSystem fileSystem, ITemplateLoader templateLoader, IStampConfig stampConfig,
            ILogger<RepositoryLoader> logger )
        {
            this.FileSystem = fileSystem;
            this.TemplateLoader = templateLoader;
            this.StampConfig = stampConfig;
            this.Logger = logger;
        }

        public IEnumerable<IRepository> LoadRepositories()
        {
            throw new NotImplementedException();

            // try
            // {
            //     const string RepositoryConfigFileName = "repositories.yml";
            //     var repoConfigPath = this.StampConfig.RootPath.Join( RepositoryConfigFileName );
            //     using var repoConfigFile = this.FileSystem.File.Open( repoConfigPath.ToString(), FileMode.Open );

            //     // TODO: Implement repository config file parsing.
            // }
            // catch( IOException ex ) when (
            //     ex is DirectoryNotFoundException ||
            //     ex is FileNotFoundException
            // )
            // {
            //     // Ignore these exceptions. When the repository config doesn't exist for whatever
            //     // reason, we'll just return the default collection which only contains the local
            //     // repository.
            // }
        }

        public IRepository LoadLocalRepository()
        {
            return new Repository( name: ".local",
                description: "Local repository",
                this.StampConfig.GetLocalRepositoryPath(),
                this.FileSystem,
                this.TemplateLoader,
                this.Logger );
        }

        private IFileSystem FileSystem { get; }
        private ITemplateLoader TemplateLoader { get; }
        private IStampConfig StampConfig { get; }
        private ILogger<RepositoryLoader> Logger { get; }
    }
}
