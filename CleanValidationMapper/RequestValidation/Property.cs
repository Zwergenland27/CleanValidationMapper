using System.Reflection;

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

public abstract class Property<T> : Property
{
    protected readonly List<Property> _properties = new();

    protected T? _mapped;
    protected bool _directMapped = false;

    private readonly ConstructorInfo _constructor;
    private bool _multipleConstructors = false;

    public Property(string name) : base(name)
    {
        var constructors = typeof(T).GetConstructors();
        if (constructors.Count() == 0) throw new InvalidOperationException($"{typeof(T)} must have one constructor");
        if (constructors.Count() > 1) _multipleConstructors = true;
        _constructor = constructors[0];
    }

    protected Dictionary<string, object?> CreateProperties(CanFail<object?> result)
    {
        Dictionary<string, object?> properties = new();

        _properties.ForEach(property =>
        {
            var creationResult = property.Create();
            result.InheritFailure(creationResult);
            if (!creationResult.HasFailed) properties.Add(property.Name, creationResult.Value);
        });

        return properties;
    }

    protected T CreateInstance(Dictionary<string, object?> properties)
    {
        if (_multipleConstructors) throw new InvalidOperationException($"{typeof(T)} can only have one constructor");
        var constructorParameters = _constructor.GetParameters();

        object?[] parameters = new object?[constructorParameters.Length];

        foreach (var parameter in constructorParameters)
        {
            if (parameter.Name is null) continue;
            parameters[parameter.Position] = properties.GetValueOrDefault(parameter.Name);
        }

        return (T)Activator.CreateInstance(typeof(T), parameters)!;
    }
}
