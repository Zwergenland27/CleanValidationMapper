using CleanValidationMapper.Errors;
using System.Linq.Expressions;

namespace CleanValidationMapper.RequestValidation;

public abstract class RequiredProperty<T> : Property<T>
{
    protected Error? _missingError;

    protected readonly bool _missingInOptionalProperty;
    public RequiredProperty(string name, bool missingInOptionalProperty = false) : base(name)
    {
        _missingInOptionalProperty = missingInOptionalProperty;
    }

    public void FromParameter(T? value, Error missingError)
    {
        _directMapped = true;
        _mapped = value;
        _missingError = missingError;
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

                if (creationResult.Value is null && _missingInOptionalProperty && IsRequiredProperty(property.GetType()))
                {
                    missingRequired = true;
                }
            }
        }

        createAllPropertiesResult.SetValue(properties);
        return (createAllPropertiesResult, missingRequired);
    }
}

public class RequiredReferenceProperty<T> : RequiredProperty<T> where T : notnull
{
    public RequiredReferenceProperty(string name, bool missingInOptionalProperty = false) : base(name, missingInOptionalProperty) { }

    public RequiredReferenceProperty<TProp> MapRequiredParameter<TProp>(string parameterName) where TProp : notnull
    {
        var property = new RequiredReferenceProperty<TProp>(parameterName, _missingInOptionalProperty);
        AddProperty(property);
        return property;
    }

    public void ByCalling(Delegate creationMethod, Action<CreationMethod<T>> methodParameters)
    {
        _creationMethod = new CreationMethod<T>(creationMethod);
        methodParameters.Invoke(_creationMethod);
    }

    public RequiredReferenceProperty<TProp> MapRequired<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : notnull
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new RequiredReferenceProperty<TProp>(memberExpression.Member.Name, _missingInOptionalProperty);
        AddProperty(property);
        return property;
    }
    //TODO: Wenn in einem Optionalen Parameter => keine Missing Fehlermeldung einbauen, kann ja eh nicht geworfen werden!

    public RequiredReferenceProperty<TProp> MapRequired<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<RequiredReferenceProperty<TProp>> propertyBuilder) where TProp : notnull
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new RequiredReferenceProperty<TProp>(memberExpression.Member.Name, _missingInOptionalProperty);
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
            if (_mapped is null)
            {
                if (_missingInOptionalProperty == false) return _missingError!;
                return result.SetValue((object?)null);
            }
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
