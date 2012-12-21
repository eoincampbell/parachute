using System;
using System.Runtime.Serialization;

namespace Parachute
{
    public class ParachuteException : Exception
    {
        public ParachuteException() :  base() { }

        public ParachuteException(string message) : base(message) { }

        public ParachuteException(string message, Exception inner) : base(message, inner) { }

        public ParachuteException(SerializationInfo info, StreamingContext context) : base(info,context) { } 

    }
}
