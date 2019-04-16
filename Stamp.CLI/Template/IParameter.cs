namespace Stamp.CLI.Template
{
    interface IParameter
    {
        string Name { get; }

        bool Required { get; }
    }
}
