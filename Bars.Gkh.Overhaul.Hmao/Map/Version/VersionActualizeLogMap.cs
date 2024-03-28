/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.Version
/// {
///     using B4.DataAccess;
///     using Hmao.Entities.Version;
///     using Hmao.Enum;
/// 
///     public class VersionActualizeLogMap : BaseImportableEntityMap<VersionActualizeLog>
///     {
///         public VersionActualizeLogMap()
///             : base("OVRHL_ACTUALIZE_LOG")
///         {
///             Map(x => x.CountActions, "COUNT_ACTIONS");
///             Map(x => x.UserName, "USER_NAME");
///             Map(x => x.DateAction, "DATE_ACTION").Not.Nullable();
///             Map(x => x.ActualizeType, "TYPE_ACTUALIZE").Not.Nullable().CustomType<VersionActualizeType>();
/// 
///             References(x => x.ProgramVersion, "VERSION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.LogFile, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map.Version
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;

    /// <summary>
    /// Маппинг для <see cref="VersionActualizeLog"/>
    /// </summary>
    public class VersionActualizeLogMap : BaseImportableEntityMap<VersionActualizeLog>
    {
        /// <inheritdoc />
        public VersionActualizeLogMap()
            : base("Bars.Gkh.Overhaul.Hmao.Entities.Version.VersionActualizeLog", "OVRHL_ACTUALIZE_LOG")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.CountActions, "количество выполненных действий").Column("COUNT_ACTIONS");
            this.Property(x => x.ProgramCrName, "Имя краткосрочной программы, в рамках которой запускалось действие").Column("PROGRAM_CR_NAME");
            this.Property(x => x.UserName, "Наименвоание пользователя").Column("USER_NAME");
            this.Property(x => x.DateAction, "Дата выполнения действия").Column("DATE_ACTION").NotNull();
            this.Property(x => x.ActualizeType, "Тип актуализации").Column("TYPE_ACTUALIZE").NotNull();
            this.Property(x => x.InputParams, "Входные параметры").Column("INPUT_PARAMS").Length(1000);
            this.Reference(x => x.ProgramVersion, "Версия").Column("VERSION_ID").NotNull().Fetch();
            this.Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
            this.Reference(x => x.LogFile, "LogFile").Column("FILE_ID").Fetch();
        }
    }
}