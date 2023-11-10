using CleanValidationMapper.Errors;
using System.Linq.Expressions;
using System.Reflection;

namespace CleanValidationMapper.RequestValidationTests;

public class CreationMethod<T>
{
    private readonly Delegate _creationMethod;

    private readonly List<Property> _parameters = new();
    private int _currentParameterIndex = 0;

    private readonly bool _missingInOptionalProperty;
    public CreationMethod(Delegate creationMethod, bool missingInOptionalProperty = false)
    {
        if (creationMethod.Method.ReturnType != typeof(CanFail<T>)) throw new InvalidOperationException($"Creation method must return CanFail<{typeof(T)}>");
        _creationMethod = creationMethod;
        _missingInOptionalProperty = missingInOptionalProperty;
    }

    private static bool IsRequiredProperty(Type propertyType)
    {
        return propertyType.Name == typeof(RequiredReference<>).Name;
    }

    private string NextParameterName()
    {
        var methodParameters = _creationMethod.Method.GetParameters();
        if (_currentParameterIndex >= methodParameters.Length) throw new InvalidOperationException($"{_creationMethod.Method.Name} only takes {methodParameters.Length} parameters");
        _currentParameterIndex++;
        return methodParameters[_currentParameterIndex - 1].Name!;
    }

    private void CheckParameters()
    {
        if (_creationMethod.Method.GetParameters().Count() > _parameters.Count())
        {
            string missingParameters = "";
            foreach (var parameter in _creationMethod.Method.GetParameters())
            {
                if (!_parameters.Any(p => p.Name == parameter.Name)) missingParameters += $"{Environment.NewLine}{typeof(T)}.{parameter.Name}";
            }

            throw new InvalidOperationException($"Property of type {typeof(T)} can not be created because you are missing following parameters for the creation method {typeof(T)}.{_creationMethod.Method.Name}: {missingParameters}");
        }
    }

    private (CanFail<Dictionary<string, object?>> result, bool missingRequired) CreateParameters()
    {
        CanFail<Dictionary<string, object?>> createAllParametersResult = new();
        Dictionary<string, object?> parameters = new();

        bool missingRequired = false;

        foreach (var parameter in _parameters)
        {
            var creationResult = parameter.Create();
            createAllParametersResult.InheritFailure(creationResult);
            if (!creationResult.HasFailed)
            {
                parameters.Add(parameter.Name, creationResult.Value);

                if (creationResult.Value is null && _missingInOptionalProperty && IsRequiredProperty(parameter.GetType()))
                {
                    missingRequired = true;
                }
            }
        }

        createAllParametersResult.SetValue(parameters);
        return (createAllParametersResult, missingRequired);
    }

    private CanFail<T> ExecuteCreate(Dictionary<string, object?> parameters)
    {
        var methodParameters = _creationMethod.Method.GetParameters();

        object?[] arguments = new object[methodParameters.Length];
        foreach (var parameter in methodParameters)
        {
            if (parameter.Name is null) continue;
            arguments[parameter.Position] = parameters.GetValueOrDefault(parameter.Name);
        }
        //TODO: prüfen, ob ein required Parameter null -> falls missingInOptionalProperty true -> null zurückgeben
        var test = (CanFail<T>)_creationMethod.DynamicInvoke(arguments)!;
        return test;
    }

    public CanFail<T> Execute()
    {
        CanFail<T> result = new();
        CheckParameters();

        var propertiesCreationResult = CreateParameters();
        result.InheritFailure(propertiesCreationResult.result);

        if (result.HasFailed) return result;
        if (propertiesCreationResult.missingRequired) return result.SetValue(default(T)!);

        return ExecuteCreate(propertiesCreationResult.result.Value);
    }
    public RequiredReference<TProp> AddRequired<TProp>() where TProp : notnull
    {
        var property = new RequiredReference<TProp>(NextParameterName(), _missingInOptionalProperty);
        _parameters.Add(property);
        return property;
    }
    //TODO: Wenn in einem Optionalen Parameter => keine Missing Fehlermeldung einbauen, kann ja eh nicht geworfen werden!

    public RequiredReference<TProp> AddRequired<TProp>(Action<RequiredReference<TProp>> propertyBuilder) where TProp : notnull
    {
        var property = new RequiredReference<TProp>(NextParameterName(), _missingInOptionalProperty);
        propertyBuilder.Invoke(property);
        _parameters.Add(property);
        return property;
    }

    public OptionalReferenceProperty<TProp> AddOptional<TProp>()
    {
        var property = new OptionalReferenceProperty<TProp>(NextParameterName());
        _parameters.Add(property);
        return property;
    }

    public OptionalReferenceProperty<TProp> AddOptional<TProp>(Action<OptionalReferenceProperty<TProp>> propertyBuilder)
    {
        var property = new OptionalReferenceProperty<TProp>(NextParameterName());
        propertyBuilder.Invoke(property);
        _parameters.Add(property);
        return property;
    }
}
