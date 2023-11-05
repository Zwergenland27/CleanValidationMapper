namespace CleanValidationMapper;

public class Playground
{

	private CanFail MultipleFails()
	{
		CanFail result = new();
		result.Failed(Error.Unexpected("Fail1", "Description"));
		result.Failed(Error.Unexpected("Fail2", "Description"));
		return result;
	}

	private CanFail MultipleFailsFromOtherResult()
	{
		CanFail result = new();
		result.Failed(MultipleFails());
		return result;
	}

	private CanFail<string?> FailsFromOtherResult()
	{
		return (string?) null;
	}
}
