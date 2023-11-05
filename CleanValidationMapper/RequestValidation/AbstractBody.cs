namespace CleanValidationMapper;

public class AbstractBody<TValidated>
{
	List<BodyParameter> _parameters = new();

	protected BodyParameterReference<TValidated, TParameter> Parameter<TParameter>(TParameter? parameter) where TParameter : notnull
	{
		var bodyParameter = new BodyParameterReference<TValidated, TParameter>(parameter);
		_parameters.Add(bodyParameter);
		return bodyParameter;
	}

	protected BodyParameterStruct<TValidated, TParameter> Parameter<TParameter>(TParameter? parameter) where TParameter : struct
	{
		var bodyParameter = new BodyParameterStruct<TValidated, TParameter>(parameter);
		_parameters.Add(bodyParameter);
		return bodyParameter;
	}

	public CanFail<TValidated> Validate()
	{
		CanFail<TValidated> result = new();

		_parameters.ForEach(parameter =>
		{
			result.InheritFailure(parameter.ValidationResult);
		});

		if (result.HasFailed) return result;

		//Construct the TValidated

		return result;
	}
}
