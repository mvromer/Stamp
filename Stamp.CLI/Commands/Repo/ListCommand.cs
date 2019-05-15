using System;
using System.Collections.Generic;
using System.Data;

using ConsoleTableExt;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

using Stamp.CLI.Config;
using Stamp.CLI.Repository;

namespace Stamp.CLI.Commands.Repo
{
    [Command( Description = "List all configured template repositories." )]
    class ListCommand
    {
        public ListCommand( ILogger<ListCommand> logger,
            IRepositoryLoader repositoryLoader )
        {
            this.Logger = logger;
            this.RepositoryLoader = repositoryLoader;
        }

        private int OnExecute( IConsole console )
        {
            try
            {
                var repositories = this.RepositoryLoader.LoadRepositories();
                var repoTable = BuildRepositoryTable( repositories );
                var tableOutput = ConsoleTableBuilder
                    .From( repoTable )
                    .WithFormat( ConsoleTableBuilderFormat.Minimal )
                    .Export();

                console.WriteLine( tableOutput );
                return 0;
            }
            catch( Exception ex )
            {
                this.Logger.LogError( ex, "Failed to load available repositories." );
                return 1;
            }
        }

        private DataTable BuildRepositoryTable( IEnumerable<IRepository> repositories )
        {
            const string NameColumn = "Name";
            const string DescriptionColumn = "Description";

            var repoTable = new DataTable();
            repoTable.Columns.AddRange( new DataColumn[] {
                new DataColumn( NameColumn, typeof(string) ),
                new DataColumn( DescriptionColumn, typeof(string) )
            } );

            foreach( var repo in repositories )
            {
                var row = repoTable.NewRow();
                row[NameColumn] = repo.Name;
                row[DescriptionColumn] = repo.Description;
                repoTable.Rows.Add( row );
            }

            return repoTable;
        }

        private ILogger<ListCommand> Logger { get; }
        private IRepositoryLoader RepositoryLoader { get; }
    }
}
