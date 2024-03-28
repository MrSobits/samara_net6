namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class SMEVNDFLAnswerMap : BaseEntityMap<SMEVNDFLAnswer>
    {
        
        public SMEVNDFLAnswerMap() : 
                base("Запрос к ВС ФНС по 2-ндфл", "GJI_CH_SMEV_NDFL_ANSWER")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVNDFL, "ЗАПРОС").Column("SMEV_NDFL_ID").NotNull().Fetch();
            Property(x => x.INNUL, "ИННЮЛ").Column("INNUL");
            Property(x => x.KPP, "КПП").Column("KPP");
            Property(x => x.OrgName, "Наименование организации").Column("ORG_NAME");
            Property(x => x.Rate, "Ставка").Column("RATE");
            Property(x => x.RevenueCode, "КодДохода").Column("REVENUE_CODE");
            Property(x => x.Month, "Месяц").Column("MONTH");
            Property(x => x.RevenueSum, "СумДохода").Column("REVENUE_SUM");
            Property(x => x.RecoupmentCode, "КодВычета").Column("RECOUPMENT_CODE");
            Property(x => x.RecoupmentSum, "СумВычета").Column("RECOUPMENT_SUM");
            Property(x => x.DutyBase, "НалБаза").Column("DUTY_BASE");
            Property(x => x.DutySum, "Сумма налога исчисленная").Column("DUTY_SUM");
            Property(x => x.UnretentionSum, "Сумма налога, не удержанная налоговым агентом").Column("UNRETENTION_SUM");
            Property(x => x.RevenueTotalSum, "СумДохОбщ").Column("REVENUE_TOTAL_SUM");

        }
    }
}
