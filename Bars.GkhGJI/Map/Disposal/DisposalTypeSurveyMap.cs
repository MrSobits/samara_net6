/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Приложение распоряжения ГЖИ"
///     /// </summary>
///     public class DisposalTypeSurveyMap : BaseGkhEntityMap<DisposalTypeSurvey>
///     {
///         public DisposalTypeSurveyMap()
///             : base("GJI_DISPOSAL_TYPESURVEY")
///         {
///             References(x => x.Disposal, "DISPOSAL_ID").Not.Nullable().Fetch.Join();
///             References(x => x.TypeSurvey, "TYPESURVEY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Тип обследования рапоряжения ГЖИ"</summary>
    public class DisposalTypeSurveyMap : BaseEntityMap<DisposalTypeSurvey>
    {
        
        public DisposalTypeSurveyMap() : 
                base("Тип обследования рапоряжения ГЖИ", "GJI_DISPOSAL_TYPESURVEY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Disposal, "Распоряжение ГЖИ").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.TypeSurvey, "тип обследования").Column("TYPESURVEY_ID").NotNull().Fetch();
        }
    }
}
