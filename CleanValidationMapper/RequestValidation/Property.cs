using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace CleanValidationMapper;

public abstract class Property
{
    public abstract CanFail<object?> Create();

    public string Name { get; private set; }
    public Property(string name)
    {
        Name = name;
    }
}
public class RequiredProperty<T> : Property where T : notnull
{
    private readonly List<Property> _properties = new();

    private T? _mapped;
    private bool _directMapped = false;
    private Error? _missingError;

    public RequiredProperty(string name) : base(name)
    {
    }

    public void FromParameter(T? value, Error missingError)
    {
        _directMapped = true;
        _mapped = value;
        _missingError = missingError;
    }

    public RequiredProperty<TProp> MapRequired<TProp>(Expression<Func<T, TProp>> propertyExpression) where TProp : notnull
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new RequiredProperty<TProp>(memberExpression.Member.Name);
        _properties.Add(property);
        return property;
    }

    public OptionalProperty<TProp> MapOptional<TProp>(Expression<Func<T, TProp>> propertyExpression)
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new OptionalProperty<TProp>(memberExpression.Member.Name);
        _properties.Add(property);
        return property;
    }

    public OptionalProperty<TProp> MapOptional<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<OptionalProperty<TProp>> propertyBuilder)
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new OptionalProperty<TProp>(memberExpression.Member.Name);
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
        Dictionary<string, object?> properties = new();

        _properties.ForEach(property =>
        {
            var creationResult = property.Create();
            result.InheritFailure(creationResult);
            if (!creationResult.HasFailed) properties.Add(property.Name, creationResult.Value);
        });

        if (result.HasFailed) return result;

        //muss das hier hin - war erst nicht da ?! if (!properties.Any(p => p.Value is not null)) return default(T);

        var constructors = typeof(T).GetConstructors();
        if (constructors.Count() > 1) throw new InvalidOperationException($"{typeof(T)} can only have one constructor");

        var constructorParameters = constructors[0].GetParameters();

        object?[] parameters = new object?[constructorParameters.Length];

        foreach (var parameter in constructorParameters)
        {
            if (parameter.Name is null) continue;
            parameters[parameter.Position] = properties.GetValueOrDefault(parameter.Name);
        }

        return (T)Activator.CreateInstance(typeof(T), parameters)!;
    }
}

public class OptionalProperty<T> : Property
{
    private readonly List<Property> _properties = new();

    private T? _mapped;
    private bool _directMapped = false;

    public OptionalProperty(string name) : base(name)
    {
    }

    public void FromParameter(T? value)
    {
        _directMapped = true;
        _mapped = value;
    }
    public OptionalProperty<TProp> Map<TProp>(Expression<Func<T, TProp>> propertyExpression)
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new OptionalProperty<TProp>(memberExpression.Member.Name);
        _properties.Add(property);
        return property;
    }
    public override CanFail<object?> Create()
    {
        if (_directMapped)
        {
            return _mapped;
        }

        CanFail<object?> result = new();
        Dictionary<string, object?> properties = new();

        _properties.ForEach(property =>
        {
            var creationResult = property.Create();
            result.InheritFailure(creationResult);
            if (!creationResult.HasFailed) properties.Add(property.Name, creationResult.Value);
        });

        if (result.HasFailed) return result;

        if (!properties.Any(p => p.Value is not null)) return default(T);

        var constructors = typeof(T).GetConstructors();
        if (constructors.Count() > 1) throw new InvalidOperationException($"{typeof(T)} can only have one constructor");

        var constructorParameters = constructors[0].GetParameters();

        object?[] parameters = new object?[constructorParameters.Length];

        foreach (var parameter in constructorParameters)
        {
            if (parameter.Name is null) continue;
            parameters[parameter.Position] = properties.GetValueOrDefault(parameter.Name);
        }

        return (T)Activator.CreateInstance(typeof(T), parameters)!;
    }
}
