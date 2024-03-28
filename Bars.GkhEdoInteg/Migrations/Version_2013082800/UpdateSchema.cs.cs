namespace Bars.GkhEdoInteg.Migrations.Version_2013082800
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013082800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhEdoInteg.Migrations.Version_2013081300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "INTGEDO_LOGR",
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime, ColumnProperty.NotNull),
                new Column("COUNT", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("COUNT_ADDED", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("COUNT_UPDATED", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("URI", DbType.String, 250, ColumnProperty.NotNull),
                new RefColumn("FILE_ID", "INTGEDO_LOG_FILE", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
                "INTGEDO_LOGR_APPCITS",
                new Column("DATE_ACTUAL", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ACTION_IMPORT_ROW", DbType.Int64, 22, ColumnProperty.NotNull, 10),
                new RefColumn("LOGR_ID", ColumnProperty.NotNull, "INTGEDO_LOGAPP_LOG", "INTGEDO_LOGR", "ID"),
                new RefColumn("APPCITS_COMPEDO_ID", ColumnProperty.NotNull, "INTGEDO_LOGAPP_APP", "INTGEDO_APPCITS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("INTGEDO_LOGR_APPCITS");
            Database.RemoveTable("INTGEDO_LOGR");
        }
    }
}