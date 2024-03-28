namespace Bars.GkhCr.Entities
{
    using System;
    using B4.Modules.FileStorage;
    using B4.Modules.States;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Конкурс на проведение работ
    /// </summary>
    public class Competition : BaseImportableEntity, IStatefulEntity
    {
        /// <summary>
        /// Номер извещения
        /// </summary>
        public virtual string NotifNumber { get; set; }

        /// <summary>
        /// Дата извещения
        /// </summary>
        public virtual DateTime NotifDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Дата рассмотрения
        /// </summary>
        public virtual DateTime? ReviewDate { get; set; }

        /// <summary>
        /// Время рассмотрения
        /// </summary>
        public virtual DateTime? ReviewTime { get; set; }

        /// <summary>
        /// Место рассмотрения
        /// </summary>
        public virtual string ReviewPlace { get; set; }

        /// <summary>
        /// Дата проведения
        /// </summary>
        public virtual DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// Время проведения
        /// </summary>
        public virtual DateTime? ExecutionTime { get; set; }

        /// <summary>
        /// Место проведения
        /// </summary>
        public virtual string ExecutionPlace { get; set; }
    }
}