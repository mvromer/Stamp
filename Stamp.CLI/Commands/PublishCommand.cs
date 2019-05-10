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
            console.WriteLine( $"Template directory to publish: {this.TemplateDirectory}" );
            return 0;
        }

        private ILogger<PublishCommand> Logger { get; }
    }
}
