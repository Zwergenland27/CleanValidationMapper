using CleanValidationMapper.Errors;
using CleanValidationMapper.InputValidation.ValidatedProperty;
using System.Linq.Expressions;

namespace CleanValidationMapper.InputValidation.Property;

public class OptionalPropertyBuilder<T> : PropertyBuilder<T>
{
    public OptionalPropertyBuilder(string name, bool shouldBeRequired = false) : base(name)
    {

    }

    public override CanFail<object?> Create()
    {
        throw new NotImplementedException();
    }
}

public class OptionalReferenceBuilder<T> : OptionalPropertyBuilder<T>, IMappingsForOptional<T> where T : notnull
{
    public OptionalReferenceBuilder(string name) : base(name)
    {

    }

    #region Required

    public OptionalReferenceBuilder<TProp> MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public void MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public OptionalStructBuilder<TProp> MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : struct
    {
        throw new NotImplementedException();
    }

    public void MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<OptionalStructBuilder<TProp>> builder) where TProp : struct
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

    public void FromParameter(T? parameter)
    {

    }

    public void ByCalling(Delegate creationMethod, Action<OptionalValidatedReferenceBuilder<T>> methodBuilder)
    {
        throw new NotImplementedException();
    }

    public override CanFail<object?> Create()
    {
        throw new NotImplementedException();
    }
}

public class OptionalStructBuilder<T> : OptionalPropertyBuilder<T>, IMappingsForOptional<T> where T : struct
{
    public OptionalStructBuilder(string name) : base(name)
    {

    }

    #region Required

    public OptionalReferenceBuilder<TProp> MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public void MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public OptionalStructBuilder<TProp> MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : struct
    {
        throw new NotImplementedException();
    }

    public void MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<OptionalStructBuilder<TProp>> builder) where TProp : struct
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

    public void FromParameter(T? parameter)
    {

    }

    public void ByCalling(Delegate creationMethod, Action<OptionalValidatedStructBuilder<T>> methodBuilder)
    {
        throw new NotImplementedException();
    }

    public override CanFail<object?> Create()
    {
        throw new NotImplementedException();
    }
}