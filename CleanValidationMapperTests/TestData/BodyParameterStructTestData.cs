using CleanValidationMapper.Errors;
using CleanValidationMapper.RequestValidation;

namespace CleanValidationMapperTests.TestData;

public static class BodyParameterStructTestData
{
	public static readonly Error MissingError = Error.Validation("Field.Mising", "The required field is missing");
	#region Optional

	public class OptionalBodyParameter : AbstractBody<OptionalCommand>
	{
        public OptionalBodyParameter(int? parameter)
        {
			Parameter = Parameter(parameter).IsOptional();
        }

		public int? Parameter { get; set; }
    }

	public record OptionalCommand(int? Parameter);

	#endregion

	#region Required

	public class RequiredBodyParameter : AbstractBody<RequiredCommand>
	{
		public RequiredBodyParameter(int? parameter)
		{
			Parameter = Parameter(parameter).IsRequired(MissingError);
		}

		public int Parameter { get; set; }
	}

	public record RequiredCommand(int? Parameter);

	#endregion
}
