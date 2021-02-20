using System;

namespace Application.Exceptions {
    public class CommunicationException : ApplicationException {
        public CommunicationException(string message) : base(message) { }
        public CommunicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
