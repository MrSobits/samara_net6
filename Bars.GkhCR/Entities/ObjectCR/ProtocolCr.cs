namespace Bars.GkhCr.Entities
{
	using Bars.B4.Modules.FileStorage;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Entities.Dicts.Multipurpose;
	using System;

	using Bars.Gkh.Enums;

    /// <summary>
    /// Протокол(акт)
    /// </summary>
    public class ProtocolCr : BaseGkhEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Участник
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
		public virtual MultipurposeGlossaryItem TypeDocumentCr { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual TypeWorkCr TypeWork { get; set; }

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

        /// <summary>
        /// Собственник, участвующий в приемке работ
        /// </summary>
        public virtual string OwnerName { get; set; }

        /// <summary>
        /// Используется при экспорте
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }

        /// <summary>
        /// Решение ОМС
        /// </summary>
        public virtual bool DecisionOms { get; set; }
    }
}
