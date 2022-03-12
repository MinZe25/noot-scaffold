namespace noot_scaffold.Exceptions;

public class CancelledScaffoldException : Exception
{
    public CancelledScaffoldException(string message) : base(message)
    {
    }

    public CancelledScaffoldException()
    {
    }
}