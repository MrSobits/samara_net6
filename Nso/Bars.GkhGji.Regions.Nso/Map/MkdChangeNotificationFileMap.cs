/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
/// 	using Bars.B4.DataAccess;
/// 	using Bars.GkhGji.Regions.Nso.Entities;
/// 
/// 	public class MkdChangeNotificationFileMap : BaseEntityMap<MkdChangeNotificationFile>
///     {
/// 		public MkdChangeNotificationFileMap()
/// 			: base("GJI_MKD_CHANGE_NOTIFICATION_FILE")
///         {
///             Map(x => x.Name, "DOC_NAME").Length(100);
///             Map(x => x.Number, "DOC_NUMBER").Length(50);
/// 			Map(x => x.Date, "DOC_DATE");
/// 			Map(x => x.Desc, "DOC_DESC").Length(500);
/// 
/// 			References(x => x.MkdChangeNotification, "MKD_CHANGE_NOTIFICATION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.MkdChangeNotificationFile"</summary>
    public class MkdChangeNotificationFileMap : BaseEntityMap<MkdChangeNotificationFile>
    {
        
        public MkdChangeNotificationFileMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.MkdChangeNotificationFile", "GJI_MKD_CHANGE_NOTIFICATION_FILE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("DOC_NAME").Length(100);
            Property(x => x.Number, "Number").Column("DOC_NUMBER").Length(50);
            Property(x => x.Date, "Date").Column("DOC_DATE");
            Property(x => x.Desc, "Desc").Column("DOC_DESC").Length(500);
            Reference(x => x.MkdChangeNotification, "MkdChangeNotification").Column("MKD_CHANGE_NOTIFICATION_ID").NotNull().Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
        }
    }
}
