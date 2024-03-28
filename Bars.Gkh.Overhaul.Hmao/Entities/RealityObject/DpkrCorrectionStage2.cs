namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сущность, содержащая данные, необходимые при учете корректировки ДПКР
    /// Лимит займа, Дефицит ...
    /// </summary>
    public class DpkrCorrectionStage2 : BaseImportableEntity
    {
        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Этап 2 формирования ДПКР
        /// </summary>
        public virtual VersionRecordStage2 Stage2 { get; set; }

        /// <summary>
        /// Скорректированный год
        /// </summary>
        public virtual int PlanYear { get; set; }

        /// <summary>
        /// Собираемость к году
        /// </summary>
        public virtual decimal YearCollection { get; set; }
        
        /// <summary>
        /// Остаток средств собственников
        /// </summary>
        public virtual decimal OwnersMoneyBalance { get; set; }
        
        /// <summary>
        /// Есть непогашенный кредит
        /// </summary>
        public virtual bool HasCredit { get; set; }

        /// <summary>
        /// Бюджет фонда
        /// </summary>
        public virtual decimal BudgetFundBalance { get; set; }

        /// <summary>
        /// Бюджет региона
        /// </summary>
        public virtual decimal BudgetRegionBalance { get; set; }

        /// <summary>
        /// Бюджет МО
        /// </summary>
        public virtual decimal BudgetMunicipalityBalance { get; set; }

        /// <summary>
        /// Дриугие источники
        /// </summary>
        public virtual decimal OtherSourceBalance { get; set; }

        /// <summary>
        /// Потребность бюджета фонда
        /// </summary>
        public virtual decimal FundBudgetNeed { get; set; }

        /// <summary>
        /// Потребность бюджета региона
        /// </summary>
        public virtual decimal RegionBudgetNeed { get; set; }

        /// <summary>
        /// Потребность бюджета МО
        /// </summary>
        public virtual decimal MunicipalityBudgetNeed { get; set; }

        /// <summary>
        /// Потребность бюджета иных источников
        /// </summary>
        public virtual decimal OtherSourcesBudgetNeed { get; set; }

        /// <summary>
        /// Потребность в финансировании собственников
        /// </summary>
        public virtual decimal OwnersMoneyNeed { get; set; }

        /// <summary>
        /// Признак того, что строка поучавствовала в расчете баланса собственников и ее считать заново не надо
        /// </summary>
        public virtual bool IsOwnerMoneyBalanceCalculated { get; set; }
    }
}