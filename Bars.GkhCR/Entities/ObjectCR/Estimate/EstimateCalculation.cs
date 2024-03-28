namespace Bars.GkhCr.Entities
{
	using B4.Modules.FileStorage;
	using B4.Modules.States;
	using Bars.GkhCr.Enums;
	using Gkh.Entities;
	using System;

	using Bars.Gkh.Enums;

    /// <summary>
    /// Сметный расчет по работе
    /// </summary>
    public class EstimateCalculation : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Документ ведомости ресурсов
        /// </summary>
        public virtual string ResourceStatmentDocumentName { get; set; }

        /// <summary>
        /// Документ сметы
        /// </summary>
        public virtual string EstimateDocumentName { get; set; }

        /// <summary>
        /// Документ файла сметы
        /// </summary>
        public virtual string FileEstimateDocumentName { get; set; }

        /// <summary>
        /// Номер документа ведомости ресурсов
        /// </summary>
        public virtual string ResourceStatmentDocumentNum { get; set; }

        /// <summary>
        /// Номер документа сметы
        /// </summary>
        public virtual string EstimateDocumentNum { get; set; }

        /// <summary>
        /// Номер документа файла сметы
        /// </summary>
        public virtual string FileEstimateDocumentNum { get; set; }

        /// <summary>
        /// Дата от ведомости ресурсов
        /// </summary>
        public virtual DateTime? ResourceStatmentDateFrom { get; set; }

        /// <summary>
        /// Дата от сметы
        /// </summary>
        public virtual DateTime? EstimateDateFrom { get; set; }

        /// <summary>
        /// Дата от файла сметы
        /// </summary>
        public virtual DateTime? FileEstimateDateFrom { get; set; }

        /// <summary>
        /// Файл от ведомости ресурсов
        /// </summary>
        public virtual FileInfo ResourceStatmentFile { get; set; }

        /// <summary>
        /// Файл от сметы
        /// </summary>
        public virtual FileInfo EstimateFile { get; set; }

        /// <summary>
        /// Файл от файла сметы
        /// </summary>
        public virtual FileInfo FileEstimateFile { get; set; }

        /// <summary>
        /// Другие затраты
        /// </summary>
        public virtual decimal? OtherCost { get; set; }

        /// <summary>
        /// Итого по смете
        /// </summary>
        public virtual decimal? TotalEstimate { get; set; }

        /// <summary>
        /// Итого прямые затраты
        /// </summary>
        public virtual decimal? TotalDirectCost { get; set; }

        /// <summary>
        /// Накладные расходы
        /// </summary>
        public virtual decimal? OverheadSum { get; set; }

        /// <summary>
        /// НДС
        /// </summary>
        public virtual decimal? Nds { get; set; }

        /// <summary>
        /// Сметная прибыль
        /// </summary>
        public virtual decimal? EstimateProfit { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Сумма по ресурсам/материалам указана без НДС
        /// </summary>
        public virtual bool IsSumWithoutNds { get; set; }

		/// <summary>
		/// Тип сметы
		/// </summary>
		public virtual EstimationType EstimationType { get; set; }

        /// <summary>
        /// Выводить документ на портал
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }
    }
}
