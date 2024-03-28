using System;

namespace Bars.Gkh.Gis.Entities.Kp50
{
    using B4.DataAccess;

    /// <summary>
    /// Норматив биллинга
    /// </summary>
    public class BilNormativStorage : PersistentObject
    {
        /// <summary>
        /// Услуга биллинга
        /// </summary>
        public virtual BilServiceDictionary BilService { get; set; }
        
        /// <summary>
        /// Код типа норматива в биллинге
        /// </summary>
        public virtual int NormativeTypeCode { get; set; }

        /// <summary>
        /// Наименование типа норматива в биллинге
        /// </summary>
        public virtual string NormativeTypeName { get; set; }

        /// <summary>
        /// Код норматива в биллинге
        /// </summary>
        public virtual int NormativeCode { get; set; }

        /// <summary>
        /// Наименование норматива
        /// </summary>
        public virtual string NormativeName { get; set; }

        /// <summary>
        /// Описание наименования норматива
        /// </summary>
        public virtual string NormativeDescription { get; set; }

        /// <summary>
        /// Значение норматива
        /// </summary>
        public virtual string NormativeValue { get; set; }

        /// <summary>
        /// Дата начала действия норматива
        /// </summary>
        public virtual DateTime? NormativeStartDate { get; set; }

        /// <summary>
        /// Дата окончания действия норматива
        /// </summary>
        public virtual DateTime? NormativeEndDate { get; set; }
    }
}
