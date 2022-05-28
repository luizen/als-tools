namespace AlsTools.Ui.Cli.Exceptions;

[System.Serializable]
public class CommandLineParseException : Exception
{
    public IEnumerable<Error>? Errors { get; private set; }

    public CommandLineParseException() { }


    public CommandLineParseException(IEnumerable<Error> errors)
    {
        this.Errors = errors;
    }

    public CommandLineParseException(string message) : base(message) { }

    public CommandLineParseException(string message, IEnumerable<Error> errors) : base(message)
    {
        this.Errors = errors;
    }
    public CommandLineParseException(string message, System.Exception inner) : base(message, inner) { }

    public CommandLineParseException(string message, IEnumerable<Error> errors, System.Exception inner) : base(message, inner)
    {
        this.Errors = errors;
    }

    protected CommandLineParseException(
        SerializationInfo info,
        StreamingContext context) : base(info, context) { }
}