namespace Bars.Gkh.Ris.Migrations.Version_2016070800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016070800")]
    [MigrationDependsOn(typeof(Version_2016070401.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.ChangeColumn("RIS_METERING_DEVICE_DATA", new Column("COMMISSIONING_DATE", DbType.DateTime));

            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("VERIFICATION_INTERVAL_GUID", DbType.String));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("METERING_DEVICE_MODEL", DbType.String));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("FACTORY_SEAL_DATE", DbType.DateTime));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("TEMPERATURE_SENSOR", DbType.Boolean));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("PRESSURE_SENSOR", DbType.Boolean));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("TRANSFORMATION_RATIO", DbType.Decimal));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("VERIFICATION_DATE", DbType.DateTime));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("SEAL_DATE", DbType.DateTime));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("REASON_VERIFICATION_CODE", DbType.String));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("REASON_VERIFICATION_GUID", DbType.String));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("MANUAL_MODE_INFORMATION", DbType.String));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("TEMPERATURE_SENSOR_INFORMATION", DbType.String));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("PRESSURE_SENSOR_INFORMATION", DbType.String));
            //this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("REPLACING_METERING_DEVICE_VERSION_GUID", DbType.String));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "VERIFICATION_INTERVAL_GUID");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "METERING_DEVICE_MODEL");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "FACTORY_SEAL_DATE");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "TEMPERATURE_SENSOR");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "PRESSURE_SENSOR");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "TRANSFORMATION_RATIO");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "VERIFICATION_DATE");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "SEAL_DATE");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "REASON_VERIFICATION_CODE");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "REASON_VERIFICATION_GUID");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "MANUAL_MODE_INFORMATION");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "TEMPERATURE_SENSOR_INFORMATION");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "PRESSURE_SENSOR_INFORMATION");
            //this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "REPLACING_METERING_DEVICE_VERSION_GUID");
        }
    }
}