using System.Linq.Expressions;

namespace CleanValidationMapper.InputValidation.Property;

public interface IGenericMappings<T>
{
    OptionalReferenceBuilder<TProp> MapOptionalReference<TProp>(Expression<Func<T, TProp?>> propertyExpression) where TProp : notnull;

    void MapOptionalReference<TProp>(Expression<Func<T, TProp?>> propertyExpression, Action<OptionalReferenceBuilder<TProp>> builder) where TProp : notnull;

    OptionalStructBuilder<TProp> MapOptionalStruct<TProp>(Expression<Func<T, TProp?>> propertyExpression) where TProp : struct;

    void MapOptionalStruct<TProp>(Expression<Func<T, TProp?>> propertyExpression, Action<OptionalStructBuilder<TProp>> builder) where TProp : struct;
}
