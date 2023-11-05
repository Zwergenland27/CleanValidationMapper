using CleanValidationMapper.Errors;

namespace CleanValidationMapper;

public sealed record CanFail : AbstractCanFail
{
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
