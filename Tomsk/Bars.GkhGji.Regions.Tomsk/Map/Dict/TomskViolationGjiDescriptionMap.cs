/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map.Dict
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tomsk.Entities.Dict;
/// 
///     using NHibernate.Type;
/// 
///     public class TomskViolationGjiDescriptionMap : BaseEntityMap<TomskViolationGjiDescription>
///     {
///         public TomskViolationGjiDescriptionMap()
///             : base("GJI_TOMSK_DICT_VIOL_DESCR")
///         {
///             this.References(x => x.ViolationGji, "VIOLATION_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.RuleOfLaw,
///                 mapper =>
///                     {
///                         mapper.Column("RULE_OF_LAW");
///                         mapper.Type<BinaryBlobType>();
///                     });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities.Dict;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.Dict.TomskViolationGjiDescription"</summary>
    public class TomskViolationGjiDescriptionMap : BaseEntityMap<TomskViolationGjiDescription>
    {
        
        public TomskViolationGjiDescriptionMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.Dict.TomskViolationGjiDescription", "GJI_TOMSK_DICT_VIOL_DESCR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ViolationGji, "ViolationGji").Column("VIOLATION_ID").NotNull();
            Property(x => x.RuleOfLaw, "RuleOfLaw").Column("RULE_OF_LAW");
        }
    }

    public class TomskViolationGjiDescriptionNHibernateMapping : ClassMapping<TomskViolationGjiDescription>
    {
        public TomskViolationGjiDescriptionNHibernateMapping()
        {
            Property(
                x => x.RuleOfLaw,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
