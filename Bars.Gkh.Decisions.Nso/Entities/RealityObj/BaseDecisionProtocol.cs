namespace Bars.Gkh.Decisions.Nso.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Entities;

    /// <summary>
    ///     Базовая сущность протокола решения
    /// </summary>
    public class BaseDecisionProtocol : BaseImportableEntity, IStatefulEntity, IDecisionProtocol
    {
        /// <summary>
        ///     МКД
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        ///     Номер протокола
        /// </summary>
        public virtual string ProtocolNumber { get; set; }

        /// <summary>
        ///     Дата портокола
        /// </summary>
        public virtual DateTime ProtocolDate { get; set; }

        /// <summary>
        /// Дата вступления в силу
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        ///     Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        ///     Файл протокола
        /// </summary>
        public virtual FileInfo ProtocolFile { get; set; }

        /// <summary>
        ///     Состояние
        /// </summary>
        public virtual State State { get; set; }
    }
}