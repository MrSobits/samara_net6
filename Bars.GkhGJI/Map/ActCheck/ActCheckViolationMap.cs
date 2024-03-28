/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Нарушение в акте ГЖИ"
///     /// </summary>
///     public class ActCheckViolationMap : SubclassMap<ActCheckViolation>
///     {
///         public ActCheckViolationMap()
///         {
///             Table("GJI_ACTCHECK_VIOLAT");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             // Может быть нарушения без домов, поэтому может быть Null
///             References(x => x.ActObject, "ACTCHECK_ROBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Этап выявления нарушения в акте проверки Данная таблица служит связтю между нарушением и Актом проверки Чтобы понимать какие Нарушения были выявлены входе проверки Для выявленных нарушений ставится плановая дата устранения"</summary>
    public class ActCheckViolationMap : JoinedSubClassMap<ActCheckViolation>
    {
        
        public ActCheckViolationMap() : 
                base("Этап выявления нарушения в акте проверки Данная таблица служит связтю между наруш" +
                        "ением и Актом проверки Чтобы понимать какие Нарушения были выявлены входе провер" +
                        "ки Для выявленных нарушений ставится плановая дата устранения", "GJI_ACTCHECK_VIOLAT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActObject, "Дом акта проверки").Column("ACTCHECK_ROBJECT_ID").NotNull().Fetch();
        }
    }
}
