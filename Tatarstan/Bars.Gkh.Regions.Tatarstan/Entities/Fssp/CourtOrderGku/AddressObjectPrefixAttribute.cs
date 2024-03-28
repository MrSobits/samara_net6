namespace Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku
{
    using System;

    /// <summary>
    /// Атрибут префикса намиенования адресного объекта
    /// </summary>
    public class AddressObjectPrefixAttribute : Attribute
    {
        /// <summary>
        /// Префикс наименования
        /// </summary>
        public string Prefix { get; set; }

        public AddressObjectPrefixAttribute(string prefix)
        {
            this.Prefix = prefix;
        }
    }
}