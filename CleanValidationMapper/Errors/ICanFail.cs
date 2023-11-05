using CleanValidationMapper.Errors;

namespace CleanValidationMapper;

public interface ICanFail
{
	/// <summary>
	/// Gets the list of errors
	/// </summary>
	IReadOnlyList<Error> Errors { get; }

	/// <summary>
	/// Indicates that one ore more errors have occured
	/// </summary>
	public bool HasFailed { get; }

	public FailureType Type { get; }
}
