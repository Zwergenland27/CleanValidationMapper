using CleanValidationMapper.Errors;

namespace CleanValidationMapper;

public sealed record CanFail<T> : AbstractCanFail, ICanFail<T>
{

	private T _value = default!;

	private bool _valueSet;

	public CanFail() : base()
	{
		_valueSet = false;
	}

	/// <summary>
	/// The normally returned value 
	/// </summary>
	public T Value
	{
		get
		{
			if (_errors.Any()) throw new InvalidOperationException("Cannot get value from CanFail result that failed");
			if (_valueSet == false) throw new InvalidOperationException("The value has not been set yet");
			return _value;
		}
	}

	public CanFail<T> Succeded(T value)
	{
		_valueSet = true;
		_value = value;

		return this;
	}

	/// <summary>
	/// Create <see cref="CanFail<typeparamref name="T"/>"/> from <see cref="Error"/>
	/// </summary>
	public static implicit operator CanFail<T>(Error error)
	{
		var canFail = new CanFail<T>();
		canFail.Failed(error);
		return canFail;
	}

	/// <summary>
	/// Create <see cref="CanFail<typeparamref name="T"/>"/> with valid parameter <paramref name="value"/>
	/// </summary>
	public static implicit operator CanFail<T>(T value)
	{
		var canFail = new CanFail<T>();
		canFail.Succeded(value);
		return canFail;
	}
}
