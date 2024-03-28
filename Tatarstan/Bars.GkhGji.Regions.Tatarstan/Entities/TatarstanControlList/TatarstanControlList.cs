namespace Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Проверочный лист.
    /// </summary>
    public class TatarstanControlList : BaseEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Уникальный идентификатор ТОР.
        /// </summary>
        public virtual Guid? TorId { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Сведения об утверждении
        /// </summary>
        public virtual string ApprovalDetails { get; set; }

        /// <summary>
        /// Дата создания проверочного листа.
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия проверочного листа.
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Скан-копия заполненного проверочного листа.
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Распоряжение.
        /// </summary>
        public virtual TatarstanDisposal Disposal { get; set; }

        /// <summary>
        /// Гуид ЕРП
        /// </summary>
        public virtual string ErpGuid { get; set; }
        
    }
}
