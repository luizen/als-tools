using CommandLine;
using System;
using System.Collections.Generic;

namespace AlsTools.Exceptions;

[System.Serializable]
public class CommandLineParseException : Exception
{
    IEnumerable<Error> Errors { get; set; }

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
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}