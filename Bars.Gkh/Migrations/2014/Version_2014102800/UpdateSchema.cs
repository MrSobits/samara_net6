namespace Bars.Gkh.Migrations._2014.Version_2014102800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014102000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_ROOM", "CADASTRAL", DbType.String, 30, ColumnProperty.Null);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_ROOM", "CADASTRAL");
        }
    }
}