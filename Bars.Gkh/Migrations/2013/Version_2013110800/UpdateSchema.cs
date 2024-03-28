namespace Bars.Gkh.Migrations.Version_2013110800
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013110700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OBJ_METERING_DEVICE", new Column("DATE_INSTALLATION", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_METERING_DEVICE", "DATE_INSTALLATION");
        }
    }
}