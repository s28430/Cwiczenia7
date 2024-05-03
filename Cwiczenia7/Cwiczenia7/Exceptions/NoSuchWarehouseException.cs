namespace Cwiczenia7.Exceptions;

public class NoSuchWarehouseException : Exception
{
    public NoSuchWarehouseException()
    {
    }

    public NoSuchWarehouseException(string? message) : base(message)
    {
    }
}