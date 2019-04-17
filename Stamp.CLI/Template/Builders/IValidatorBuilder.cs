namespace Stamp.CLI.Template.Builders
{
    interface IValidatorBuilder
    {
        IValidator Build<T>();
    }
}
