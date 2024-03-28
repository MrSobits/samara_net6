/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.Gkh.Enums;
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Дом акта проверки"
///     /// </summary>
///     public class ActCheckRealityObjectMap : BaseGkhEntityByCodeMap<ActCheckRealityObject>
///     {
///         public ActCheckRealityObjectMap() : base("GJI_ACTCHECK_ROBJECT")
///         {
///             Map(x => x.Description, "DESCRIPTION", false, 2000);
///             Map(x => x.NotRevealedViolations, "NOT_REVEALED_VIOLATIONS", false, 1000);
///             Map(x => x.HaveViolation, "HAVE_VIOLATION", false);
///             Map(x => x.PersonsWhoHaveViolated, "PERSONS_WHO_HAVE_VIOLATED", false, 1000);
///             Map(x => x.OfficialsGuiltyActions, "OFFICIALS_GUILTY_ACTIONS", false, 1000);
/// 
///             References(x => x.ActCheck, "ACTCHECK_ID", ReferenceMapConfig.NotNull);
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дом акта проверки Данная таблица хранит всебе все дома которые необходимо проверить"</summary>
    public class ActCheckRealityObjectMap : BaseEntityMap<ActCheckRealityObject>
    {
        
        public ActCheckRealityObjectMap() : 
                base("Дом акта проверки Данная таблица хранит всебе все дома которые необходимо провери" +
                        "ть", "GJI_ACTCHECK_ROBJECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            Reference(x => x.ActCheck, "Акт проверки").Column("ACTCHECK_ID").NotNull();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").Fetch();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.NotRevealedViolations, "Не выявленные нарушения").Column("NOT_REVEALED_VIOLATIONS").Length(1000);
            Property(x => x.HaveViolation, "Признак выявлено или невыявлено нарушение").Column("HAVE_VIOLATION");
            Property(x => x.PersonsWhoHaveViolated, "Сведения о лицах, допустивших нарушения").Column("PERSONS_WHO_HAVE_VIOLATED").Length(1000);
            Property(x => x.OfficialsGuiltyActions, "Сведения, свидетельствующие, что нарушения допущены в результате виновных действи" +
                    "й (бездействия) должностных лиц и (или) работников проверяемого лица").Column("OFFICIALS_GUILTY_ACTIONS").Length(1000);
            this.Property(x => x.ErpGuid, nameof(ActCheckRealityObject.ErpGuid)).Column("ERP_GUID");

        }
    }
}
