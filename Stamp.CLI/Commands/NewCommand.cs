using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace Stamp.CLI.Commands
{
    [Command( Description = "Create a new instance of a named template." )]
    class NewCommand
    {
        [Argument( order: 0,
            name: "<TEMPLATE NAME>",
            description: "Name of the template to instantiate." )]
        [Required]
        public string TemplateName { get; }

        private int OnExecute( IConsole console )
        {
            console.WriteLine( $"Instantiating {this.TemplateName}" );
            return 0;
        }
    }
}
