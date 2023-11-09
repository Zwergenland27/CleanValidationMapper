using System.Reflection;

namespace CleanValidationMapper.RequestValidation;

public enum InitMethod
{
    Constructor,
    CreateMethod
}
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

    private readonly ConstructorInfo? _constructor;
    private readonly bool _multipleConstructors = false;

    protected CreationMethod<T>? _creationMethod;

    public Property(string name) : base(name)
    {
        var constructors = typeof(T).GetConstructors();
        if (constructors.Count() == 0)
        {
            return;
        }
        if (constructors.Count() > 1) _multipleConstructors = true;
        _constructor = constructors[0];
    }

    protected void AddProperty(Property property)
    {
        if (_properties.Any(p => p.Name == property.Name)) throw new InvalidOperationException($"Property {typeof(T)}.{property.Name} could not be added because it already exists");
        _properties.Add(property);
    }

    private InitMethod GetInitMethod()
    {
        if (_constructor == null)
        {
            CheckCreationMethod();
            return InitMethod.CreateMethod;
        }

        CheckConstructor();
        return InitMethod.Constructor;
    }

    private void CheckCreationMethod()
    {
        if (_creationMethod == null) throw new InvalidOperationException($"${typeof(T)} must have one public constructor OR a static create method that returns CanFail<${typeof(T)}>");

        if (_creationMethod.Method.ReturnType != typeof(CanFail<T>)) throw new InvalidOperationException($"Creation method must return CanFail<{typeof(T)}>");
        if (_creationMethod.Method.GetParameters().Count() > _properties.Count())
        {
            string missingParameters = "";
            foreach (var parameter in _creationMethod.Method.GetParameters())
            {
                if (!_properties.Any(p => p.Name == parameter.Name)) missingParameters += $"{Environment.NewLine}{typeof(T)}.{parameter.Name}";
            }

            throw new InvalidOperationException($"Property of type {typeof(T)} can not be created because you are missing following parameters for the creation method {typeof(T)}.{_creationMethod.Method.Name}: {missingParameters}");
        }
    }

    private void CheckConstructor()
    {
        if (_multipleConstructors) throw new InvalidOperationException($"{typeof(T)} can only have one constructor");

        if (_constructor!.GetParameters().Count() > _properties.Count())
        {
            string missingProperties = "";

            foreach (var parameter in _constructor.GetParameters())
            {
                if (!_properties.Any(p => p.Name == parameter.Name)) missingProperties += $"{Environment.NewLine}{typeof(T)}.{parameter.Name}";
            }

            throw new InvalidOperationException($"Property of type {typeof(T)} can not be created because you are missing following properties in the constructor: {missingProperties}");
        }
    }

    protected CanFail<T> CreateInstance(Dictionary<string, object?> properties)
    {
        InitMethod initMethod =  GetInitMethod();

        if(initMethod == InitMethod.Constructor)
        {
            var constructorParameters = _constructor!.GetParameters();

            object?[] parameters = new object?[constructorParameters.Length];

            foreach (var parameter in constructorParameters)
            {
                if (parameter.Name is null) continue;
                parameters[parameter.Position] = properties.GetValueOrDefault(parameter.Name);
            } 

            return new CanFail<T>().SetValue((T)_constructor.Invoke(parameters)!);
        }
        else if(initMethod == InitMethod.CreateMethod)
        {
            var methodParameters = _creationMethod!.Method.GetParameters();

            object?[] parameters = new object[methodParameters.Length];
            foreach (var parameter in methodParameters)
            {
                if (parameter.Name is null) continue;
                parameters[parameter.Position] = properties.GetValueOrDefault(parameter.Name);
            }

            return (CanFail<T>)_creationMethod.DynamicInvoke(parameters)!; ;
        }

        throw new NotImplementedException("If you get this exception something went horribly wrong.");

    }
}
