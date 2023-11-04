namespace CleanValidationMapper;

public sealed record CanFail : ICanFail
{
	private readonly List<Error> _errors = new();

	/// <summary>
	/// List of all errors that occured
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when no errors occured</exception>
	public IReadOnlyList<Error> Errors => _errors.Any() ? _errors.AsReadOnly() : throw new InvalidOperationException("Cannot get errors from CanFail result that did not fail");

	/// <summary>
	/// Indicates that one ore more errors have occured
	/// </summary>
	public bool HasFailed => _errors.Any();

    //TODO: get summary for each error type - One or more validation errors have occured
    //TODO: handle different error types - multiple error type field

	/// <summary>
	/// One error occured
	/// </summary>
	public CanFail Failed(Error error)
	{
		_errors.Add(error);
		return this;
	}

	/// <summary>
	/// Adds the errors of an <see cref="ICanFail"/> object if it has failed
	/// </summary>
	public CanFail Failed(ICanFail canFailResult)
	{
		if (canFailResult.HasFailed) _errors.AddRange(canFailResult.Errors);
		return this;
	}

	/// <summary>
	/// Create <see cref="CanFail"/> from <see cref="Error"/>
	/// </summary>
	public static implicit operator CanFail(Error error)
	{
		var canFail = new CanFail();
		canFail.Failed(error);
		return canFail;
	}
}
