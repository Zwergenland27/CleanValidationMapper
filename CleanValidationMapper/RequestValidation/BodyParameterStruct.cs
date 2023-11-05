namespace CleanValidationMapper;

public sealed class BodyParameterStruct<TValidated, TParameter> : BodyParameter where TParameter : struct
{
	private readonly TParameter? _inParameter;
	public BodyParameterStruct(TParameter? inParameter)
	{
		_inParameter = inParameter;
	}

	public TParameter IsRequired(Error missingError)
	{
		if (_inParameter is null)
		{
			_validationResult.Failed(missingError);
			return default!;
		}

		return _inParameter.Value;
	}

	public TParameter? IsOptional()
	{
		return _inParameter;
	}
}