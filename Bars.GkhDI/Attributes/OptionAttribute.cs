namespace Bars.GkhDi.Attributes
{
    using System;

    /// <summary>
    /// Аттрибут для настраиваемых полей
    /// </summary>
    public class OptionFieldAttribute : Attribute
    {
        /// <summary>
        /// Конструктор <see cref="OptionFieldAttribute"/> класса.
        /// </summary>
        /// <param name="name">
        /// Имя.
        /// </param>
        public OptionFieldAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }
    }
}
