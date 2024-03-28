/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CompetitionProtocolMap : BaseImportableEntityMap<CompetitionProtocol>
///     {
///         public CompetitionProtocolMap() : base("CR_COMPETITION_PROTOCOL")
///         {
///             References(x => x.Competition, "COMPETITION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
/// 
///             Map(x => x.ExecDate, "EXEC_DATE");
///             Map(x => x.ExecTime, "EXEC_TIME");
///             Map(x => x.IsCancelled, "IS_CANCELLED", true);
///             Map(x => x.Note, "CNOTE", false, 500);
///             Map(x => x.SignDate, "SIGN_DATE", true);
///             Map(x => x.TypeProtocol, "TYPE_PROTOCOL", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Протокол конкурса"</summary>
    public class CompetitionProtocolMap : BaseImportableEntityMap<CompetitionProtocol>
    {
        
        public CompetitionProtocolMap() : 
                base("Протокол конкурса", "CR_COMPETITION_PROTOCOL")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Competition, "Конкурс").Column("COMPETITION_ID").NotNull().Fetch();
            Property(x => x.TypeProtocol, "Тип протокола").Column("TYPE_PROTOCOL").NotNull();
            Property(x => x.SignDate, "Дата подписания протокола").Column("SIGN_DATE").NotNull();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Property(x => x.ExecDate, "Дата проведения процедуры").Column("EXEC_DATE");
            Property(x => x.ExecTime, "Время проведения процедуры").Column("EXEC_TIME");
            Property(x => x.Note, "Примечание").Column("CNOTE").Length(500);
            Property(x => x.IsCancelled, "Конкурс признан несостоявшимся").Column("IS_CANCELLED").NotNull();
        }
    }
}
