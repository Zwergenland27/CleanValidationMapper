using CleanValidationMapper.Errors;

namespace CleanValidationMapper;

public sealed record CanFail<T> : AbstractCanFail, ICanFail<T>
{

	private T _value = default!;

	private bool _valueSet;

	public CanFail() : base()
	{
		if (typeof(T).IsAssignableTo(typeof(AbstractCanFail))) throw new InvalidOperationException($"You cannot create a CanFail result that contains a {typeof(T)}");
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

	public CanFail<T> SetValue(T value)
	{
        if (value is not null && value.GetType().IsAssignableTo(typeof(AbstractCanFail))) throw new InvalidOperationException($"You cannot set the value of CanFail<{typeof(T)}> to {value.GetType()}");
        _valueSet = true;
		_value = value;

		return this;
	}

	public CanFail<T> Inherit(CanFail<T> result)
	{
        InheritFailure(result);
		if (!HasFailed)
		{
			SetValue(result.Value);
		}

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
}
