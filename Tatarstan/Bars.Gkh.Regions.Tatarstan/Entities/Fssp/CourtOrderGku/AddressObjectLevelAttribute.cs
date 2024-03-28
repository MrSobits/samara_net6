namespace Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku
{
    using System;
    
    /// <summary>
    /// Атрибут уровня адресного объекта
    /// </summary>
    public class AddressObjectLevelAttribute : Attribute
    {
        /// <summary>
        /// ровень адресного объекта
        /// </summary>
        public int Level { get; set; }

        public AddressObjectLevelAttribute(int level)
        {
            this.Level = level;
        }
    }
}