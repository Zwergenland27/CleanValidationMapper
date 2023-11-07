using System.Linq.Expressions;

namespace CleanValidationMapper.RequestValidation;

public abstract class OptionalProperty<T> : Property<T>
{
    public OptionalProperty(string name) : base(name) { }
    public void FromParameter(T? value)
    {
        _directMapped = true;
        _mapped = value;
    }

    private bool IsRequiredProperty(Type propertyType)
    {
        return propertyType.Name == typeof(RequiredReferenceProperty<>).Name;
    }

    protected Dictionary<string, object?> CreateProperties(CanFail<object?> result, ref bool missingRequired)
    {
        Dictionary<string, object?> properties = new();

        bool missingRequiredLocal = false;

        foreach (var property in Properties)
        {
            var creationResult = property.Create();
            result.InheritFailure(creationResult);
            if (!creationResult.HasFailed) properties.Add(property.Name, creationResult.Value);

            if (creationResult.Value is null && IsRequiredProperty(property.GetType()))
            {
                missingRequiredLocal = true;
            }
        }

        missingRequired = missingRequiredLocal;
        return properties;
    }
}

public class OptionalReferenceProperty<T> : OptionalProperty<T>
{
    public OptionalReferenceProperty(string name) : base(name) { }

    public RequiredReferenceProperty<TProp> MapRequired<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : notnull
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new RequiredReferenceProperty<TProp>(memberExpression.Member.Name, true);
        AddProperty(property);
        return property;
    }

    public RequiredReferenceProperty<TProp> MapRequired<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<RequiredReferenceProperty<TProp>> propertyBuilder) where TProp : notnull
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new RequiredReferenceProperty<TProp>(memberExpression.Member.Name, true);
        propertyBuilder.Invoke(property);
        AddProperty(property);
        return property;
    }

    public OptionalReferenceProperty<TProp> MapOptional<TProp>(Expression<Func<T, TProp>> propertyExpression)
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new OptionalReferenceProperty<TProp>(memberExpression.Member.Name);
        AddProperty(property);
        return property;
    }

    public OptionalReferenceProperty<TProp> MapOptional<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<OptionalReferenceProperty<TProp>> propertyBuilder)
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new OptionalReferenceProperty<TProp>(memberExpression.Member.Name);
        propertyBuilder.Invoke(property);
        AddProperty(property);
        return property;
    }

    public override CanFail<object?> Create()
    {
        if (_directMapped)
        {
            return _mapped;
        }

        CanFail<object?> result = new();
        bool missingRequired = false;

        var properties = CreateProperties(result, ref missingRequired);
        if (result.HasFailed) return result;

        if (missingRequired) return default(T);

        return CreateInstance(properties);
    }
}
