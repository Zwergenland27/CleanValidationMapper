using CleanValidationMapper;

namespace CleanValidationMapperTests.TestData;

public static class ErrorTestData
{
	public static readonly Error Error = Error.Conflict("Error.Conflict", "Message");
	public static List<Error> CreateErrorList(int errors, string customExtension = "")
	{
		List<Error> list = new();

		for (int i = 0; i < errors; i++)
		{
			list.Add(Error.Conflict($"Error.Conflict.{i}.{customExtension}", $"Message {i} {customExtension}"));
		}

		return list;
	}
	public class ErrorLists : TheoryData<List<Error>>
	{
		public ErrorLists()
		{
			for (int i = 1; i < 3; i++)
			{
				Add(CreateErrorList(i));
			}
		}
	}
}
