namespace Bars.Gkh.Migrations.Version_2013073000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013073000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OBJ_BASE_WRK", new Column("EXTERNAL_ID", DbType.String, 36));

            Database.RemoveColumn("GKH_OBJ_BASE_WRK", "DATE_LAST_REPAIR");
            Database.AddColumn("GKH_OBJ_BASE_WRK", new Column("YEAR_LAST_REPAIR", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_BASE_WRK", "EXTERNAL_ID");

            Database.RemoveColumn("GKH_OBJ_BASE_WRK", "YEAR_LAST_REPAIR");
            Database.AddColumn("GKH_OBJ_BASE_WRK", new Column("DATE_LAST_REPAIR", DbType.Date));
        }
    }
}