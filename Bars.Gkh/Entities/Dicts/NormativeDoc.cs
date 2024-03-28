namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.Entities.Base;
    using Enums;
    using System;

    /// <summary>
    /// Нормативно-правовой документ
    /// </summary>
    public class NormativeDoc : BaseGkhEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        public virtual NormativeDocCategory Category { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? DateTo { get; set; }

        /// <summary>
        /// Идентификатор из ТОР КНД
        /// </summary>
        public virtual Guid? TorId { get; set; }
    }
}