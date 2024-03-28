namespace Bars.GkhCr.Entities
{
    using System;

    using Bars.GkhCr.Enums;

    using Gkh.Entities;

    /// <summary>
    /// Архив значений в мониторинге СМР
    /// </summary>
    public class ArchiveSmr : BaseGkhEntity
    {
        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Этап работы
        /// </summary>
        public virtual StageWorkCr StageWorkCr { get; set; }

        /// <summary>
        /// Объем выполнения
        /// </summary>
        public virtual decimal? VolumeOfCompletion { get; set; }

        /// <summary>
        /// Производитель
        /// </summary>
        public virtual string ManufacturerName { get; set; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        public virtual decimal? PercentOfCompletion { get; set; }

        /// <summary>
        /// Сумма расходов
        /// </summary>
        public virtual decimal? CostSum { get; set; }

        /// <summary>
        /// Чиленность рабочих(дробное так как м.б. смысл поля как пол ставки)
        /// </summary>
        public virtual decimal? CountWorker { get; set; }

        /// <summary>
        /// Дата изменения записи
        /// </summary>
        public virtual DateTime? DateChangeRec { get; set; }

        /// <summary>
        /// Тип архива значений в мониторинге СМР
        /// </summary>
        public virtual TypeArchiveSmr TypeArchiveSmr { get; set; }
    }
}
