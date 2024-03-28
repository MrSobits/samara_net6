/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.ServOrg
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Договор организации поставщика жил. услуг"
///     /// </summary>
///     public class ServiceOrgContractMap : BaseGkhEntityMap<ServiceOrgContract>
///     {
///         public ServiceOrgContractMap()
///             : base("GKH_SORG_CONTRACT")
///         {
///             Map(x => x.DocumentNumber, "DOCUMENT_NUM").Length(300);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.Note, "NOTE").Length(300);
/// 
///             References(x => x.ServOrg, "SERV_ORG_ID").Fetch.Join();
///             References(x => x.FileInfo, "FILE_INFO_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Базовый класс договоров управления"</summary>
    public class ServiceOrgContractMap : BaseImportableEntityMap<ServiceOrgContract>
    {
        
        public ServiceOrgContractMap() : 
                base("Базовый класс договоров управления", "GKH_SORG_CONTRACT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DateStart, "Дата начала действия").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания действия").Column("DATE_END");
            Property(x => x.Note, "Примечание").Column("NOTE").Length(300);
            Reference(x => x.ServOrg, "Управляющая организация").Column("SERV_ORG_ID").Fetch();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").Fetch();
        }
    }
}
