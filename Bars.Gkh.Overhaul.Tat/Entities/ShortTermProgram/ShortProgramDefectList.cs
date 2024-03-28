namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Дефектная ведомость Краткосрочной программы
    /// </summary>
    public class ShortProgramDefectList : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ShortProgramRealityObject ShortObject { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Сумма по ведомости, руб
        /// </summary>
        public virtual decimal? Sum { get; set; }
    }
}