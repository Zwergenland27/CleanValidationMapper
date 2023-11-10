using System.Reflection;
using CleanValidationMapper.InputValidation.ValidatedProperty;

namespace CleanValidationMapper.InputValidation.Property;
public abstract class PropertyBuilder
{
    public string Name { get; private set; }

    public PropertyBuilder(string name)
    {
        Name = name;
    }

    public abstract CanFail<object?> Create();
}

public abstract class PropertyBuilder<T> : PropertyBuilder
{
    private List<PropertyBuilder> _properties = new List<PropertyBuilder>();

    protected IReadOnlyCollection<PropertyBuilder> Properties => _properties.AsReadOnly();

    protected T? _mapped;

    protected bool _directMapped = false;

    protected ConstructorInfo? _constructor;

    protected ValidatedPropertyBuilder<T>? _creationMethod;

    public PropertyBuilder(string name) : base(name)
    {

    }

    protected void AddProperty(PropertyBuilder property)
    {

    }

    private void CheckConstructor()
    {

    }

    protected CanFail<T> CreateInstance(Dictionary<string, object?> properties)
    {
        throw new NotImplementedException();
    }
}
