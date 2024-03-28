namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using System;


    /// <summary>Маппинг для "Платежный агент"</summary>
    public class ContragentClwMap : BaseEntityMap<ContragentClw>
    {
        
        public ContragentClwMap() : 
                base("Платежный агент", "GKH_CONTRAGENT_CLW")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            this.Property(x => x.DateFrom, "Дела с даты").Column("DATE_FROM").NotNull();
            this.Property(x => x.DateTo, "Дела по дату").Column("DATE_TO").NotNull();
        }
    }
}
