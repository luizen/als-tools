namespace AlsTools.Ui.Cli;

public abstract class BaseCommandHandler
{
    public BaseCommandHandler(IOptions<ParameterValuesOptions> parameterValuesOptions)
    {
        ParameterValuesOptions = parameterValuesOptions;
    }

    protected IOptions<ParameterValuesOptions> ParameterValuesOptions { get; }
}
