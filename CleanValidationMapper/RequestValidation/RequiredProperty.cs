using System.Linq.Expressions;

namespace CleanValidationMapper;

public abstract class RequiredProperty<T> : Property<T>
{
    protected Error? _missingError;
    public RequiredProperty(string name) : base(name) { }

    public void FromParameter(T? value, Error missingError)
    {
        _directMapped = true;
        _mapped = value;
        _missingError = missingError;
    }
}

public class RequiredReferenceProperty<T> : RequiredProperty<T> where T : notnull
{
    public RequiredReferenceProperty(string name) : base(name) { }

    public RequiredReferenceProperty<TProp> MapRequired<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : notnull
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new RequiredReferenceProperty<TProp>(memberExpression.Member.Name);
        _properties.Add(property);
        return property;
    }

    public RequiredReferenceProperty<TProp> MapRequired<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<RequiredReferenceProperty<TProp>> propertyBuilder) where TProp : notnull
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new RequiredReferenceProperty<TProp>(memberExpression.Member.Name);
        propertyBuilder.Invoke(property);
        _properties.Add(property);
        return property;
    }

    public OptionalReferenceProperty<TProp> MapOptional<TProp>(Expression<Func<T, TProp>> propertyExpression)
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new OptionalReferenceProperty<TProp>(memberExpression.Member.Name);
        _properties.Add(property);
        return property;
    }

    public OptionalReferenceProperty<TProp> MapOptional<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<OptionalReferenceProperty<TProp>> propertyBuilder)
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new OptionalReferenceProperty<TProp>(memberExpression.Member.Name);
        propertyBuilder.Invoke(property);
        _properties.Add(property);
        return property;
    }

    public override CanFail<object?> Create()
    {
        if (_directMapped)
        {
            if (_mapped is null) return _missingError!;
            return _mapped;
        }

        CanFail<object?> result = new();
        var properties = CreateProperties(result);

        if (result.HasFailed) return result;

        return CreateInstance(properties);
    }
}
