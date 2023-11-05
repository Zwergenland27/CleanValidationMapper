namespace CleanValidationMapper;

public sealed class BodyParameterReference<TValidated, TParameter> : BodyParameter where TParameter : notnull
{
    private readonly TParameter? _inParameter;
    public BodyParameterReference(TParameter? inParameter)
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

        return _inParameter;
    }

    public TParameter? IsOptional()
    {
        return _inParameter;
    }
}