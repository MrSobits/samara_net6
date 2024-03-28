/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class RealityObjectDirectManagContractMap : SubclassMap<RealityObjectDirectManagContract>
///     {
///         public RealityObjectDirectManagContractMap()
///         {
///             Table("GKH_OBJ_DIRECT_MANAG_CNRT");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             this.Map(x => x.IsServiceContract, "IS_SERV_CONTR");
/// 
///             this.Map(x => x.DateStartService, "SERV_START_DATE");
///             this.Map(x => x.DateEndService, "SERV_END_DATE");
/// 
///             this.References(x => x.ServContractFile, "SERV_FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Договор непосредственного управления жилым домом"</summary>
    public class RealityObjectDirectManagContractMap : JoinedSubClassMap<RealityObjectDirectManagContract>
    {
        
        public RealityObjectDirectManagContractMap() : 
                base("Договор непосредственного управления жилым домом", "GKH_OBJ_DIRECT_MANAG_CNRT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.IsServiceContract, "Договор оказания услуг").Column("IS_SERV_CONTR");
            Property(x => x.DateStartService, "Дата начала оказания услуг").Column("SERV_START_DATE");
            Property(x => x.DateEndService, "Дата окончания оказания услуг").Column("SERV_END_DATE");
            Reference(x => x.ServContractFile, "Файл договора").Column("SERV_FILE_ID").Fetch();
        }
    }
}
