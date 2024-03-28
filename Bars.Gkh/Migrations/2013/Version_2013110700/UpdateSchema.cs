namespace Bars.Gkh.Migrations.Version_2013110700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013110600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_METERING_DEVICE", new Column("DEVICE_GROUP", DbType.Int16));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_METERING_DEVICE", "DEVICE_GROUP");
        }
    }
}