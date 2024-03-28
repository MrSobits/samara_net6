namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVComplaintsDecisionLifeSituationMap : BaseEntityMap<SMEVComplaintsDecisionLifeSituation>
    {
        
        public SMEVComplaintsDecisionLifeSituationMap() : 
                base("", "SMEV_CH_COMPLAINTS_DECISION_LS")
        {
        }
        
        protected override void Map()
        {          
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Номер проверки").Column("NAME");
            Property(x => x.FullName, "Пояснительный текст к жалобе").Column("FULLNAME");
            Reference(x => x.SMEVComplaintsDecision, "Номер проверки").Column("DEC_ID");
        }
    }
}
