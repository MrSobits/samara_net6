/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict
/// {
///     using B4.DataAccess;
///     using Entities.Dicts;
/// 
///     /// <summary>
///     ///     Мап для пункта нормативного документа
///     /// </summary>
///     public class NormativeDocItemMap : BaseImportableEntityMap<NormativeDocItem>
///     {
///         public NormativeDocItemMap()
///             : base("GKH_DICT_NORMATIVE_DOC_ITEM")
///         {
///             Map(item => item.Number, "DOC_NUMBER").Not.Nullable().Length(400);
///             Map(item => item.Text, "DOC_TEXT").Length(2000);
///             References(item => item.NormativeDoc, "NORMATIVE_DOC_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;
    
    
    /// <summary>Маппинг для "Пункт нормативного документа"</summary>
    public class NormativeDocItemMap : BaseImportableEntityMap<NormativeDocItem>
    {
        
        public NormativeDocItemMap() : 
                base("Пункт нормативного документа", "GKH_DICT_NORMATIVE_DOC_ITEM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Number, "Номер").Column("DOC_NUMBER").Length(400).NotNull();
            Property(x => x.Text, "Текст (описание)").Column("DOC_TEXT").Length(2000);
            Reference(x => x.NormativeDoc, "Нормативный документ").Column("NORMATIVE_DOC_ID").NotNull().Fetch();
        }
    }
}
