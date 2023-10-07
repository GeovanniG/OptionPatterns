namespace OptionPatternsWithValidation;

public sealed class AppOptions : IOperationLifetime
{
    public string ApiKey { get; set; } = null!;
    public Guid OperationId => _guid;
    
    private readonly Guid _guid;
    
    public AppOptions() : this(Guid.NewGuid())
    { }
    
    private AppOptions(Guid guid)
    {
        _guid = guid;
    }

}

public interface IOperationLifetime
{
    Guid OperationId { get; }
}