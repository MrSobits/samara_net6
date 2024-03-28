namespace Bars.Gkh.Migrations._2016.Version_2016111700
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016111700
    /// </summary>
    [Migration("2016111700")]
    [MigrationDependsOn(typeof(Version_2016110300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_OBJ_METERING_DEVICE", new Column("SERIAL_NUMBER", DbType.String, 100));
            this.Database.AddColumn("GKH_OBJ_METERING_DEVICE", new Column("READINGS_MANUALLY", DbType.Boolean));
            this.Database.AddColumn("GKH_OBJ_METERING_DEVICE", new Column("NECESSITY_VERIFICATION", DbType.Boolean));
            this.Database.AddColumn("GKH_OBJ_METERING_DEVICE", new Column("ACCOUNT_NUMBER", DbType.String, 100));
            this.Database.AddColumn("GKH_OBJ_METERING_DEVICE", new Column("DATE_FIRST_VERIFICATION", DbType.DateTime));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_OBJ_METERING_DEVICE", "SERIAL_NUMBER");
            this.Database.RemoveColumn("GKH_OBJ_METERING_DEVICE", "READINGS_MANUALLY");
            this.Database.RemoveColumn("GKH_OBJ_METERING_DEVICE", "NECESSITY_VERIFICATION");
            this.Database.RemoveColumn("GKH_OBJ_METERING_DEVICE", "ACCOUNT_NUMBER");
            this.Database.RemoveColumn("GKH_OBJ_METERING_DEVICE", "DATE_FIRST_VERIFICATION");
        }
    }
}
