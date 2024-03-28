namespace Bars.GkhGji.Entities
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    using Newtonsoft.Json;

    /// <summary>
    /// Нарушения требований (Предостережение)
    /// </summary>
    public class WarningDocViolations : BaseEntity
    {
        /// <summary>
        /// Предостережение ГЖИ
        /// </summary>
        public virtual WarningDoc WarningDoc { get; set; }

        /// <summary>
        /// Описание нарушения
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Проверяемый объект
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Нормативный документ
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }

        /// <summary>
        /// Принятые меры
        /// </summary>
        public virtual string TakenMeasures { get; set; }

        /// <summary>
        /// Срок устранения нарушения
        /// </summary>
        public virtual DateTime? DateSolution { get; set; }

        /// <summary>
        /// Список нарушений
        /// </summary>
        [JsonIgnore]
        public virtual IList<WarningDocViolationsDetail> ViolationList { get; set; }
    }
}