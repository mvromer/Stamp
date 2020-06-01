using System.IO;

using PathLib;

namespace Stamp.CLI.Template
{
    interface ITemplateLoader
    {
        /// <summary>
        /// Loads a template from its manifest stored at the given path.
        /// </summary>
        /// <param name="manifestPath">Path to the manifest file for the template to load.</param>
        /// <returns>
        /// A template object defined by the contents of the given manifest.
        /// </returns>
        ITemplate LoadFromManifest( IPurePath manifestPath );

        /// <summary>
        /// Loads a template from its manifest currently opened by the given <c>TextReader</c>.
        /// </summary>
        /// <param name="reader"><c>TextReader</c> opened to the manifest of the template to load.</param>
        /// <returns>
        /// A template object defined by the contents of manifest opened by the given
        /// <c>TextReader</c>.
        /// </returns>
        ITemplate LoadFromReader( TextReader reader );

        ITemplate FindTemplate( string templateName );
    }
}
