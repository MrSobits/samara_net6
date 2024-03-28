/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     public class DocumentsMap : BaseGkhEntityMap<Documents>
///     {
///         /// <summary>
///         /// Документы по 731 постановлению правительства РФ
///         /// </summary>
///         public DocumentsMap(): base("DI_DISINFO_DOCUMENTS")
///         {
///             Map(x => x.NotAvailable, "NOT_AVAILABLE").Not.Nullable();
///             Map(x => x.DescriptionProjectContract, "DESCR_PROJ_CONTR").Length(1500);
/// 
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.FileProjectContract, "FILE_PROJ_CONTR_ID").Fetch.Join();
///             References(x => x.FileCommunalService, "FILE_COMMUNAL_ID").Fetch.Join();
///             References(x => x.FileServiceApartment, "FILE_APARTMENT_ID").Fetch.Join();
///             Map(x => x.DescriptionCommunalCost, "DESCR_COMMUNAL_COST").Length(1500);
///             Map(x => x.DescriptionCommunalTariff, "DESCR_COMMUNAL_TARIF").Length(2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.Documents"</summary>
    public class DocumentsMap : BaseImportableEntityMap<Documents>
    {
        
        public DocumentsMap() : 
                base("Bars.GkhDi.Entities.Documents", "DI_DISINFO_DOCUMENTS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.NotAvailable, "NotAvailable").Column("NOT_AVAILABLE").NotNull();
            Property(x => x.DescriptionProjectContract, "DescriptionProjectContract").Column("DESCR_PROJ_CONTR").Length(1500);
            Property(x => x.DescriptionCommunalCost, "DescriptionCommunalCost").Column("DESCR_COMMUNAL_COST").Length(1500);
            Property(x => x.DescriptionCommunalTariff, "DescriptionCommunalTariff").Column("DESCR_COMMUNAL_TARIF").Length(2000);
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
            Reference(x => x.FileProjectContract, "FileProjectContract").Column("FILE_PROJ_CONTR_ID").Fetch();
            Reference(x => x.FileCommunalService, "FileCommunalService").Column("FILE_COMMUNAL_ID").Fetch();
            Reference(x => x.FileServiceApartment, "FileServiceApartment").Column("FILE_APARTMENT_ID").Fetch();
        }
    }
}
