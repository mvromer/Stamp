using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Stamp.CLI.Template
{
    class TemplateValidator : ITemplateValidator
    {
        public void ValidateName( string templateName )
        {
            if( !this.NamePattern.IsMatch( templateName ) )
                throw new ValidationException( $"Invalid template name {templateName}." );
        }

        private Regex NamePattern { get; } = new Regex( @"^[A-Za-z](?:[_A-Za-z0-9.\-]*[A-Za-z0-9])?$" );
    }
}
