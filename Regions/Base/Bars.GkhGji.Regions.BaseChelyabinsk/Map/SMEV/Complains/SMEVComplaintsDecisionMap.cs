namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVComplaintsDecisionMap : BaseEntityMap<SMEVComplaintsDecision>
    {
        
        public SMEVComplaintsDecisionMap() : 
                base("", "SMEV_CH_COMPLAINTS_DECISION")
        {
        }
        
        protected override void Map()
        {          
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Номер проверки").Column("NAME");
            Property(x => x.FullName, "Пояснительный текст к жалобе").Column("FULLNAME");
            Property(x => x.CompleteReject, "Номер проверки").Column("REC_TYPE");

        }
    }
}
