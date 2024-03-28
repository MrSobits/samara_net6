namespace Bars.Gkh.Repair.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Акт выполненных работ
    /// </summary>
    public class PerformedRepairWorkAct : BaseEntity
    {
        /// <summary>
        /// Текущий ремонт
        /// </summary>
        public virtual RepairWork RepairWork { get; set; }

        /// <summary>
        /// Фотография объекта
        /// </summary>
        public virtual FileInfo ObjectPhoto { get; set; }

        /// <summary>
        /// Описание фотографии
        /// </summary>
        public virtual string ObjectPhotoDescription { get; set; }

        /// <summary>
        /// Дата акта
        /// </summary>
        public virtual DateTime? ActDate { get; set; }

        /// <summary>
        /// Номер акта
        /// </summary>
        public virtual string ActNumber { get; set; }

        /// <summary>
        /// Объем выполненных работ
        /// </summary>
        public virtual decimal PerformedWorkVolume { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal ActSum { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo ActFile { get; set; }

        /// <summary>
        /// Описание акта
        /// </summary>
        public virtual string ActDescription { get; set; }
    }
}