namespace Bars.GkhCr.Entities
{
    using System;

    using B4.Modules.FileStorage;
    using B4.Modules.States;

    using Bars.Gkh.Enums;
    using Bars.GkhCr.Enums;

    using Gkh.Entities;
    using Gkh.Entities.Dicts;

    /// <summary>
    /// Дефектная ведомость
    /// </summary>
    public class DefectList : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual TypeWorkCr TypeWork { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Сумма по ведомости, руб
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal? Volume { get; set; }

        /// <summary>
        /// Стоимость на единицу объема по ведомости
        /// </summary>
        public virtual decimal? CostPerUnitVolume { get; set; }

        /// <summary>
        /// Тип дефектной ведомости
        /// </summary>
        public virtual TypeDefectList? TypeDefectList { get; set; }

        /// <summary>
        /// Выводить документ на портал
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }
    }
}
