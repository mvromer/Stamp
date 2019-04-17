namespace Stamp.CLI.Template
{
    interface IValidator<T>
    {
        bool Validate( T value );
    }
}
