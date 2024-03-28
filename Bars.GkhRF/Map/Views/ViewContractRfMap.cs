/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhRf.Entities;
/// 
///     public class ViewContractRfMap : PersistentObjectMap<ViewContractRf>
///     {
///         public ViewContractRfMap()
///             : base("VIEW_RF_CONTRACT")
///         {
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.ContractRfObjectsCount, "CONTRACT_OBJS_COUNT");
///             Map(x => x.ManagingOrganizationName, "MAN_ORG_NAME");
///             Map(x => x.MunicipalityName, "MUNICIPALITY_NAME");
///             Map(x => x.ManagingOrganizationId, "MAN_ORG_ID");
///             Map(x => x.ContragentId, "CONTRAGENT_ID");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.SumAreaMkd, "SUM_AREA_MKD");
///             Map(x => x.SumAreaLivingOwned, "SUM_AREA_OWNED");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Вьюха на Договор рег. фонда"</summary>
    public class ViewContractRfMap : PersistentObjectMap<ViewContractRf>
    {
        
        public ViewContractRfMap() : 
                base("Вьюха на Договор рег. фонда", "VIEW_RF_CONTRACT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.ContractRfObjectsCount, "Количество объектов").Column("CONTRACT_OBJS_COUNT");
            Property(x => x.ManagingOrganizationName, "Управляющая организация").Column("MAN_ORG_NAME");
            Property(x => x.MunicipalityName, "Муниципальное образование").Column("MUNICIPALITY_NAME");
            Property(x => x.ManagingOrganizationId, "Управляющая организация(id)").Column("MAN_ORG_ID");
            Property(x => x.ContragentId, "Идентификатор контрагента").Column("CONTRAGENT_ID");
            Property(x => x.MunicipalityId, "Идентификатор муниципального образования").Column("MU_ID");
            Property(x => x.SumAreaMkd, "Cумма общих площадей из жилых домов вкладки \"Дома, включенные в договор\"").Column("SUM_AREA_MKD");
            Property(x => x.SumAreaLivingOwned, "Cумма площадей в собственности граждан из жилых домов вкладки \"Дома, включенные в" +
                    " договор\"").Column("SUM_AREA_OWNED");
        }
    }
}
