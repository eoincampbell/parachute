using System;
using System.Runtime.Serialization;

namespace Parachute.Exceptions
{
    /// <summary>
    /// A type of exception for critcal errors which occur with in the application and require it to halt.
    /// </summary>
    public class ParachuteException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParachuteException" /> class.
        /// </summary>
        public ParachuteException() :  base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParachuteException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ParachuteException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParachuteException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public ParachuteException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParachuteException" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public ParachuteException(SerializationInfo info, StreamingContext context) : base(info,context) { } 
    }
}
