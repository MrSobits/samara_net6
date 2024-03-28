
namespace Bars.Gkh.Exceptions;

using System;

/// <summary>
/// The exception thrown to indicate that no instances are returned by a provider.Note: the WMI .NET libraries are now considered in final state,
/// and no further development, enhancements, or updates will be available for non-security related issues affecting these libraries.
/// The MI APIs should be used for all new development.
/// </summary>
/// <remarks><see cref="https://learn.microsoft.com/en-us/dotnet/api/system.management.instrumentation.instancenotfoundexception?view=netframework-4.8.1"/></remarks>
public class InstanceNotFoundException: Exception
{
    /// <summary>Initializes a new instance of the InstanceNotFoundException class.</summary>
    public InstanceNotFoundException()
    {
    }

    /// <summary>Initializes a new instance of the InstanceNotFoundException class with its message string set to message.</summary>
    /// <param name="message">A string that contains the error message that explains the reason for the exception.</param>
    public InstanceNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>Initializes a new instance of the InstanceNotFoundException class with the specified error message and the inner exception.</summary>
    /// <param name="message">A string that contains the error message that explains the reason for the exception.</param>
    /// <param name="innerException">The Exception that caused the current exception to be thrown.</param>
    public InstanceNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}