﻿using System;
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

    public class ParachuteAbortException : Exception
    {
        public ParachuteAbortException() :  base() { }

        public ParachuteAbortException(string message) : base(message) { }

        public ParachuteAbortException(string message, Exception inner) : base(message, inner) { }

        public ParachuteAbortException(SerializationInfo info, StreamingContext context) : base(info, context) { } 
    }
}
