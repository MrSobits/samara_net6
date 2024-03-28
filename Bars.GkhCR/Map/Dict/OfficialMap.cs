/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map.Dict
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class OfficialMap : BaseImportableEntityMap<Official>
///     {
///         public OfficialMap() : base("CR_DICT_OFFICIAL")
///         {
///             References(x => x.Operator, "OPERATOR_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.Fio, "FIO", true, 300);
///             Map(x => x.Code, "CODE", true, 300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Должностное лицо"</summary>
    public class OfficialMap : BaseImportableEntityMap<Official>
    {
        
        public OfficialMap() : 
                base("Должностное лицо", "CR_DICT_OFFICIAL")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Operator, "Оператор").Column("OPERATOR_ID").NotNull().Fetch();
            Property(x => x.Fio, "Фио").Column("FIO").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
        }
    }
}
