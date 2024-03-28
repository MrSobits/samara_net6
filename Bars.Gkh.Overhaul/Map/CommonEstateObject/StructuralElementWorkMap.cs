/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Entities;
/// 
///     public class StructuralElementWorkMap : BaseImportableEntityMap<StructuralElementWork>
///     {
///         public StructuralElementWorkMap()
///             : base("OVRHL_STRUCT_EL_WORK")
///         {
///             References(x => x.StructuralElement, "STRUCT_EL_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Job, "JOB_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Работа по конструктивному элементу"</summary>
    public class StructuralElementWorkMap : BaseImportableEntityMap<StructuralElementWork>
    {
        
        public StructuralElementWorkMap() : 
                base("Работа по конструктивному элементу", "OVRHL_STRUCT_EL_WORK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.StructuralElement, "Конструктивный элемент").Column("STRUCT_EL_ID").NotNull().Fetch();
            Reference(x => x.Job, "Работа").Column("JOB_ID").NotNull().Fetch();
        }
    }
}
