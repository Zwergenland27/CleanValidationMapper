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

    protected (CanFail<Dictionary<string, object?>> result, bool missingRequired) CreateProperties()
    {
        CanFail<Dictionary<string, object?>> createAllPropertiesResult = new();
        Dictionary<string, object?> properties = new();

        bool missingRequired = false;

        foreach (var property in Properties)
        {
            var creationResult = property.Create();
            createAllPropertiesResult.InheritFailure(creationResult);
            if (!creationResult.HasFailed)
            {
                properties.Add(property.Name, creationResult.Value);

                if (creationResult.Value is null && IsRequiredProperty(property.GetType()))
                {
                    missingRequired = true;
                }
            }
        }

        createAllPropertiesResult.SetValue(properties);
        return (createAllPropertiesResult, missingRequired);
    }
}

public class OptionalReferenceProperty<T> : OptionalProperty<T>
{
    public OptionalReferenceProperty(string name) : base(name) { }

    public void ByCalling(Delegate creationMethod, Action<CreationMethod<T>> methodParameters)
    {
        _creationMethod = new CreationMethod<T>(creationMethod, true);
        methodParameters.Invoke(_creationMethod);
    }

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
        CanFail<object?> result = new();
        if (_directMapped)
        {
            return result.SetValue(_mapped);
        }

        var propertiesCreationResult = CreateProperties();
        result.InheritFailure(propertiesCreationResult.result);

        if (result.HasFailed) return result;
        if (propertiesCreationResult.missingRequired) return result.SetValue(default(T));

        var creationResult = CreateInstance(propertiesCreationResult.result.Value);
        result.InheritFailure(creationResult);
        if (result.HasFailed) return result;

        return result.SetValue(creationResult.Value);
    }
}
