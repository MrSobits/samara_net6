/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CompetitionMap : BaseImportableEntityMap<Competition>
///     {
///         public CompetitionMap() : base("CR_COMPETITION")
///         {
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
/// 
///             Map(x => x.NotifNumber, "NOTIF_NUMBER", false, 100);
///             Map(x => x.NotifDate, "NOTIF_DATE", true);
/// 
///             Map(x => x.ReviewDate, "REVIEW_DATE");
///             Map(x => x.ReviewTime, "REVIEW_TIME");
///             Map(x => x.ReviewPlace, "REVIEW_PLACE", false, 300);
/// 
///             Map(x => x.ExecutionDate, "EXEC_DATE");
///             Map(x => x.ExecutionTime, "EXEC_TIME");
///             Map(x => x.ExecutionPlace, "EXEC_PLACE", false, 100);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Конкурс на проведение работ"</summary>
    public class CompetitionMap : BaseImportableEntityMap<Competition>
    {
        
        public CompetitionMap() : 
                base("Конкурс на проведение работ", "CR_COMPETITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.NotifNumber, "Номер извещения").Column("NOTIF_NUMBER").Length(100);
            Property(x => x.NotifDate, "Дата извещения").Column("NOTIF_DATE").NotNull();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Property(x => x.ReviewDate, "Дата рассмотрения").Column("REVIEW_DATE");
            Property(x => x.ReviewTime, "Время рассмотрения").Column("REVIEW_TIME");
            Property(x => x.ReviewPlace, "Место рассмотрения").Column("REVIEW_PLACE").Length(300);
            Property(x => x.ExecutionDate, "Дата проведения").Column("EXEC_DATE");
            Property(x => x.ExecutionTime, "Время проведения").Column("EXEC_TIME");
            Property(x => x.ExecutionPlace, "Место проведения").Column("EXEC_PLACE").Length(100);
        }
    }
}
