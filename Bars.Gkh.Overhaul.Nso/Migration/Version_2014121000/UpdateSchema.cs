using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014121000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014120300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC", new Column("CHANGES", DbType.String, 50));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_ADD_ACTUALIZE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_CH_YEAR_ACTUALIZE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_CH_SUM_ACTUALIZE", DbType.Boolean, ColumnProperty.NotNull, false));

            Database.AddEntityTable("OVRHL_ACTUALIZE_LOG",
                new RefColumn("VERSION_ID", ColumnProperty.NotNull, "OVRHL_ACTUALIZE_LOG_PV", "OVRHL_PRG_VERSION", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "OVRHL_ACTUALIZE_LOG_F", "B4_FILE_INFO", "ID"),
                new Column("TYPE_ACTUALIZE", DbType.Int16, ColumnProperty.NotNull, 10),
                new Column("DATE_ACTION", DbType.DateTime, ColumnProperty.NotNull),
                new Column("USER_NAME", DbType.String, 500),
                new Column("COUNT_ACTIONS", DbType.Int32));

        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "CHANGES");
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_ADD_ACTUALIZE");
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_CH_YEAR_ACTUALIZE");
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_CH_SUM_ACTUALIZE");

            Database.RemoveTable("OVRHL_ACTUALIZE_LOG");
        }
    }
}