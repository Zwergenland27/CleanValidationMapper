using CleanValidationMapper.InputValidation.Property;

namespace CleanValidationMapper.InputValidation.ValidatedProperty;

public interface IMappingsForOptional<T> : IGenericMappings<T>
{
    OptionalReferenceBuilder<TProp> AddRequiredReference<TProp>() where TProp : notnull;

    void AddRequiredReference<TProp>(Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull;

    OptionalStructBuilder<TProp> AddRequiredStruct<TProp>() where TProp : struct;

    void AddRequiredStruct<TProp>(Action<OptionalStructBuilder<TProp>> builder) where TProp : struct;
}
