
namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Enums;
    using System;

    public class GASUData : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС ГАСУ
        /// </summary>
        public virtual GASU GASU { get; set; }

        /// <summary>
        /// УИД показателя
        /// </summary>
        public virtual String IndexUid { get; set; }

        /// <summary>
        /// Единица измерения ОКЕИ
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }      

        /// <summary>
        ///Наименование показателя
        /// </summary>
        public virtual String Indexname { get; set; }       

        /// <summary>
        ///Значение показателя
        /// </summary>
        public virtual decimal Value { get; set; }
    }
}
