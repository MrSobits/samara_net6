namespace Bars.GkhCr.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Entities;

    /// <summary>
    /// Предложение о проведении капитального ремонта
    /// </summary>
    public class OverhaulProposal : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Программа
        /// </summary>
        public virtual ProgramCr ProgramCr { get; set; }

        /// <summary>
        /// Программа
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Номер по программе
        /// </summary>
        public virtual string ProgramNum { get; set; }

        /// <summary>
        /// Дата завершения работ подрядчиком
        /// </summary>
        public virtual DateTime? DateEndBuilder { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }      

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
       
    }
}