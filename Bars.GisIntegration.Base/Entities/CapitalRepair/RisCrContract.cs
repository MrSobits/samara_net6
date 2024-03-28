namespace Bars.GisIntegration.Base.Entities.CapitalRepair
{
    using System;

    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Импортируемый договор
    /// </summary>
    public class RisCrContract : BaseRisEntity
    {
        /// <summary>
        /// Идентификатор КПР в ГИС ЖКХ
        /// </summary>
        public virtual string PlanGUID { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Дата начала выполнения работ
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания выполнения работ
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Сумма договора
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public virtual string Customer { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual string Performer { get; set; }

        /// <summary>
        /// Гарантийный срок (месяцев)
        /// </summary>
        public virtual int WarrantyMonthCount => 60;

        /// <summary>
        /// Сметная документация отсутсвует
        /// </summary>
        public virtual bool OutlayMissing { get; set; }

        /// <summary>
        /// Адрес страницы в сети Интернет, на которой размещена информация об отборе подрядных организаций
        /// </summary>
        public virtual string TenderInetAddress => "http://fgkh.tatarstan.ru/";
    }
}
