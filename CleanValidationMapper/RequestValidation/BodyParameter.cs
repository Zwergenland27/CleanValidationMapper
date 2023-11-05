namespace CleanValidationMapper;

public abstract class BodyParameter
{
	protected readonly CanFail _validationResult = new();

	public CanFail ValidationResult => _validationResult;

}
