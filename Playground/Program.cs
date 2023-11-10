using CleanValidationMapper;
using CleanValidationMapper.Errors;
using CleanValidationMapper.InputValidation;
using CleanValidationMapper.InputValidation.Property;

#region Direct
class DirectReferenceTests : ReturnsReference<DirectCommand>
{
    public DirectReferenceTests(string? directRequiredReference, string? directOptionalReference, int? directRequiredStruct, int? directOptionalStruct)
    {
        DirectRequiredReference = directRequiredReference;
        DirectOptionalReference = directOptionalReference;
        DirectRequiredStruct = directRequiredStruct;
        DirectOptionalStruct = directOptionalStruct;
    }

    public string? DirectRequiredReference { get; }
    public string? DirectOptionalReference { get; }
    public int? DirectRequiredStruct { get; }
    public int? DirectOptionalStruct { get; }

    protected override void Configure(RequiredReferenceBuilder<DirectCommand> propertyBuilder)
    {
        propertyBuilder.MapRequiredReference(c => c.DirectRequiredReference)
             .FromParameter(DirectRequiredReference, Error.Validation("DirectRequiredReference missing", ""));
        propertyBuilder.MapOptionalReference(c => c.DirectRequiredReference)
            .FromParameter(DirectOptionalReference);

        propertyBuilder.MapRequiredStruct(c => c.DirectRequiredStruct)
            .FromParameter(DirectRequiredStruct, Error.Validation("DirectRequiredStruct missing", ""));
        propertyBuilder.MapOptionalStruct(c => c.DirectOptionalStruct)
            .FromParameter(DirectOptionalStruct);
    }
}

public record DirectCommand(string DirectRequiredReference, string? DirectOptionalReference, int DirectRequiredStruct, int? DirectOptionalStruct);

#endregion

#region Nested
class NestedReferenceTests : ReturnsReference<NestedCommand>
{
    public NestedReferenceTests(
        string? requiredAReference, string? optionalAReference,
        int? requiredAStruct, int? optionalAStruct,
        string? requiredBReference, string? optionalBReference,
        int? requiredBStruct, int? optionalBStruct)
    {
        RequiredAReference = requiredAReference;
        OptionalAReference = optionalAReference;
        RequiredAStruct = requiredAStruct;
        OptionalAStruct = optionalAStruct;
        RequiredBReference = requiredBReference;
        OptionalBReference = optionalBReference;
        RequiredBStruct = requiredBStruct;
        OptionalBStruct = optionalBStruct;
    }

    public string? RequiredAReference { get; }
    public string? OptionalAReference { get; }
    public int? RequiredAStruct { get; }
    public int? OptionalAStruct { get; }
    public string? RequiredBReference { get; }
    public string? OptionalBReference { get; }
    public int? RequiredBStruct { get; }
    public int? OptionalBStruct { get; }

    protected override void Configure(RequiredReferenceBuilder<NestedCommand> propertyBuilder)
    {
        propertyBuilder.MapRequiredReference(c => c.RequiredAReference)
            .MapRequiredReference(ar => ar.Value)
            .FromParameter(RequiredAReference, Error.Validation("RequiredAReference missing", ""));
        propertyBuilder.MapOptionalReference(c => c.OptionalAReference)
           .MapRequiredReference(ar => ar.Value)
           .FromParameter(OptionalAReference);

        propertyBuilder.MapRequiredReference(c => c.RequiredAStruct)
            .MapRequiredStruct(ast => ast.Value)
            .FromParameter(RequiredAStruct, Error.Validation("RequiredAStruct missing", ""));
        propertyBuilder.MapOptionalReference(c => c.OptionalAStruct)
          .MapRequiredStruct(ast => ast.Value)
          .FromParameter(OptionalAStruct);



        propertyBuilder.MapRequiredStruct(c => c.RequiredBReference)
            .MapRequiredReference(ar => ar.Value)
            .FromParameter(RequiredBReference, Error.Validation("RequiredBReference missing", ""));
        propertyBuilder.MapOptionalStruct(c => c.OptionalBReference)
           .MapRequiredReference(ar => ar.Value)
           .FromParameter(OptionalBReference);

        propertyBuilder.MapRequiredStruct(c => c.RequiredBStruct)
            .MapRequiredStruct(br => br.Value)
            .FromParameter(RequiredAStruct, Error.Validation("RequiredBStruct missing", ""));
        propertyBuilder.MapOptionalStruct(c => c.OptionalBStruct)
          .MapRequiredStruct(br => br.Value)
          .FromParameter(OptionalBStruct);
    }
}

public record AReference(string Value);

public record AStruct(int Value);

public record struct BReference(string Value);

public record struct BStruct(int Value);

public record NestedCommand(
    AReference RequiredAReference, AReference? OptionalAReference,
    AStruct RequiredAStruct, AStruct? OptionalAStruct,
    BReference RequiredBReference, BReference? OptionalBReference,
    BStruct RequiredBStruct, BStruct? OptionalBStruct);

#endregion

#region Direct CanFail

public class DirectCanFailTest : ReturnsReference<Test>
{
    public DirectCanFailTest(string? text, int? value, string? innerTest, int? innerInt)
    {
        Text = text;
        Value = value;
        InnerTest = innerTest;
        InnerInt = innerInt;
    }

    public string? Text { get; }
    public int? Value { get; }
    public string? InnerTest { get; }
    public int? InnerInt { get; }

    protected override void Configure(RequiredReferenceBuilder<Test> propertyBuilder)
    {
        propertyBuilder.MapRequiredReference(t => t.Param1)
            .ByCalling(CanFailDirect.Create, methodBuilder =>
            {
                methodBuilder.AddRequiredReference<string>().FromParameter(Text, Error.Validation("Text missing", ""));
                methodBuilder.AddRequiredStruct<int>().FromParameter(Value, Error.Validation("", ""));
                methodBuilder.AddRequiredReference<InnerParameter>(innerBuilder =>
                {
                     innerBuilder.MapRequiredReference(ip => ip.innertest)
                        .FromParameter(InnerTest, Error.Validation("InnerTest missing", ""));

                    innerBuilder.MapOptionalStruct(ip => ip.innerInt)
                        .FromParameter(InnerInt);
                });
            });
        propertyBuilder.MapOptionalReference(t => t.Param2)
           .ByCalling(CanFailDirect.Create, methodBuilder =>
           {
               methodBuilder.AddRequiredReference<string>().FromParameter(Text);
               methodBuilder.AddRequiredStruct<int>().FromParameter(Value);
           });
    }
}

public record InnerParameter(string innertest, int? innerInt);

public class CanFailDirect
{
    private CanFailDirect(string text, int value, InnerParameter inner)
    {
        Text = text;

        Value = value;
        Inner = inner;
    }

    public string Text { get; set; }

    public int Value { get; set; }
    public InnerParameter Inner { get; }

    public static CanFail<CanFailDirect> Create(string text, int value, InnerParameter inner)
    {
        CanFail<CanFailDirect> result = new();
        if(text == "failure")
        {
            return Error.Validation("Text.Failure", "text is invalid");
        }

        if(value < 0)
        {
            return Error.Conflict("test", "");
        }

        if(inner.innertest == "innerfailure")
        {
            return Error.Validation("Test2", "");
        }

        return result.SetValue(new CanFailDirect(text, value, inner));
    }
}

public record Test(CanFailDirect Param1, CanFailDirect? Param2);

#endregion