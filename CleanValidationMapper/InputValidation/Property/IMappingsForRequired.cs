using CleanValidationMapper.InputValidation.ValidatedProperty;
using System.Linq.Expressions;

namespace CleanValidationMapper.InputValidation.Property;

public interface IMappingsForRequired<T> : IGenericMappings<T>
{
    RequiredReferenceBuilder<TProp> MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : notnull;

    void MapRequiredReference<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<RequiredReferenceBuilder<TProp>> builder) where TProp : notnull;

    RequiredStructBuilder<TProp> MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : struct;

    void MapRequiredStruct<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<RequiredStructBuilder<TProp>> builder) where TProp : struct;
}
