using System;

namespace Stamp.CLI.Script
{
    class ComputedString
    {
        internal ComputedString( string expression )
        {
            m_value = new Lazy<string>( () => ComputeValue( expression ) );
        }

        internal string Value
        {
            get { return m_value.Value; }
        }

        private Lazy<string> m_value;

        private static string ComputeValue( string expression )
        {
            return expression;
        }
    }
}
