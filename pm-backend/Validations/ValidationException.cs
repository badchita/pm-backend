using System;
using System.Collections.Generic;

public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
    {
        Errors = errors;
    }
}
