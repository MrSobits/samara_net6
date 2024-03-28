namespace Bars.Gkh.Utils.Attributes
{
    using System;

    /// <summary>
    /// Аттрибут привязки типа
    /// </summary>
    public class TypeAttribute : Attribute
    {
        /// <summary>
        /// Тип
        /// </summary>
        public Type Type { get; set; }

        public TypeAttribute(Type type)
        {
            Type = type;
        }
    }
}