using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain
{
    public class EvenementException : Exception
    {
        public EvenementException()
        {
        }

        public EvenementException(string? errorMessage) : base($"Deze handeling kon niet worden uitgevoerd: {errorMessage}")
        {
        }

        public EvenementException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
