namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014102200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014102100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_ACTUALIZE_LOG",
                new RefColumn("VERSION_ID", ColumnProperty.NotNull, "OVRHL_ACTUALIZE_LOG_PV", "OVRHL_PRG_VERSION", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "OVRHL_ACTUALIZE_LOG_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "OVRHL_ACTUALIZE_LOG_F", "B4_FILE_INFO", "ID"),
                new Column("TYPE_ACTUALIZE", DbType.Int16, ColumnProperty.NotNull, 10),
                new Column("DATE_ACTION", DbType.DateTime, ColumnProperty.NotNull),
                new Column("USER_NAME", DbType.String, 500),
                new Column("COUNT_ACTIONS", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_ACTUALIZE_LOG");
        }
    }
}