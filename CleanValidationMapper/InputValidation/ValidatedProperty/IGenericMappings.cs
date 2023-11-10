using CleanValidationMapper.InputValidation.Property;
using System.Linq.Expressions;

namespace CleanValidationMapper.InputValidation.ValidatedProperty;

public interface IGenericMappings<T>
{
    OptionalReferenceBuilder<TProp> AddOptionalReference<TProp>() where TProp : notnull;

    void AddOptionalReference<TProp>(Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull;

    OptionalStructBuilder<TProp> AddOptionalStruct<TProp>() where TProp : struct;

    void AddOptionalStruct<TProp>(Action<OptionalStructBuilder<TProp>> builder) where TProp : struct;
}
