namespace Bars.Gkh.Ris.Migrations.Version_2016082400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016082400")]
    [MigrationDependsOn(typeof(Version_2016052700.UpdateSchema))] // чтобы убрать хвост в текущих версиях
    [MigrationDependsOn(typeof(Version_2016081100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //this.Database.AddColumn("RIS_METERING_DEVICE_VERIFICATION_VALUE", new Column("SEAL_DATE", DbType.DateTime));
            //this.Database.AddColumn("RIS_METERING_DEVICE_VERIFICATION_VALUE", new Column("MUNICIPAL_RESOURCE_CODE", DbType.String));
            //this.Database.AddColumn("RIS_METERING_DEVICE_VERIFICATION_VALUE", new Column("MUNICIPAL_RESOURCE_GUID", DbType.String));

            //this.Database.AddColumn("RIS_METERING_DEVICE_CURRENT_VALUE", new Column("MUNICIPAL_RESOURCE_CODE", DbType.String));
            //this.Database.AddColumn("RIS_METERING_DEVICE_CURRENT_VALUE", new Column("MUNICIPAL_RESOURCE_GUID", DbType.String));

            //this.Database.AddColumn("RIS_METERING_DEVICE_CONTROL_VALUE", new Column("MUNICIPAL_RESOURCE_CODE", DbType.String));
            //this.Database.AddColumn("RIS_METERING_DEVICE_CONTROL_VALUE", new Column("MUNICIPAL_RESOURCE_GUID", DbType.String));
        }

        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_VERIFICATION_VALUE", "SEAL_DATE");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_VERIFICATION_VALUE", "MUNICIPAL_RESOURCE_CODE");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_VERIFICATION_VALUE", "MUNICIPAL_RESOURCE_GUID");

            //this.Database.RemoveColumn("RIS_METERING_DEVICE_CURRENT_VALUE", "MUNICIPAL_RESOURCE_CODE");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_CURRENT_VALUE", "MUNICIPAL_RESOURCE_GUID");

            //this.Database.RemoveColumn("RIS_METERING_DEVICE_CONTROL_VALUE", "MUNICIPAL_RESOURCE_CODE");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_CONTROL_VALUE", "MUNICIPAL_RESOURCE_GUID");
        }
    }
}