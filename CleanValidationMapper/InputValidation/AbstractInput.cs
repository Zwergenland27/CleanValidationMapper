using CleanValidationMapper.InputValidation.Property;

namespace CleanValidationMapper.InputValidation;

public abstract class ReturnsReference<TResult> where TResult : notnull
{
    private readonly RequiredReferenceBuilder<TResult> _propertyBuilder;

    public ReturnsReference()
    {
        _propertyBuilder = new RequiredReferenceBuilder<TResult>(nameof(TResult));
    }

    protected abstract void Configure(RequiredReferenceBuilder<TResult> propertyBuilder);

    public CanFail<TResult> Validate()
    {
        var result = new CanFail<TResult>();

        Configure(_propertyBuilder);
        var validationResult = _propertyBuilder.Create();

        result.InheritFailure(validationResult);
        if (result.HasFailed) return result;

        return result.SetValue((TResult)validationResult.Value!);
    }
}

public abstract class ReturnsStruct<TResult> where TResult : struct
{
    private readonly RequiredStructBuilder<TResult> _propertyBuilder;

    public ReturnsStruct()
    {
        _propertyBuilder = new RequiredStructBuilder<TResult>(nameof(TResult));
    }

    protected abstract void Configure(RequiredStructBuilder<TResult> propertyBuilder);

    public CanFail<TResult> Validate()
    {
        var result = new CanFail<TResult>();

        Configure(_propertyBuilder);
        var validationResult = _propertyBuilder.Create();

        result.InheritFailure(validationResult);
        if (result.HasFailed) return result;

        return result.SetValue((TResult)validationResult.Value!);
    }
}