namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Протокол Краткосрочной программы
    /// </summary>
    public class ShortProgramProtocol : BaseEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ShortProgramRealityObject ShortObject { get; set; }

        /// <summary>
        /// Участник
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Доля принявших участие на кв. м.
        /// </summary>
        public virtual decimal? CountAccept { get; set; }

        /// <summary>
        /// Количество голосов на кв. м.
        /// </summary>
        public virtual decimal? CountVote { get; set; }

        /// <summary>
        /// Количество голосов общее на кв. м.
        /// </summary>
        public virtual decimal? CountVoteGeneral { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Оценка жителей
        /// </summary>
        public virtual int? GradeOccupant { get; set; }

        /// <summary>
        /// Оценка заказчика
        /// </summary>
        public virtual int? GradeClient { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Сумма Акта сверки данных о расходах
        /// </summary>
        public virtual decimal? SumActVerificationOfCosts { get; set; }
    }
}
