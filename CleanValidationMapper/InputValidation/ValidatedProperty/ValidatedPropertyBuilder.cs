namespace CleanValidationMapper.InputValidation.ValidatedProperty;

public abstract class ValidatedPropertyBuilder<T>
{
    public abstract CanFail<T> Execute();
}
