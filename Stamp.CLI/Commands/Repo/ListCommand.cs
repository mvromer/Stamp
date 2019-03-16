using System;
using McMaster.Extensions.CommandLineUtils;

namespace Stamp.CLI.Commands.Repo
{
    [Command( Description = "List all configured template repositories." )]
    class ListCommand
    {
        internal static bool ListRepositories( IConsole console )
        {
            console.WriteLine( "Listing repositories." );
            return true;
        }

        private int OnExecute( IConsole console )
        {
            return ListRepositories( console ) ? 0 : 1;
        }
    }
}
