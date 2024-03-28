namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Уведомления решения собственников помещений МКД (при формирования фонда КР на спец.счете)
    /// </summary>
    public class SpecialAccountDecisionNotice : BaseImportableEntity, IStatefulEntity
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Решение собственников помещений МКД
        /// </summary>
        public virtual SpecialAccountDecision SpecialAccountDecision { get; set; }

        /// <summary>
        /// Номер уведомления (числ)
        /// </summary>
        public virtual int NoticeNum { get; set; }

        /// <summary>
        /// Номер уведомления
        /// </summary>
        public virtual string NoticeNumber { get; set; }

        /// <summary>
        /// Дата уведомления
        /// </summary>
        public virtual DateTime? NoticeDate { get; set; }

        /// <summary>
        /// Документ уведомления
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Дата регистрации уведомления в ГЖИ
        /// </summary>
        public virtual DateTime? RegDate { get; set; }

        /// <summary>
        /// Входящий номер в ГЖИ
        /// </summary>
        public virtual string GjiNumber { get; set; }

        /// <summary>
        /// Оригинал уведомления поступил
        /// </summary>
        public virtual bool HasOriginal { get; set; }

        /// <summary>
        /// Копия протокола поступила
        /// </summary>
        public virtual bool HasCopyProtocol { get; set; }

        /// <summary>
        /// Копия справки поступила
        /// </summary>
        public virtual bool HasCopyCertificate { get; set; }
    }
}