namespace Bookanizer.REST.Exceptions
{
    public sealed class InsufficientReadHistoryException : Exception
    {
        public InsufficientReadHistoryException(string message) : base(message) { }
    }
}
