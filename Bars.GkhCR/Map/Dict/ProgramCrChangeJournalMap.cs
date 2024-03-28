/// <mapping-converter-backup>
/// using Bars.B4.DataAccess.ByCode;
/// using Bars.GkhCr.Enums;
/// 
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Журнал изменений программы КР"
///     /// </summary>
///     public class ProgramCrChangeJournalMap : BaseImportableEntityMap<ProgramCrChangeJournal>
///     {
///         public ProgramCrChangeJournalMap()
///             : base("CR_DICT_PROG_CHANGE_JOUR")
///         {
///             References(x => x.ProgramCr, "PROGRAM_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.TypeChange, "TYPE_CHANGE", true, TypeChangeProgramCr.Manually);
///             Map(x => x.ChangeDate, "DATE_CHANGE", true);
///             Map(x => x.MuCount, "MU_COUNT");
///             Map(x => x.UserName, "USER_NAME");
///             Map(x => x.Description, "DESCRIPTION");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    
    
    /// <summary>Маппинг для "Журнал изменений программы КР"</summary>
    public class ProgramCrChangeJournalMap : BaseImportableEntityMap<ProgramCrChangeJournal>
    {
        
        public ProgramCrChangeJournalMap() : 
                base("Журнал изменений программы КР", "CR_DICT_PROG_CHANGE_JOUR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ProgramCr, "Программа").Column("PROGRAM_ID").NotNull().Fetch();
            Property(x => x.TypeChange, "Способ формирования").Column("TYPE_CHANGE").DefaultValue(TypeChangeProgramCr.Manually).NotNull();
            Property(x => x.ChangeDate, "Дата").Column("DATE_CHANGE").NotNull();
            Property(x => x.MuCount, "Количство МО").Column("MU_COUNT");
            Property(x => x.UserName, "Способ формирования").Column("USER_NAME").Length(250);
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(250);
        }
    }
}
