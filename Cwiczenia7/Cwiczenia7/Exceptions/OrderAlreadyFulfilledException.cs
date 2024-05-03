namespace Cwiczenia7.Exceptions;

public class OrderAlreadyFulfilledException : Exception
{
    public OrderAlreadyFulfilledException()
    {
    }

    public OrderAlreadyFulfilledException(string? message) : base(message)
    {
    }
}