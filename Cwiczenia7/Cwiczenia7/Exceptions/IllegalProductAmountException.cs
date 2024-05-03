namespace Cwiczenia7.Exceptions;

public class IllegalProductAmountException : Exception
{
    public IllegalProductAmountException()
    {
    }

    public IllegalProductAmountException(string? message) : base(message)
    {
    }
}