namespace Bars.Gkh.Decisions.Nso.Map.Decisions
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Базовая сущность протокола решения"</summary>
    public class BaseDecisionProtocolMap : BaseImportableEntityMap<BaseDecisionProtocol>
    {
        public BaseDecisionProtocolMap() : 
                base("Базовая сущность протокола решения", "DEC_BASE_PROTOCOL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ProtocolNumber, "Номер протокола").Column("PROTOCOL_NUM").Length(250);
            this.Property(x => x.ProtocolDate, "Дата протокола").Column("PROTOCOL_DATE");
            this.Property(x => x.DateStart, "Дата вступления в силу").Column("DATE_START");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(250);

            this.Reference(x => x.RealityObject, "МКД").Column("RO_ID").NotNull().Fetch();
            this.Reference(x => x.ProtocolFile, "Файл протокола").Column("PROT_FILE_ID").Fetch();
            this.Reference(x => x.State, "Состояние").Column("STATE_ID").Fetch();
        }
    }
}