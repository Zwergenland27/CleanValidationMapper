﻿using System.Linq.Expressions;
using System.Reflection.Emit;

namespace CleanValidationMapper;

public class AbstractBody<TValidated>
{
	List<Property> _properties = new();

	protected RequiredProperty<T> RequiredProperty<T>(Expression<Func<TValidated, T>> propertyExpression) where T : notnull
	{
		MemberExpression memberExpression = (MemberExpression) propertyExpression.Body;
		var property = new RequiredProperty<T>(memberExpression.Member.Name);
		_properties.Add(property);
		return property;
	}

    protected RequiredProperty<T> RequiredProperty<T>(Expression<Func<TValidated, T>> propertyExpression, Action<RequiredProperty<T>> propertyBuilder) where T : notnull
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new RequiredProperty<T>(memberExpression.Member.Name);
        propertyBuilder.Invoke(property);
        _properties.Add(property);
        return property;
    }

    protected OptionalProperty<T> OptionalProperty<T>(Expression<Func<TValidated, T?>> propertyExpression) where T : notnull
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new OptionalProperty<T>(memberExpression.Member.Name);
        _properties.Add(property);
        return property;
    }

    protected OptionalProperty<T> OptionalProperty<T>(Expression<Func<TValidated, T?>> propertyExpression, Action<OptionalProperty<T>> propertyBuilder) where T : notnull
    {
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        var property = new OptionalProperty<T>(memberExpression.Member.Name);
        propertyBuilder.Invoke(property);
        _properties.Add(property);
        return property;
    }

    public CanFail<TValidated> Validate()
	{
		CanFail<TValidated> result = new();
		Dictionary<string, object?> properties = new();

		_properties.ForEach(property =>
		{
			var creationResult = property.Create();
			result.InheritFailure(creationResult);
			if (!creationResult.HasFailed)
			{
				properties.Add(property.Name, creationResult.Value);
			}
		});

        if (result.HasFailed) return result;

        var constructors = typeof(TValidated).GetConstructors();
        if (constructors.Count() > 1) throw new InvalidOperationException($"{typeof(TValidated)} can only have one constructor");

        var constructorParameters = constructors[0].GetParameters();

        object?[] parameters = new object?[constructorParameters.Length];

        foreach (var parameter in constructorParameters)
        {
            if (parameter.Name is null) continue;
            parameters[parameter.Position] = properties.GetValueOrDefault(parameter.Name);
        }

        return (TValidated)Activator.CreateInstance(typeof(TValidated), parameters)!;
    }
}
