using System.IO;

using PathLib;

namespace Stamp.CLI.Template
{
    interface ITemplateLoader
    {
        ITemplate LoadFromManifest( IPurePath manifestPath );

        ITemplate LoadFromReader( TextReader reader );

        ITemplate FindTemplate( string templateName );
    }
}
