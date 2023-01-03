using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain
{
    public class DatabaseErrorException : Exception
    {
        public DatabaseErrorException() 
        {
        }

        public DatabaseErrorException(string? errorDuring) : base($"A Database Error occured during {errorDuring} phase.")
        {
        }

        public DatabaseErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
