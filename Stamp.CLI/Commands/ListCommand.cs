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

        private int OnExecute( IConsole console )
        {
            console.WriteLine( $"Listing for {this.RepositoryName ?? "all"}" );
            return 0;
        }
    }
}
