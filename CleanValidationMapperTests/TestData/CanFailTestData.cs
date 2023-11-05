using CleanValidationMapper;
using CleanValidationMapper.Errors;
using Moq;

namespace CleanValidationMapperTests.TestData;

public static class CanFailTestData
{
    private static List<AbstractCanFail> CreateResultMockList(int results, int errors)
    {
		List<AbstractCanFail> list = new();

        for (int i = 0; i < results; i++)
        {
			AbstractCanFail result = new();

            ErrorTestData.CreateErrorList(errors).ForEach(error =>
            {
                result.Failed(error);
            });

            list.Add(result);
        }

        return list;
    }

    public class ResultLists : TheoryData<List<AbstractCanFail>>
    {
        public ResultLists()
        {
            for (int results = 1; results < 3; results++)
            {
                for (int errors = 1; errors < 3; errors++)
                {
                    Add(CreateResultMockList(results, errors));
                }
            }
        }
    }

    public class SingleErrorFailureTypes : TheoryData<Error, FailureType>
    {
        public SingleErrorFailureTypes()
        {
            Add(Error.Conflict("", ""), FailureType.SingleConflict);
			Add(Error.NotFound("", ""), FailureType.SingleNotFound);
			Add(Error.Validation("", ""), FailureType.SingleValidation);
			Add(Error.Unexpected("", ""), FailureType.SingleUnexpected);
		}
    }

	public class MultipleErrorFailureTypes : TheoryData<Error, FailureType>
	{
		public MultipleErrorFailureTypes()
		{
			Add(Error.Conflict("", ""), FailureType.MultipleConflict);
			Add(Error.NotFound("", ""), FailureType.MultipleNotFound);
			Add(Error.Validation("", ""), FailureType.MultipleValidation);
			Add(Error.Unexpected("", ""), FailureType.MultipleUnexpected);
		}
	}
}
