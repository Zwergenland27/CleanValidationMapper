using CleanValidationMapper.InputValidation.ValidatedProperty;
using System.Linq.Expressions;

namespace CleanValidationMapper.InputValidation.Property;

public interface IMappingsForOptional<T> : IGenericMappings<T>
{
    OptionalReferenceBuilder<TProp> MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : notnull;

    void MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull;

    OptionalStructBuilder<TProp> MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : struct;

    void MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<OptionalStructBuilder<TProp>> builder) where TProp : struct;
}
