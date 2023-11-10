using CleanValidationMapper.Errors;
using CleanValidationMapper.InputValidation.ValidatedProperty;
using System.Linq.Expressions;

namespace CleanValidationMapper.InputValidation.Property;

public abstract class RequiredPropertyBuilder<T> : PropertyBuilder<T>
{
    public RequiredPropertyBuilder(string name) : base(name)
    {

    }
}

public class RequiredReferenceBuilder<T> : RequiredPropertyBuilder<T>, IMappingsForRequired<T> where T : notnull
{
    public RequiredReferenceBuilder(string name) : base(name)
    {

    }

    #region Required

    public RequiredReferenceBuilder<TProp> MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public void MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<RequiredReferenceBuilder<TProp>> builder) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public RequiredStructBuilder<TProp> MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : struct
    {
        throw new NotImplementedException();
    }

    public void MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<RequiredStructBuilder<TProp>> builder) where TProp : struct
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Optional

    public OptionalReferenceBuilder<TProp> MapOptionalReference<TProp>(Expression<Func<T, TProp?>> propertyExpression) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public void MapOptionalReference<TProp>(Expression<Func<T, TProp?>> propertyExpression, Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public OptionalStructBuilder<TProp> MapOptionalStruct<TProp>(Expression<Func<T, TProp?>> propertyExpression) where TProp : struct
    {
        throw new NotImplementedException();
    }

    public void MapOptionalStruct<TProp>(Expression<Func<T, TProp?>> propertyExpression, Action<OptionalStructBuilder<TProp>> builder) where TProp : struct
    {
        throw new NotImplementedException();
    }

    #endregion

    public void FromParameter(T? parameter, Error missingError)
    {

    }

    public void ByCalling(Delegate creationMethod, Action<RequiredValidatedReferenceBuilder<T>> methodBuilder)
    {
        throw new NotImplementedException();
    }

    public override CanFail<object?> Create()
    {
        throw new NotImplementedException();
    }
}

public class RequiredStructBuilder<T> : RequiredPropertyBuilder<T>, IMappingsForRequired<T> where T : struct
{
    public RequiredStructBuilder(string name) : base(name)
    {

    }

    #region Required

    public RequiredReferenceBuilder<TProp> MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public void MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<RequiredReferenceBuilder<TProp>> builder) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public RequiredStructBuilder<TProp> MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : struct
    {
        throw new NotImplementedException();
    }

    public void MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<RequiredStructBuilder<TProp>> builder) where TProp : struct
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Optional

    public OptionalReferenceBuilder<TProp> MapOptionalReference<TProp>(Expression<Func<T, TProp?>> propertyExpression) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public void MapOptionalReference<TProp>(Expression<Func<T, TProp?>> propertyExpression, Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public OptionalStructBuilder<TProp> MapOptionalStruct<TProp>(Expression<Func<T, TProp?>> propertyExpression) where TProp : struct
    {
        throw new NotImplementedException();
    }

    public void MapOptionalStruct<TProp>(Expression<Func<T, TProp?>> propertyExpression, Action<OptionalStructBuilder<TProp>> builder) where TProp : struct
    {
        throw new NotImplementedException();
    }

    #endregion

    public void FromParameter(T? parameter, Error missingError)
    {

    }

    public void ByCalling(Delegate creationMethod, Action<RequiredValidatedStructBuilder<T>> methodBuilder)
    {
        throw new NotImplementedException();
    }

    public override CanFail<object?> Create()
    {
        throw new NotImplementedException();
    }
}
