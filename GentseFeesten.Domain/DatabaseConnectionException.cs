namespace GentseFeesten.Domain
{
    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException() 
        {
        }

        public DatabaseConnectionException(string? errorDuring) : base($"Er deed zich een fout voor bij volgende uitvoering richting de database: {errorDuring}.")
        {
        }

        public DatabaseConnectionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
