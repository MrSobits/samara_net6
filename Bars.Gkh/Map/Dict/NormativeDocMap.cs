/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict
/// {
///     using Bars.Gkh.Entities.Dicts;
/// 
///     public class NormativeDocMap : BaseGkhEntityMap<NormativeDoc>
///     {
///         public NormativeDocMap()
///             : base("GKH_DICT_NORMATIVE_DOC")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.FullName, "FULLNAME").Not.Nullable().Length(1000);
///             Map(x => x.Code, "CODE").Not.Nullable();
///             Map(x => x.Category, "Category").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts
{
    using Bars.Gkh.Entities.Dicts;
    
    
    /// <summary>Маппинг для "Нормативно-правовой документ"</summary>
    public class NormativeDocMap : BaseImportableEntityMap<NormativeDoc>
    {
        
        public NormativeDocMap() : 
                base("Нормативно-правовой документ", "GKH_DICT_NORMATIVE_DOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.FullName, "Полное наименвоание").Column("FULLNAME").Length(1000).NotNull();
            Property(x => x.Code, "Код").Column("CODE").NotNull();
            Property(x => x.Category, "Категория").Column("CATEGORY").NotNull();
            Property(x => x.DateFrom, "Дата начала действия").Column("DATE_FROM");
            Property(x => x.DateTo, "Дата окончания действия").Column("DATE_TO");
            //Property(x => x.TorId, "TorId").Column("TOR_ID");
        }
    }
}
