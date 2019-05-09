using System;

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

using Stamp.CLI.Config;

namespace Stamp.CLI.Commands
{
    [Command( Description = "List available templates." )]
    class ListCommand
    {
        [Argument( order: 0,
            name: "<REPOSITORY NAME>",
            description: "List templates available through the named repository." )]
        public string RepositoryName { get; }

        public ListCommand( ILogger<ListCommand> logger )
        {
            this.Logger = logger;
        }

        private int OnExecute( IConsole console )
        {
            try
            {
                var stampConfig = StampConfig.Load();
                return 0;
            }
            catch( Exception ex )
            {
                this.Logger.LogError( ex, "Failed to list available templates." );
                return 1;
            }
        }

        private ILogger<ListCommand> Logger { get; }
    }
}
