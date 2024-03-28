/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Разрезы финансирования муниципального образования"
///     /// </summary>
///     public class MunicipalitySourceFinancingMap : BaseGkhEntityMap<MunicipalitySourceFinancing>
///     {
///         public MunicipalitySourceFinancingMap()
///             : base("GKH_DICT_MUNICIPAL_SOURCE")
///         {
///             Map(x => x.AddEk, "ADD_EK").Length(50);
///             Map(x => x.AddFk, "ADD_FK").Length(50);
///             Map(x => x.AddKr, "ADD_KR").Length(50);
///             Map(x => x.Kcsr, "KCSR").Length(50);
///             Map(x => x.Kes, "KES").Length(50);
///             Map(x => x.Kfsr, "KFSR").Length(50);
///             Map(x => x.Kif, "KIF").Length(50);
///             Map(x => x.Kvr, "KVR").Length(50);
///             Map(x => x.Kvsr, "KVSR").Length(50);
///             Map(x => x.SourceFinancing, "SOURCE_FINANCING").Length(50);
/// 
///             References(x => x.Municipality, "MUNICIPALITY_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Разрез финансирования муниципального образования"</summary>
    public class MunicipalitySourceFinancingMap : BaseImportableEntityMap<MunicipalitySourceFinancing>
    {
        
        public MunicipalitySourceFinancingMap() : 
                base("Разрез финансирования муниципального образования", "GKH_DICT_MUNICIPAL_SOURCE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.AddEk, "Доп. ЭК").Column("ADD_EK").Length(50);
            Property(x => x.AddFk, "Доп. ФК").Column("ADD_FK").Length(50);
            Property(x => x.AddKr, "Доп. КР").Column("ADD_KR").Length(50);
            Property(x => x.Kcsr, "КЦСР").Column("KCSR").Length(50);
            Property(x => x.Kes, "КЭС").Column("KES").Length(50);
            Property(x => x.Kfsr, "КФСР").Column("KFSR").Length(50);
            Property(x => x.Kif, "КИФ").Column("KIF").Length(50);
            Property(x => x.Kvr, "КВР").Column("KVR").Length(50);
            Property(x => x.Kvsr, "КВСР").Column("KVSR").Length(50);
            Property(x => x.SourceFinancing, "Разрез финансирования").Column("SOURCE_FINANCING").Length(50);
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").Fetch();
        }
    }
}
