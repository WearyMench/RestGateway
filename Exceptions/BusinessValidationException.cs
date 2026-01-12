namespace RestGateway.Exceptions;

/// <summary>
/// Exception for business validation errors from SOAP service
/// </summary>
public class BusinessValidationException : Exception
{
    public BusinessValidationException(string message) : base(message)
    {
    }

    public BusinessValidationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
