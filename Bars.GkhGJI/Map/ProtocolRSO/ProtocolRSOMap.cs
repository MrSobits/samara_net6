namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Протокол прокуратуры"</summary>
    public class ProtocolRSOMap : JoinedSubClassMap<ProtocolRSO>
    {
        
        public ProtocolRSOMap() : 
                base("Протокол РСО", "GJI_PROTOCOLRSO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.DateSupply, "Дата поступления в ГЖИ").Column("DATE_SUPPLY");
            Property(x => x.TypeSupplierProtocol, "Тип РСО составившей протокол").Column("TYPE_SUPPLIER");
            Reference(x => x.Executant, "Тип исполнителя документа").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
            Reference(x => x.GasSupplier, "РСО, вынесшее протокл").Column("RSO_ID").Fetch();
        }
    }
}
