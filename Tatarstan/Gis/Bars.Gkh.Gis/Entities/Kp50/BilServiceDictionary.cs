namespace Bars.Gkh.Gis.Entities.Kp50
{
    using B4.DataAccess;

    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Справочник услуг биллинга
    /// </summary>
    public class BilServiceDictionary : PersistentObject
    {
        /// <summary>
        /// Код префикса схемы в БД биллинга
        /// </summary>
        public virtual BilDictSchema Schema { get; set; }

        /// <summary>
        /// Эталонная услуга в МЖФ
        /// </summary>
        public virtual ServiceDictionary Service { get; set; }
        
        /// <summary>
        /// Код услуги в биллинге
        /// </summary>
        public virtual int ServiceCode { get; set; }

        /// <summary>
        /// Наименование услуги
        /// </summary>
        public virtual string ServiceName { get; set; }

        /// <summary>
        /// Код типа услуги в биллинге
        /// </summary>
        public virtual int ServiceTypeCode { get; set; }

        /// <summary>
        /// Наименование типа услуги в биллинге
        /// (жилищная, коммунальная и тд)
        /// </summary>
        public virtual string ServiceTypeName { get; set; }

        /// <summary>
        /// Код единицы измерения в биллинге
        /// </summary>
        public virtual int MeasureCode { get; set; }

        /// <summary>
        /// Наименование единицы измерения
        /// </summary>
        public virtual string MeasureName { get; set; }
        
        /// <summary>
        /// Порядковый номер (в  ЕПД)
        /// </summary>
        public virtual int OrderNumber { get; set; }

        /// <summary>
        /// Признак "Услуга предоставляется на общедомовые нужды (ОДН)"
        /// (true - ОДН, false - не ОДН)
        /// </summary>
        public virtual bool IsOdnService { get; set; }

        /// <summary>
        /// Код родительской услуги в биллинге
        /// </summary>
        public virtual int ParentServiceCode { get; set; }
    }
}