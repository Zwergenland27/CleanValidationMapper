namespace CleanValidationMapper.Errors;
/// <summary>
/// Used to indicate an error instead of using exceptions
/// </summary>
public sealed record Error
{
    private Error(ErrorType type, string code, string message)
    {
        Type = type;
        Code = code;
        Message = message;
    }

    /// <summary>
    /// The unique error code
    /// </summary>
    /// <remarks>
    /// This should be a constant naming convention shared in the project containing
    /// at minimum the class in which the error can occur and a short description of the error
    /// </remarks>
    /// <example>Book.Title.Missing</example>
    public string Code { get; }

    /// <summary>
    /// More detailed deschription of the error
    /// </summary>
    /// <example>The book title cannot be empty</example>
    public string Message { get; }

    /// <summary>
    /// Type of the error
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Creates new Error of type Conflict
    /// </summary>
    public static Error Conflict(string code, string message)
    {
        CanFail test = new CanFail();
        return new Error(ErrorType.Conflict, code, message);
    }

    /// <summary>
    /// Creates new Error of type NotFound
    /// </summary>
    public static Error NotFound(string code, string message)
    {
        return new Error(ErrorType.NotFound, code, message);
    }

    /// <summary>
    /// Creates new Error of type Validation
    /// </summary>
    public static Error Validation(string code, string message)
    {
        return new Error(ErrorType.Validation, code, message);
    }

    /// <summary>
    /// Creates new Error of type Unexpected
    /// </summary>
    public static Error Unexpected(string code, string message)
    {
        return new Error(ErrorType.Unexpected, code, message);
    }
}
