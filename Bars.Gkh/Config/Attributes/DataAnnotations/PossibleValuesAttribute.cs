namespace Bars.Gkh.Config.Attributes.DataAnnotations
{
    using System;

    /// <summary>
    /// Аттрибут для указания возможных значений простых типов
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PossibleValuesAttribute : Attribute
    {
        private readonly object[] _values;

        public PossibleValuesAttribute(params object[] values)
        {
            _values = values;
        }

        public object[] Values
        {
            get { return _values; }
        }
    }
}