using CleanValidationMapper.InputValidation.Property;

namespace CleanValidationMapper.InputValidation.ValidatedProperty;

public abstract class OptionalValidatedPropertyBuilder<T> : ValidatedPropertyBuilder<T>
{
}

public class OptionalValidatedReferenceBuilder<T> : OptionalValidatedPropertyBuilder<T>, IMappingsForOptional<T> where T : notnull
{
    #region Required

    public OptionalReferenceBuilder<TProp> AddRequiredReference<TProp>() where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public void AddRequiredReference<TProp>(Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public OptionalStructBuilder<TProp> AddRequiredStruct<TProp>() where TProp : struct
    {
        throw new NotImplementedException();
    }

    public void AddRequiredStruct<TProp>(Action<OptionalStructBuilder<TProp>> builder) where TProp : struct
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Optional

    public OptionalReferenceBuilder<TProp> AddOptionalReference<TProp>() where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public void AddOptionalReference<TProp>(Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public OptionalStructBuilder<TProp> AddOptionalStruct<TProp>() where TProp : struct
    {
        throw new NotImplementedException();
    }

    public void AddOptionalStruct<TProp>(Action<OptionalStructBuilder<TProp>> builder) where TProp : struct
    {
        throw new NotImplementedException();
    }

    #endregion

    public override CanFail<T> Execute()
    {
        throw new NotImplementedException();
    }

}


public class OptionalValidatedStructBuilder<T> : OptionalValidatedPropertyBuilder<T>, IMappingsForOptional<T> where T : struct
{
    #region Required

    public OptionalReferenceBuilder<TProp> AddRequiredReference<TProp>() where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public void AddRequiredReference<TProp>(Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public OptionalStructBuilder<TProp> AddRequiredStruct<TProp>() where TProp : struct
    {
        throw new NotImplementedException();
    }

    public void AddRequiredStruct<TProp>(Action<OptionalStructBuilder<TProp>> builder) where TProp : struct
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Optional

    public OptionalReferenceBuilder<TProp> AddOptionalReference<TProp>() where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public void AddOptionalReference<TProp>(Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull
    {
        throw new NotImplementedException();
    }

    public OptionalStructBuilder<TProp> AddOptionalStruct<TProp>() where TProp : struct
    {
        throw new NotImplementedException();
    }

    public void AddOptionalStruct<TProp>(Action<OptionalStructBuilder<TProp>> builder) where TProp : struct
    {
        throw new NotImplementedException();
    }

    #endregion

    public override CanFail<T> Execute()
    {
        throw new NotImplementedException();
    }
}

