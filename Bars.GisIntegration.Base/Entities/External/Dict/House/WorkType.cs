namespace Bars.GisIntegration.Base.Entities.External.Dict.House
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Dict.Common;

    /// <summary>
    /// Классификатор видов работ (услуг)
    /// </summary>
    public class WorkType : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string DictCode { get; set; }
        /// <summary>
        /// Код
        /// </summary>
        public virtual string DictCodeShort { get; set; }
        /// <summary>
        /// Уровень
        /// </summary>
        public virtual int Level { get; set; }
        /// <summary>
        /// НАименование
        /// </summary>
        public virtual string WorkTypeName { get; set; }
        /// <summary>
        /// ГУИД из ГИС
        /// </summary>
        public virtual string GisGuid { get; set; }
        /// <summary>
        /// Классификатор видов работ - родитель
        /// </summary>
        public virtual WorkType Parent { get; set; }
        /// <summary>
        /// ОКЕИ
        /// </summary>
        public virtual Okei Okei { get; set; }
        /// <summary>
        /// Кем изменено
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Когда изменено
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}
