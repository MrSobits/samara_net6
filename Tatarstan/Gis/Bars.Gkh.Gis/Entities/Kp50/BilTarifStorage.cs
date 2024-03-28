namespace Bars.Gkh.Gis.Entities.Kp50
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Тариф биллинга
    /// </summary>
    public class BilTarifStorage : PersistentObject
    {
        /// <summary>
        /// Услуга биллинга
        /// </summary>
        public virtual BilServiceDictionary BilService { get; set; }

        /// <summary>
        /// Дом МЖФ
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Код поставщика в биллинге
        /// </summary>
        public virtual int SupplierCode { get; set; }

        /// <summary>
        /// Наименование поставщика
        /// </summary>
        public virtual string SupplierName { get; set; }

        /// <summary>
        /// Код методики расчета в биллинге
        /// </summary>
        public virtual int FormulaCode { get; set; }

        /// <summary>
        /// Код типа получения тарифа методики расчета в биллинге
        /// </summary>
        public virtual int FormulaTypeCode { get; set; }

        /// <summary>
        /// Наименование методики расчета
        /// </summary>
        public virtual string FormulaName { get; set; }

        /// <summary>
        /// Код тарифа в биллинге
        /// </summary>
        public virtual int TarifCode { get; set; }
        
        /// <summary>
        /// Наименование тарифа
        /// </summary>
        public virtual string TarifName { get; set; }

        /// <summary>
        /// Код типа тарифа в биллинге
        /// </summary>
        public virtual int TarifTypeCode { get; set; }

        /// <summary>
        /// Наименование типа тарифа в биллинге
        /// </summary>
        public virtual string TarifTypeName { get; set; }

        /// <summary>
        /// Значение тарифа
        /// </summary>
        public virtual decimal TarifValue { get; set; }
        
        /// <summary>
        /// Дата начала действия тарифа
        /// </summary>
        public virtual DateTime? TarifStartDate  { get; set; }

        /// <summary>
        /// Дата окончания действия тарифа
        /// </summary>
        public virtual DateTime? TarifEndDate { get; set; }

        /// <summary>
        /// Количество лицевых счетов
        /// </summary>
        public virtual long LsCount { get; set; }

    }
}