namespace Bars.Gkh.RegOperator.Exceptions
{
    using System;

    [Serializable]
    public class PersonalAccountOperationException : Exception
    {
        public PersonalAccountOperationException() { }

        public PersonalAccountOperationException(string message)
            : base(message)
        {

        }
    }
}
