/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess;
///     using Entities;
/// 
///     public class RealityObjectResOrgMap : BaseImportableEntityMap<RealityObjectResOrg>
///     {
///         public RealityObjectResOrgMap()
///             : base("GKH_OBJ_RESORG")
///         {
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.ContractNumber, "CONTRACT_NUMBER");
///             Map(x => x.ContractDate, "CONTRACT_DATE");
///             Map(x => x.Note, "NOTE").Length(300);
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ResourceOrg, "RESORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.FileInfo, "FILE_INFO_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Договор поставщика коммунальных услуг с жилым домом"</summary>
    public class RealityObjectResOrgMap : BaseImportableEntityMap<RealityObjectResOrg>
    {
        
        public RealityObjectResOrgMap() : 
                base("Договор поставщика коммунальных услуг с жилым домом", "GKH_OBJ_RESORG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.ContractNumber, "Номер договора").Column("CONTRACT_NUMBER");
            Property(x => x.ContractDate, "Дата договора").Column("CONTRACT_DATE");
            Property(x => x.Note, "Примечание").Column("NOTE").Length(300);
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.ResourceOrg, "Поставщик коммунальных услуг").Column("RESORG_ID").NotNull().Fetch();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").Fetch();
        }
    }
}
