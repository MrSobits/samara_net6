namespace Bars.Gkh.RegOperator.Exceptions
{
    using System;

    [Serializable]
    public class RealityObjectAccountGenerationException : Exception
    {
        public RealityObjectAccountGenerationException()
        {

        }
        public RealityObjectAccountGenerationException(string message)
            : base(message)
        {

        }
    }
} 
