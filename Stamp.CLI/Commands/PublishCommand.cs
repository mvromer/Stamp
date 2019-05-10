using System;
using System.IO;

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Stamp.CLI.Commands
{
    [Command( Description = "Publishes a template to its repository." )]
    class PublishCommand
    {
        [Argument( order: 0,
            name: "<TEMPLATE DIRECTORY>",
            description: "Directory containing template to publish. Defaults to current directory." )]
        public string TemplateDirectory { get; }

        public PublishCommand( ILogger<PublishCommand> logger )
        {
            this.Logger = logger;
        }

        private int OnExecute( IConsole console )
        {
            try
            {
                string templateDir = string.IsNullOrEmpty( this.TemplateDirectory ) ?
                    Directory.GetCurrentDirectory() : this.TemplateDirectory;

                if( !Directory.Exists( templateDir ) )
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
    }
}
