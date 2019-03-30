using System;

namespace Stamp.CLI.Script
{
    class ComputedString
    {
        internal ComputedString( string value )
        {
            this.Value = value;
            m_computedValue = new Lazy<string>( () => ComputeValue( value ) );
        }

        internal string Value { get; }

        internal string ComputedValue
        {
            get { return m_computedValue.Value; }
        }

        private Lazy<string> m_computedValue;

        private static string ComputeValue( string value )
        {
            return value;
        }
    }
}
