/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class FsGorodMapItemMap : BaseImportableEntityMap<FsGorodMapItem>
///     {
///         public FsGorodMapItemMap() : base("REGOP_FS_IMPORT_MAP_ITEM")
///         {
///             Map(x => x.ErrorText, "ERROR_TEXT");
///             Map(x => x.GetValueFromRegex, "GET_VAL_FROM_REGEX");
///             Map(x => x.Index, "ITEM_INDEX");
///             Map(x => x.IsMeta, "IS_META");
///             Map(x => x.PropertyName, "PROPERTY_NAME");
///             Map(x => x.Regex, "REGEX_VAL");
///             Map(x => x.RegexSuccessValue, "REGEX_SUCC_VAL");
///             Map(x => x.UseFilename, "USE_FILENAME");
///             Map(x => x.Format, "FORMAT");
///             Map(x => x.Required, "REQUIRED");
/// 
///             References(x => x.ImportInfo, "INFO_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.FsGorodMapItem"</summary>
    public class FsGorodMapItemMap : BaseImportableEntityMap<FsGorodMapItem>
    {
        
        public FsGorodMapItemMap() : 
                base("Bars.Gkh.RegOperator.Entities.FsGorodMapItem", "REGOP_FS_IMPORT_MAP_ITEM")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PaymentAgent, "GKH_PAYMENT_AGENT").Column("PAYMENT_AGENT_ID");
            Reference(x => x.ImportInfo, "ImportInfo").Column("INFO_ID").NotNull().Fetch();
            Property(x => x.IsMeta, "IsMeta").Column("IS_META");
            Property(x => x.Index, "Index").Column("ITEM_INDEX");
            Property(x => x.PropertyName, "PropertyName").Column("PROPERTY_NAME").Length(250);
            Property(x => x.Regex, "Regex").Column("REGEX_VAL").Length(250);
            Property(x => x.GetValueFromRegex, "GetValueFromRegex").Column("GET_VAL_FROM_REGEX");
            Property(x => x.RegexSuccessValue, "RegexSuccessValue").Column("REGEX_SUCC_VAL").Length(250);
            Property(x => x.ErrorText, "ErrorText").Column("ERROR_TEXT").Length(250);
            Property(x => x.UseFilename, "UseFilename").Column("USE_FILENAME");
            Property(x => x.Format, "Format").Column("FORMAT").Length(250);
            Property(x => x.Required, "Required").Column("REQUIRED");
        }
    }
}
