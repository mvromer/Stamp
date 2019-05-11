using System.IO;

using PathLib;

namespace Stamp.CLI.Template
{
    interface ITemplateFactory
    {
        ITemplate CreateFromManifest( IPurePath manifestPath );

        ITemplate CreateFromReader( TextReader reader );
    }
}
