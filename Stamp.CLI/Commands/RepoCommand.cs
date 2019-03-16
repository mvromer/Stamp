using McMaster.Extensions.CommandLineUtils;

namespace Stamp.CLI.Commands
{
    [Command( Description = "Manage configured template repositories.",
        ExtendedHelpText = "If no command is given, 'list' is assumed." )]
    [Subcommand( typeof(Repo.ListCommand) )]
    class RepoCommand
    {
        private int OnExecute( IConsole console )
        {
            return Repo.ListCommand.ListRepositories( console ) ? 0 : 1;
        }
    }
}
