namespace Bookanizer.REST.Exceptions
{
    public sealed class RecommenderUnavailableException : Exception
    {
        public RecommenderUnavailableException(string message, Exception? inner = null)
            : base(message, inner) { }
    }
}
