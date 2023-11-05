namespace CleanValidationMapper.Errors;

public enum FailureType
{
	/// <summary>
	/// No errors occured
	/// </summary>
	None,
	/// <summary>
	/// One data conflict occured
	/// </summary>
	SingleConflict,
	/// <summary>
	/// Multiple data conflicts occured
	/// </summary>
	MultipleConflict,
	/// <summary>
	/// One not found error occured
	/// </summary>
	SingleNotFound,
	/// <summary>
	/// Multiple not found errors occured
	/// </summary>
	MultipleNotFound,
	/// <summary>
	/// One validation error occured
	/// </summary>
	SingleValidation,
	/// <summary>
	/// Multiple validation errors occured
	/// </summary>
	MultipleValidation,
	/// <summary>
	/// One unexpected error occured
	/// </summary>
	SingleUnexpected,
	/// <summary>
	/// Multiple unexpected errors occured
	/// </summary>
	MultipleUnexpected,
	/// <summary>
	/// Multiple differnt errors occured
	/// </summary>
	MultipleDifferent
}
