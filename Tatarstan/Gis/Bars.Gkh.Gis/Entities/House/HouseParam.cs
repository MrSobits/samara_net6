namespace Bars.Gkh.Gis.Entities.House
{
    using System;
    using B4.DataAccess;

    /// <summary>
    /// Дом
    /// </summary>
    public class HouseParam : PersistentObject
    {
        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public virtual long HouseId { get; set; }
        
        /// <summary>
        /// Наименование параметра
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Тип параметра (bool, int, sprav...)
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// код справочника связанного с параметром	res_y
        /// </summary>
        public virtual long? NzpRes { get; set; }

        /// <summary>
        /// Номер параметра
        /// </summary>
        public virtual long PrmNum { get; set; }

        /// <summary>
        /// Значение параметра
        /// </summary>
        public virtual string ValPrm { get; set; }

        /// <summary>
        /// Наименование таблицы
        /// </summary>
        public virtual string NameTab { get; set; }

        /// <summary>
        /// Наименование строки
        /// </summary>
        public virtual string NameY { get; set; }

        /// <summary>
        /// Начало действия значения
        /// </summary>
        public virtual DateTime DateBegin { get; set; }

        /// <summary>
        /// Окончание действия значения
        /// </summary>
        public virtual DateTime DateEnd { get; set; }
    }
}
