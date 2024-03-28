namespace Bars.GkhCr.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
	using Bars.GkhCr.Enums;

    /// <summary>
    /// Вьюха на сметный расчет объекта КР
    /// </summary>
    /*
     * Данная вьюха предназначена для реестра Сметный расчет по выбранному объекту КР
     * с агрегированными показателями из реестра Смет и Ведомость ресурсов:
     * Сумма по смете
     * Сумма по ресурсам
     */
    public class ViewObjCrEstimateCalcDetail : PersistentObject
    {
        /// <summary>
        /// Объект кап ремонта
        /// </summary>
        public virtual int ObjectCrId { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Наименование работы
        /// </summary>
        public virtual string WorkName { get; set; }

        /// <summary>
        /// Наименование источника финансировния
        /// </summary>
        public virtual string FinSourceName { get; set; }

        /// <summary>
        /// Сумма по сметам
        /// </summary>
        public virtual decimal? SumEstimate { get; set; }

        /// <summary>
        /// Сумма по ведомостям ресурсов
        /// </summary>
        public virtual decimal? SumResource { get; set; }

        /// <summary>
        /// Итоги по смете
        /// </summary>
        public virtual decimal? TotalEstimate { get; set; }

		/// <summary>
		/// Тип сметы
		/// </summary>
		public virtual EstimationType EstimationType { get; set; }
    }
}