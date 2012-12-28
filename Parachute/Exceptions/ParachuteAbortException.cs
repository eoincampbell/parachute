using System;
using System.Runtime.Serialization;

namespace Parachute.Exceptions
{
    public class ParachuteAbortException : Exception
    {
        public ParachuteAbortException() :  base() { }

        public ParachuteAbortException(string message) : base(message) { }

        public ParachuteAbortException(string message, Exception inner) : base(message, inner) { }

        public ParachuteAbortException(SerializationInfo info, StreamingContext context) : base(info, context) { } 
    }
}