using CleanValidationMapper;

namespace CleanValidationMapperTests.TestData;

public static class BodyParameterReferenceTestData
{
	public static readonly Error MissingError = Error.Validation("Field.Mising", "The required field is missing");
	#region Optional

	public class OptionalBodyParameter : AbstractBody<OptionalCommand>
	{
        public OptionalBodyParameter(string? parameter)
        {
			Parameter = Parameter(parameter).IsOptional();
        }

		public string? Parameter { get; set; }
    }

	public record OptionalCommand(string? Parameter);

	#endregion

	#region Required

	public class RequiredBodyParameter : AbstractBody<RequiredCommand>
	{
		public RequiredBodyParameter(string? parameter)
		{
			Parameter = Parameter(parameter).IsRequired(MissingError);
		}

		public string Parameter { get; set; }
	}

	public record RequiredCommand(string? Parameter);

	#endregion
}
