namespace AlsTools.Ui.Cli.Exceptions;

[Serializable]
public class CommandLineParseException : Exception
{
    public IEnumerable<Error>? Errors { get; private set; }

    public CommandLineParseException() { }


    public CommandLineParseException(IEnumerable<Error> errors)
    {
        Errors = errors;
    }

    public CommandLineParseException(string message) : base(message) { }

    public CommandLineParseException(string message, IEnumerable<Error> errors) : base(message)
    {
        Errors = errors;
    }
    public CommandLineParseException(string message, System.Exception inner) : base(message, inner) { }

    public CommandLineParseException(string message, IEnumerable<Error> errors, System.Exception inner) : base(message, inner)
    {
        Errors = errors;
    }
}