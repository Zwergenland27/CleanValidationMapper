namespace CleanValidationMapper;

public interface ICanFail<T> : ICanFail
{
	public static readonly InvalidOperationException ValueNotSet = new InvalidOperationException("The value did not get set");
	/// <summary>
	/// The normally returned value
	/// </summary>
	T Value { get; }
}
