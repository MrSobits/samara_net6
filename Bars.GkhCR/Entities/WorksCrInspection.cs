namespace Bars.GkhCr.Entities
{
    using System;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;

    using Enums;

    /// <summary>
    /// Обследование объекта
    /// </summary>
    public class WorksCrInspection : BaseImportableEntity
    {
        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual TypeWorkCr TypeWork { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual Official Official { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Плановая дата
        /// </summary>
        public virtual DateTime? PlanDate { get; set; }

        /// <summary>
        /// Факт обследования
        /// </summary>
        public virtual InspectionState InspectionState { get; set; }

        /// <summary>
        /// Фактическая дата
        /// </summary>
        public virtual DateTime? FactDate { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}