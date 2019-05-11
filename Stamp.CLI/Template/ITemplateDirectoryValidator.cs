using PathLib;

namespace Stamp.CLI.Template
{
    interface ITemplateDirectoryValidator
    {
        void Validate( IPurePath templateDir );
    }
}
