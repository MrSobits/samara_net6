/// <mapping-converter-backup>
/// using Bars.B4.DataAccess;
/// 
/// namespace Bars.GkhGji.Map
/// {
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "План инспекционных проверок"
///     /// </summary>
///     public class PlanInsCheckGjiMap : BaseEntityMap<PlanInsCheckGji>
///     {
///         public PlanInsCheckGjiMap()
///             : base("GJI_DICT_PLANINSCHECK")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "План инспекционных проверок ГЖИ"</summary>
    public class PlanInsCheckGjiMap : BaseEntityMap<PlanInsCheckGji>
    {
        
        public PlanInsCheckGjiMap() : 
                base("План инспекционных проверок ГЖИ", "GJI_DICT_PLANINSCHECK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.DateApproval, "Дата утверждения плана").Column("DATE_APPROVAL");
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
        }
    }
}
