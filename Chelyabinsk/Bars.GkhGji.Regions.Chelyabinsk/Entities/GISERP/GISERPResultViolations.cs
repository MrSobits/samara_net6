
namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    using System;

    public class GISERPResultViolations : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС ГИС ЕРП
        /// </summary>
        public virtual GISERP GISERP { get; set; }

        /// <summary>
        /// Сведения о выявленных нарушениях 
        /// </summary>
        public virtual String VIOLATION_NOTE { get; set; }

        /// <summary>
        /// Сведения о выявленных нарушениях 
        /// </summary>
        public virtual String VIOLATION_ACT { get; set; }

        /// <summary>
        ///Формулировка судебного сведения
        /// </summary>
        public virtual String TEXT { get; set; }

        /// <summary>
        ///Глобально-уникальный идентификатор записи формата GUID
        /// </summary>
        public virtual String NUM_GUID { get; set; }

        /// <summary>
        /// Простой тип - коды видов судебных сведений о выявленных нарушениях
        /// </summary>
        public virtual ERPVLawSuitType VLAWSUIT_TYPE_ID { get; set; }

        /// <summary>
        ///Реквизиты предписания об устранении выявленных нарушений в результате проведения КНМ
        /// </summary>
        public virtual String CODE { get; set; }

        /// <summary>
        /// Дата вынесения предписания об устранении выявленных нарушений в результате проведения КНМ
        /// </summary>
        public virtual DateTime? DATE_APPOINTMENT { get; set; }

        /// <summary>
        /// Дата вынесения предписания об устранении выявленных нарушений в результате проведения КНМ
        /// </summary>
        public virtual DateTime? EXECUTION_DEADLINE { get; set; }

        /// <summary>
        ///Реквизиты предписания об устранении выявленных нарушений в результате проведения КНМ
        /// </summary>
        public virtual String EXECUTION_NOTE { get; set; }       

    }
}
