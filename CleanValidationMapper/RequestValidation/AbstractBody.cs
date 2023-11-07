using CleanValidationMapper.RequestValidation;

namespace CleanValidationMapper;

public abstract class AbstractBody<TValidated> where TValidated : notnull
{
    protected readonly RequiredReferenceProperty<TValidated> _propertyBuilder;

    public AbstractBody()
    {
        _propertyBuilder = new RequiredReferenceProperty<TValidated>(nameof(TValidated));
    }

    public CanFail<TValidated> Validate()
	{
        var validationResult = _propertyBuilder.Create();

        if(validationResult.HasFailed)
        {
            var result = new CanFail<TValidated>();
            result.InheritFailure(validationResult);
            return result;
        }
        else
        {
            return (TValidated)validationResult.Value!;
        }
    }
}