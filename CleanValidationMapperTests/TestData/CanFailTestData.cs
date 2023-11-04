using CleanValidationMapper;
using System.Configuration;
using System.Net.Http.Headers;

namespace CleanValidationMapperTests.TestData;

public static class CanFailTestData
{
    public static readonly Error Error = Error.Conflict("CanFailTestData.Conflict", "Message");

	private static List<Error> CreateErrorList(int errors)
	{
		List<Error> list = new();

		for (int i = 0; i < errors; i++)
		{
			list.Add(Error.Conflict($"CanFailTestData.Conflict.{i}", $"Message {i}"));
		}

		return list;
	}

	private static List<CanFail> CreateResultList(int results)
	{
		List<CanFail> list = new();

		for(int i = 0; i < results; i++)
		{
			CanFail result = new();
			Error error = Error.Conflict($"CanFailTestData.Conflict.{i}", $"Message {i}");
			result.Failed(Error);
			list.Add(result);
		}

		return list;
	}

	public class ErrorLists : TheoryData<List<Error>>
	{
        public ErrorLists()
        {
            for(int i = 1; i < 3; i++)
            {
                Add(CreateErrorList(i));
            }
        }
    }

	public class ResultLists : TheoryData<List<CanFail>>
	{
        public ResultLists()
        {
			for(int i = 1; i < 3; i++)
			{
				Add(CreateResultList(i));
			}
		}
    }
}
