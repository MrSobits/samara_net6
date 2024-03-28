namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Устав деятельности ТСЖ
    /// </summary>
    public class ActivityTsjStatute : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Деятельность ТСЖ
        /// </summary>
        public virtual ActivityTsj ActivityTsj { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string DocNumber { get; set; }

        /// <summary>
        /// Дата предоставления устава
        /// </summary>
        public virtual DateTime? StatuteProvisionDate { get; set; }

        /// <summary>
        /// Дата утверждения устава
        /// </summary>
        public virtual DateTime? StatuteApprovalDate { get; set; }

        /// <summary>
        /// Файл устава
        /// </summary>
        public virtual FileInfo StatuteFile { get; set; }

        /// <summary>
        /// Дата заключения
        /// </summary>
        public virtual DateTime? ConclusionDate { get; set; }

        /// <summary>
        /// Номер заключения
        /// </summary>
        public virtual string ConclusionNum { get; set; }

        /// <summary>
        /// Описание заключения
        /// </summary>
        public virtual string ConclusionDescription { get; set; }

        /// <summary>
        /// Тип заключения
        /// </summary>
        public virtual TypeConclusion TypeConclusion { get; set; }

        /// <summary>
        /// Файл заключения
        /// </summary>
        public virtual FileInfo ConclusionFile { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}