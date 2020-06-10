using System.IO;

using PathLib;

namespace Stamp.CLI.Template
{
    /// <summary>
    /// Loads Stamp templates from various sources.
    /// </summary>
    interface ITemplateLoader
    {
        /// <summary>
        /// Loads a template rooted at the given path.
        /// </summary>
        /// <param name="templatePath">Path to the directory containing the template. This path is
        /// assumed to have the template's manifest file within it.</param>
        /// <returns>
        /// A template object defined by the contents of the manifest stored in the given template
        /// path.
        /// </returns>
        ITemplate LoadFromTemplateDirectory( IPurePath templatePath );
    }
}
