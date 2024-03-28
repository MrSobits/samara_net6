namespace Bars.Gkh.Migrations._2016.Version_2016112500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;

    /// <summary>
    /// Миграция 2016112500
    /// </summary>
    [Migration("2016112500")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016111701.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_METERING_DEVICES_CHECKS", 
                new Column("CONTROL_READING", DbType.Int16, ColumnProperty.Null),
                new Column("REMOVAL_CONTROL_READING_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("START_DATE_CHECK", DbType.DateTime,ColumnProperty.Null),
                new Column("START_VALUE", DbType.Int16, ColumnProperty.Null),
                new Column("END_DATE_CHECK", DbType.DateTime, ColumnProperty.Null),
                new Column("END_VALUE", DbType.Int16, ColumnProperty.Null),
                new Column("MARK_METERING_DEVICE", DbType.String),
                new Column("INTERVAL_VERIFICATION", DbType.Int16, ColumnProperty.Null),
                new Column("NEXT_DATE_CHECK", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("METERING_DEVICE_ID", "GKH_METERING_DEVICES_CHECKS_GKH_OBJ_METERING_DEVICE", "GKH_OBJ_METERING_DEVICE", "ID"),
                new RefColumn("RO_ID", "GKH_METERING_DEVICES_CHECKS_GKH_REALITY_OBJECT", "GKH_REALITY_OBJECT", "ID"));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("GKH_METERING_DEVICES_CHECKS");
        }
    }
}