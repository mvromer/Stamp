using System;
using System.Runtime.CompilerServices;
using McMaster.Extensions.CommandLineUtils;

[assembly: CLSCompliant( isCompliant: false )]
[assembly: InternalsVisibleTo( "Stamp.Tests" )]

namespace Stamp.CLI
{
    [Command( Name = "stamp", Description = "Stamp template generator." )]
    [Subcommand( typeof(Commands.NewCommand) )]
    [Subcommand( typeof(Commands.RepoCommand) )]
    class Program
    {
        static void Main( string[] args ) =>
            CommandLineApplication.Execute<Program>( args );

        private void OnExecute( CommandLineApplication app ) => app.ShowHelp();
    }
}
