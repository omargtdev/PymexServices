using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pymex.Services.Exceptions
{
    [Serializable]
    public class InsufficientPermissionsException : Exception
    {
        public string Username { get; }

        public InsufficientPermissionsException() { }

        public InsufficientPermissionsException(string message) : base(message) { }

        public InsufficientPermissionsException(string message, Exception innerException) : base(message, innerException) { }

        public InsufficientPermissionsException(string message, string username) : base(message)
        {
            Username = username;
        }
    }
}
