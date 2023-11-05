namespace CleanValidationMapper.Errors;

public record AbstractCanFail : ICanFail
{
	protected readonly List<Error> _errors;

	public AbstractCanFail()
	{
		_errors = new();
	}

	/// <summary>
	/// List of all errors that occured
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when no errors occured</exception>
	public IReadOnlyList<Error> Errors => _errors.Any() ? _errors.AsReadOnly() : throw new InvalidOperationException("Cannot get errors from CanFail result that did not fail");

	/// <summary>
	/// Indicates that one ore more errors have occured
	/// </summary>
	public bool HasFailed => _errors.Any();

	public FailureType Type
	{
		get
		{
			if (!_errors.Any()) return FailureType.None;

			ErrorType firstErrorType = _errors[0].Type;
			bool differentErrorTypes = false;
			_errors.ForEach(error =>
			{
				if(error.Type != firstErrorType) differentErrorTypes = true;
			});

			if (differentErrorTypes) return FailureType.MultipleDifferent;

			if (_errors.Count == 1)
			{
				return firstErrorType switch
				{
					ErrorType.Conflict => FailureType.SingleConflict,
					ErrorType.NotFound => FailureType.SingleNotFound,
					ErrorType.Validation => FailureType.SingleValidation,
					ErrorType.Unexpected => FailureType.SingleUnexpected,
					_ => FailureType.SingleUnexpected
				};
			}

			return firstErrorType switch
			{
				ErrorType.Conflict => FailureType.MultipleConflict,
				ErrorType.NotFound => FailureType.MultipleNotFound,
				ErrorType.Validation => FailureType.MultipleValidation,
				ErrorType.Unexpected => FailureType.MultipleUnexpected,
				_ => FailureType.MultipleUnexpected
			};
		}
	}

	/// <summary>
	/// One error occured
	/// </summary>
	public AbstractCanFail Failed(Error error)
	{
		_errors.Add(error);
		return this;
	}

	/// <summary>
	/// Adds the errors of an <see cref="ICanFail"/> object if it has failed
	/// </summary>
	public AbstractCanFail Failed(AbstractCanFail canFailResult)
	{
		if (canFailResult.HasFailed) _errors.AddRange(canFailResult.Errors);
		return this;
	}
}
