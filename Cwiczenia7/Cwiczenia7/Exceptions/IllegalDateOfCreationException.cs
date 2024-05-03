namespace Cwiczenia7.Exceptions;

public class IllegalDateOfCreationException : Exception
{
    public IllegalDateOfCreationException()
    {
    }

    public IllegalDateOfCreationException(string? message) : base(message)
    {
    }
}