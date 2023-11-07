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
    private readonly List<Property> _properties = new();

    protected IReadOnlyList<Property> Properties => _properties.AsReadOnly();

    protected T? _mapped;
    protected bool _directMapped = false;

    private readonly ConstructorInfo _constructor;
    private readonly bool _multipleConstructors = false;

    public Property(string name) : base(name)
    {
        var constructors = typeof(T).GetConstructors();
        if (constructors.Count() == 0) throw new InvalidOperationException($"{typeof(T)} must have one constructor");
        if (constructors.Count() > 1) _multipleConstructors = true;
        _constructor = constructors[0];
    }

    protected void AddProperty(Property property)
    {
        if (_properties.Any(p => p.Name == property.Name)) throw new InvalidOperationException($"Property {typeof(T)}.{property.Name} could not be added because it already exists");
        _properties.Add(property);
    }

    protected void CheckConstructor()
    {
        if (_multipleConstructors) throw new InvalidOperationException($"{typeof(T)} can only have one constructor");

        if(_constructor.GetParameters().Count() > _properties.Count())
        {
            string missingProperties = "";

            foreach(var parameter in _constructor.GetParameters())
            {
                if (!_properties.Any(p => p.Name == parameter.Name)) missingProperties += $"{Environment.NewLine}{typeof(T)}.{parameter.Name}";
            }

            throw new InvalidOperationException($"Property of type {typeof(T)} can not be created because you are missing following properties: {missingProperties}");
        }
    }

    protected T CreateInstance(Dictionary<string, object?> properties)
    {
        CheckConstructor();

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
