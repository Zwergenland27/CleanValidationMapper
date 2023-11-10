namespace CleanValidationMapper.RequestValidationTests;

public abstract class AbstractBody<TValidated> where TValidated : notnull
{
    private readonly RequiredReference<TValidated> _propertyBuilder;

    public AbstractBody()
    {
        _propertyBuilder = new RequiredReference<TValidated>(nameof(TValidated));
    }

    protected abstract void Configure(RequiredReference<TValidated> propertyBuilder);

    public CanFail<TValidated> Validate()
    {
        var result = new CanFail<TValidated>();

        Configure(_propertyBuilder);
        var validationResult = _propertyBuilder.Create();

        result.InheritFailure(validationResult);
        if (result.HasFailed) return result;

        return result.SetValue((TValidated)validationResult.Value!);
    }
}