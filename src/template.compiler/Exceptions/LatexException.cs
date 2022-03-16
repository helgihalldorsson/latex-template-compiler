using System;

namespace template.compiler.Exceptions
{
    [Serializable]
    public class LatexException : Exception
    {
        public LatexException() { }
        public LatexException(string message) : base(message) { }
        public LatexException(string message, Exception inner) : base(message, inner) { }
        protected LatexException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
