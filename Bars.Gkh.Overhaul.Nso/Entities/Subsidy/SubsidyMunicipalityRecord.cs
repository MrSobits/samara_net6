namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    ///  Запись субсидии для муниципального образования
    /// </summary>
    public class SubsidyMunicipalityRecord : BaseEntity
    {
        /// <summary>
        /// Субсидия МО
        /// </summary>
        public virtual SubsidyMunicipality SubsidyMunicipality { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int SubsidyYear { get; set; }

        /// <summary>
        /// Бюджет фонда
        /// </summary>
        public virtual decimal BudgetFund { get; set; }

        /// <summary>
        /// Бюджет региона
        /// </summary>
        public virtual decimal BudgetRegion { get; set; }

        /// <summary>
        /// Бюджет МО
        /// </summary>
        public virtual decimal BudgetMunicipality { get; set; }

        /// <summary>
        /// Дриугие источники
        /// </summary>
        public virtual decimal OtherSource { get; set; }

        /// <summary>
        /// Расчетная собираемость
        /// </summary>
        public virtual decimal CalculatedCollection { get; set; }

        /// <summary>
        /// Лимит средств собственников
        /// </summary>
        public virtual decimal OwnersLimit { get; set; }

        /// <summary>
        /// Доля бюджета фонда
        /// </summary>
        public virtual decimal ShareBudgetFund { get; set; }

        /// <summary>
        /// Доля бюджета региона
        /// </summary>
        public virtual decimal ShareBudgetRegion { get; set; }

        /// <summary>
        /// Доля бюджета МО
        /// </summary>
        public virtual decimal ShareBudgetMunicipality { get; set; }

        /// <summary>
        /// Доля дриугих источников
        /// </summary>
        public virtual decimal ShareOtherSource { get; set; }

        /// <summary>
        /// Доля cредств собственников
        /// </summary>
        public virtual decimal ShareOwnerFounds { get; set; }

        /// <summary>
        /// Потребность в финансировании ДО
        /// </summary>
        [ObsoleteAttribute("Не использовать для расчетов. Вместо этого поля брать данные из SubsidyRecordVersionData", true)]
        public virtual decimal FinanceNeedBefore { get; set; }

        /// <summary>
        /// Потребность в финансировании ПОСЛЕ
        /// </summary>
        [ObsoleteAttribute("Не использовать для расчетов. Вместо этого поля брать данные из SubsidyRecordVersionData", true)]
        public virtual decimal FinanceNeedAfter { get; set; }

        /// <summary>Потребность в финансировании в зависимости была 
        ///     или нет корректировка ДПКР
        /// </summary>
        public virtual decimal FinanceNeedFromCorrect { get; set; }

        /// <summary>
        /// Установленный тариф
        /// </summary>
        public virtual decimal EstablishedTarif { get; set; }

        /// <summary>
        /// Начальный рекомендуемый тариф
        /// </summary>
        public virtual decimal StartRecommendedTarif { get; set; }

        /// <summary>
        /// Рекомендуемый тариф
        /// </summary>
        [ObsoleteAttribute("Не использовать для расчетов. Вместо этого поля брать данные из SubsidyRecordVersionData", true)]
        public virtual decimal RecommendedTarif { get; set; }

        /// <summary>
        /// Собираемость по рекомендуемому тарифу
        /// </summary>
        [ObsoleteAttribute("Не использовать для расчетов. Вместо этого поля брать данные из SubsidyRecordVersionData", true)]
        public virtual decimal RecommendedTarifCollection { get; set; }

        /// <summary>
        /// Дифицит/Профицит До
        /// </summary>
        [ObsoleteAttribute("Не использовать для расчетов. Вместо этого поля брать данные из SubsidyRecordVersionData", true)]
        public virtual decimal DeficitBefore { get; set; }

        /// <summary>
        /// Дифицит/Профицит После
        /// </summary>
        public virtual decimal DeficitAfter { get; set; }

        /// <summary>
        /// Дифицит/Профицит После корректировки
        /// </summary>
        public virtual decimal DeficitFromCorrect { get; set; }
    }
}