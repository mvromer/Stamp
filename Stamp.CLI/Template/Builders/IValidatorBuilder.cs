namespace Stamp.CLI.Template.Builders
{
    interface IValidatorBuilder
    {
        IValidator<T> Build<T>();
    }
}
