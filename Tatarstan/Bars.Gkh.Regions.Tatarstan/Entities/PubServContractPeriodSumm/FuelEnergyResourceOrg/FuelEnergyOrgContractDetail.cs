namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    /// <summary>
    /// Детализация по услуге
    /// </summary>
    public class FuelEnergyOrgContractDetail : BaseEntity
    {
        //TODO: уникальный индекс на связка ServiceOrgFuelEnergyResourcePeriodSumm и Услуга

        /// <summary>
        /// Агрегация по РСО в периоде
        /// </summary>
        public virtual ServiceOrgFuelEnergyResourcePeriodSumm PeriodSumm { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        /// <remarks>Здесь имеем ссылку на коренную услугу, т.к. собираем по всем договорам по одной услуге</remarks>
        public virtual ServiceDictionary Service { get; set; } 

        /// <summary>
        /// Начислено за месяц
        /// </summary>
        public virtual decimal Charged { get; set; }

        /// <summary>
        /// Оплачено за месяц
        /// </summary>
        public virtual decimal Paid { get; set; }

        /// <summary>
        /// Задолженность на конец месяца
        /// </summary>
        public virtual decimal Debt { get; set; }

        /// <summary>
        /// Процент планируемых оплат по газу
        /// </summary>
        public virtual PlanPaymentsPercentage GasEnergyPercents { get; set; }

        /// <summary>
        /// Процент планируемых оплат по электроэнергии
        /// </summary>
        public virtual PlanPaymentsPercentage ElectricityEnergyPercents { get; set; }

        /// <summary>
        /// Планируемая оплата Газа
        /// </summary>
        public virtual decimal PlanPayGas => this.Paid * (this.GasEnergyPercents?.Percentage ?? 0) / 100.0m;

        /// <summary>
        /// Планируемая оплата Электроэнергии
        /// </summary>
        public virtual decimal PlanPayElectricity => this.Paid * (this.ElectricityEnergyPercents?.Percentage ?? 0) / 100.0m;
    }
}