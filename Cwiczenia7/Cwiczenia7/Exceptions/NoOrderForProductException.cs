namespace Cwiczenia7.Exceptions;

public class NoOrderForProductException : Exception
{
    public NoOrderForProductException()
    {
    }

    public NoOrderForProductException(string? message) : base(message)
    {
    }
}