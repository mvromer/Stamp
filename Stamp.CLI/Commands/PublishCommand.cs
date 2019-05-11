using System;
using System.IO;

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace Stamp.CLI.Commands
{
    [Command( Description = "Publishes a template to its repository." )]
    class PublishCommand
    {
        [Argument( order: 0,
            name: "<TEMPLATE DIRECTORY>",
            description: "Directory containing template to publish. Defaults to current directory." )]
        public string TemplateDirectory { get; }

        public PublishCommand( ILogger<PublishCommand> logger, IFileSystem fileSystem )
        {
            this.Logger = logger;
            this.FileSystem = fileSystem;
        }

        private int OnExecute( IConsole console )
        {
            try
            {
                var templateDir = string.IsNullOrEmpty( this.TemplateDirectory ) ?
                    this.FileSystem.Directory.GetCurrentDirectory() : this.TemplateDirectory;

                if( !this.FileSystem.Directory.Exists( templateDir ) )
                    throw new DirectoryNotFoundException( $"The template directory {templateDir} does not exist." );

                console.WriteLine( $"Template directory to publish: {templateDir}" );
                return 0;
            }
            catch( Exception ex )
            {
                this.Logger.LogError( ex, "Failed to publish template." );
                return 1;
            }
        }

        private ILogger<PublishCommand> Logger { get; }

        private IFileSystem FileSystem { get; }
    }
}
