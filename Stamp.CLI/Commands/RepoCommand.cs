using System.Linq;

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Stamp.CLI.Commands
{
    [Command( Description = "Manage configured template repositories.",
        ExtendedHelpText = "If no command is given, 'list' is assumed." )]
    [Subcommand( typeof(Repo.ListCommand) )]
    class RepoCommand
    {
        private int OnExecute( IConsole console, CommandLineApplication<RepoCommand> command )
        {
            const string ListRepoCommandName = "list";
            return command.Commands
                .Where( c => c.Name == ListRepoCommandName )
                .First()
                .Invoke();
        }
    }
}
