namespace Impress.Validation
{
    public interface IInvalidationReason
    {
        InvalidationSeverity Severity { get; }
        string MessageKey { get; }
        object[] MessageParameters { get; }
    }
}
