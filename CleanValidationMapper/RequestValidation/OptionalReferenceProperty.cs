using System.Linq.Expressions;

namespace CleanValidationMapper;

public abstract class OptionalProperty<T> : Property<T>
{
    public OptionalProperty(string name) : base(name) { }
    public void FromParameter(T? value)
    {
        _directMapped = true;
        _mapped = value;
    }
}

public class OptionalReferenceProperty<T> : OptionalProperty<T>
{
    public OptionalReferenceProperty(string name) : base(name) { }

    public OptionalReferenceProperty<TProp> Map<TProp>(Expression<Func<T, TProp>> propertyExpression)
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new OptionalReferenceProperty<TProp>(memberExpression.Member.Name);
        _properties.Add(property);
        return property;
    }

    public OptionalReferenceProperty<TProp> Map<TProp>(Expression<Func<T, TProp>> propertyExpression, Action<OptionalReferenceProperty<TProp>> propertyBuilder)
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
            return _mapped;
        }

        CanFail<object?> result = new();
        var properties = CreateProperties(result);

        if (result.HasFailed) return result;

        if (!properties.Any(p => p.Value is not null)) return default(T);

        return CreateInstance(properties);
    }
}
