// See https://aka.ms/new-console-template for more information
using CleanValidationMapper;
using CleanValidationMapper.Errors;
using CleanValidationMapper.RequestValidation;

var body = new Body("1", "3");
body.WithWurzel(null, "Test");
body.Validated("Test");
var res = body.Validate();
Console.WriteLine("Tst");
public class Body : AbstractBody<Test>
{
    private RequiredReferenceProperty<CustomParam> customParamBuilder;
    public Body(string? param, string? param3)
    {
        _propertyBuilder.MapRequired(t => t.Param2, pb =>
            {
                pb.MapRequired(cp => cp.Titel)
                   .FromParameter(param3, Error.Validation("Titel missing", ""));
                customParamBuilder = pb;
            });

        _propertyBuilder.MapRequired(t => t.Param)
            .FromParameter(param, Error.Validation("Parameter missing", ""));
    }

    public void WithWurzel(string? param2, string? param4)
    {
        customParamBuilder.MapOptional(cp => cp.Value, cb =>
        {
            cb.MapRequired(Wurzel => Wurzel.Wert)
               .FromParameter(param2, Error.Validation("1 fehlt", ""));

            cb.MapOptional(Wurzel => Wurzel.Test)
                .MapRequired(Primitiv => Primitiv.Test)
                .FromParameter(param4, Error.Validation("Primitiv fehlt", ""));
        });
    }

    public void Validated(string? parameter)
    {
        _propertyBuilder.MapRequired(t => t.Validated, vb =>
        {
            vb.MapRequiredParameter<string>("var1")
                .FromParameter(parameter, Error.Validation("validated missing", ""));
            vb.ByCalling(ValidatedParam.Create);
        });

    }
}

public record Primitiv(string Test);

public record Wurzel(string Wert, Primitiv? Test); 

public record CustomParam(Wurzel? Value, string Titel);

public record ValidatedParam
{
    private ValidatedParam(string value)
    {
        Value = value;
    }

    public string Value { get; set; }

    public static void Furz()
    {

    }
    public static CanFail<ValidatedParam> Create(string var1)
    {
        if (var1 == "fail") return Error.Validation("ValidatedParam.Fail", "");

        return new ValidatedParam(var1);
    }
}

public record Test(string Param, CustomParam Param2, ValidatedParam Validated);