using System;
using System.Runtime.Serialization;

namespace Parachute.Exceptions
{
    /// <summary>
    /// A special type of parachute exception for intentionally & gracefully aborting the application
    /// </summary>
    public class ParachuteAbortException : ParachuteException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParachuteAbortException" /> class.
        /// </summary>
        public ParachuteAbortException() :  base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParachuteAbortException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ParachuteAbortException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParachuteAbortException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public ParachuteAbortException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParachuteAbortException" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public ParachuteAbortException(SerializationInfo info, StreamingContext context) : base(info, context) { } 
    }
}