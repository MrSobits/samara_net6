/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Entities;
///     using Bars.Gkh.Overhaul.Enum;
/// 
///     public class WorkTypeFinSourceMap : BaseImportableEntityMap<WorkTypeFinSource>
///     {
///         public WorkTypeFinSourceMap() : base("OVRHL_DICT_WORK_TYPE_FIN")
///         {
///             Map(x => x.TypeFinSource, "TYPE_FIN_SOURCE").Not.Nullable().CustomType<TypeFinSource>();
/// 
///             References(x => x.Work, "WORK_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Источник финансирования вида работ"</summary>
    public class WorkTypeFinSourceMap : BaseImportableEntityMap<WorkTypeFinSource>
    {
        
        public WorkTypeFinSourceMap() : 
                base("Источник финансирования вида работ", "OVRHL_DICT_WORK_TYPE_FIN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeFinSource, "Тип источника финансирования").Column("TYPE_FIN_SOURCE").NotNull();
            Reference(x => x.Work, "Работа").Column("WORK_ID").NotNull().Fetch();
        }
    }
}
