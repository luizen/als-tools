namespace AlsTools.Ui.Cli;

public interface IOptionCommandHandler<TOptionsType> where TOptionsType : ICliOptions
{
    Task Execute(TOptionsType options);
}
