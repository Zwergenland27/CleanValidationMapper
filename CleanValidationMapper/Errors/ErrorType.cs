namespace CleanValidationMapper.Errors;

/// <summary>
/// Different error types that can occur
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// A data conflict occured
    /// </summary>
    /// <example>An duplicate entity is inserted into the database</example>
    Conflict,
    /// <summary>
    /// The entity could not be found
    /// </summary>
    NotFound,
    /// <summary>
    /// An validation error occured
    /// </summary>
    /// <example>A required parameter is missing</example>
    Validation,
    /// <summary>
    /// An unexpected error occured
    /// </summary>
    Unexpected
}
