using Microsoft.Extensions.Logging;

using McMaster.Extensions.CommandLineUtils;

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

        private int OnExecute()
        {
            return 0;
        }

        private ILogger<ListCommand> Logger { get; }
    }
}
