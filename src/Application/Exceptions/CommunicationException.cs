using System;

namespace Application.Exceptions {
    public class CommunicationException : ApplicationException {
        public CommunicationException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
