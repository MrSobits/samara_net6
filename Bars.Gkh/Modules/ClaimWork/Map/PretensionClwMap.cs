namespace Bars.Gkh.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    
    
    /// <summary>Маппинг для "Претензия"</summary>
    public class PretensionClwMap : JoinedSubClassMap<PretensionClw>
    {
        
        public PretensionClwMap() : 
                base("Претензия", "CLW_PRETENSION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DateReview, "Дата рассмотрения").Column("REVIEW_DATE");
            this.Property(x => x.DebtBaseTariffSum, "Сумма по базовому тарифу").Column("DEBT_BASE_TARIFF_SUM");
            this.Property(x => x.DebtDecisionTariffSum, "Сумма по тарифу решения").Column("DEBT_DECISION_TARIFF_SUM");
            this.Property(x => x.Sum, "Сумма").Column("SUM");
            this.Property(x => x.Penalty, "Пени").Column("PENALTY");
            this.Property(x => x.SumPenaltyCalc, "Расчет суммы претензии (пени)").Column("SUM_PENALTY_CALC").Length(1000);
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Property(x => x.SendDate, "Дата отправки").Column("SEND_DATE");
            this.Property(x => x.RequirementSatisfaction, "Удовлетворение требований").Column("REQ_SATISFACTION").DefaultValue(RequirementSatisfaction.Not).NotNull();
            this.Property(x => x.NumberPretension, "Номер претензии").Column("PRETENSION_NUMBER");
        }
    }
}
