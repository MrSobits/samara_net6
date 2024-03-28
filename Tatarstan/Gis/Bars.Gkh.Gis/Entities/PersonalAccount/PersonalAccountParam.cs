namespace Bars.Gkh.Gis.Entities.PersonalAccount
{
    using System;
    using B4.DataAccess;

    /// <summary>
    /// Параметр лицевого счета
    /// </summary>
    public class PersonalAccountParam : PersistentObject
    {
        /// <summary>
        /// Внутренний идентификатор квартиры (nzp_kvar)
        /// </summary>
        public virtual long ApartmentId { get; set; }
        
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
        /// Идентификатор параметра
        /// </summary>
        public virtual long PrmCode { get; set; }

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
