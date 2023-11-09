// See https://aka.ms/new-console-template for more information
using CleanValidationMapper;
using CleanValidationMapper.Errors;
using CleanValidationMapper.RequestValidation;

var body = new Body("1", "2", "3", "4", null);
var res = body.Validate();
Console.WriteLine("Tst");
public class Body : AbstractBody<Test>
{
    private RequiredReferenceProperty<CustomParam> customParamBuilder;
    public Body(string? param, string? param2, string? param3, string? param4, string? param5)
    {
        Param = param;
        Param2 = param2;
        Param3 = param3;
        Param4 = param4;
        Param5 = param5;
    }

    public string? Param { get; }
    public string? Param2 { get; }
    public string? Param3 { get; }
    public string? Param4 { get; }
    public string? Param5 { get; }

    protected override void Configure(RequiredReferenceProperty<Test> propertyBuilder)
    {
        propertyBuilder.MapRequired(t => t.Param2, pb =>
        {
            pb.MapRequired(cp => cp.Titel)
               .FromParameter(Param3, Error.Validation("Titel missing", ""));
            pb.MapRequired(cp => cp.Value, cb =>
            {
                cb.MapRequired(Wurzel => Wurzel.Wert)
                   .FromParameter(Param2, Error.Validation("1 fehlt", ""));

                cb.MapRequired(Wurzel => Wurzel.Test)
                    .MapRequired(Primitiv => Primitiv.Test)
                    .FromParameter(Param4, Error.Validation("Primitiv fehlt", ""));
            });
        });

        propertyBuilder.MapRequired(t => t.Param)
            .FromParameter(Param, Error.Validation("Parameter missing", ""));

        propertyBuilder.MapOptional (t => t.Validated)
            .ByCalling(ValidatedParam.Create, methodParameters =>
            {
                methodParameters.AddRequired<string>().FromParameter(Param5, Error.Validation("Paramter 5 missing", ""));
            });
    }

    //public void Validated(string? parameter)
    //{
    //    _propertyBuilder.MapRequired(t => t.Validated, vb =>
    //    {
    //        vb.MapRequiredParameter<string>("var1")
    //            .FromParameter(parameter, Error.Validation("validated missing", ""));
    //        vb.ByCalling(ValidatedParam.Create);
    //    });

    //}
}

public record Primitiv(string Test);

public record Wurzel(string Wert, Primitiv Test); 

public record CustomParam(Wurzel Value, string Titel);

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
        CanFail<ValidatedParam> result = new();
        if (var1 == "fail") return Error.Validation("ValidatedParam.Fail", "");

        return result.SetValue(new ValidatedParam(var1));
    }
}

public record Test(string Param, CustomParam Param2, ValidatedParam? Validated);