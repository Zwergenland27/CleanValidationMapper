using CleanValidationMapper.InputValidation.Property;
using System.Linq.Expressions;

namespace CleanValidationMapper.InputValidation.ValidatedProperty;

public interface IMappingsForRequired<T> : IGenericMappings<T>
{
    RequiredReferenceBuilder<TProp> AddRequiredReference<TProp>() where TProp : notnull;

    void AddRequiredReference<TProp>(Action<RequiredReferenceBuilder<TProp>> builder) where TProp : notnull;

    RequiredStructBuilder<TProp> AddRequiredStruct<TProp>() where TProp : struct;

    void AddRequiredStruct<TProp>(Action<RequiredStructBuilder<TProp>> builder) where TProp : struct;
}
