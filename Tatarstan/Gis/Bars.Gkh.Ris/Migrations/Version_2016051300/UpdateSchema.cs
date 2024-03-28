namespace Bars.Gkh.Ris.Migrations.Version_2016051300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016051300")]
    [MigrationDependsOn(typeof(Version_2016050500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //if (!this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "ARCHIVING_REASON_CODE"))
            //{
            //    this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("ARCHIVING_REASON_CODE", DbType.String, 50));
            //}

            //if (!this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "ARCHIVING_REASON_GUID"))
            //{
            //    this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("ARCHIVING_REASON_GUID", DbType.String, 50));
            //}

            //if (!this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "PLANNED_VERIFICATION"))
            //{
            //    this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("PLANNED_VERIFICATION", DbType.Boolean));
            //}

            //if (!this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "ONE_RATE_DEVICE_VALUE"))
            //{
            //    this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("ONE_RATE_DEVICE_VALUE", DbType.Decimal));
            //}

            //if (!this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "ARCHIVATION"))
            //{
            //    this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("ARCHIVATION", DbType.Int16));
            //}

            //if (!this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "BASE_VALUE_T1"))
            //{
            //    this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("BASE_VALUE_T1", DbType.Decimal));
            //}

            //if (!this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "BASE_VALUE_T2"))
            //{
            //    this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("BASE_VALUE_T2", DbType.Decimal));
            //}

            //if (!this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "BASE_VALUE_T3"))
            //{
            //    this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("BASE_VALUE_T3", DbType.Decimal));
            //}

            //if (!this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "BEGIN_DATE"))
            //{
            //    this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("BEGIN_DATE", DbType.DateTime));
            //}

            //if (!this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "REPLACING_METERING_DEVICE_GUID"))
            //{
            //    this.Database.AddColumn("RIS_METERING_DEVICE_DATA", new Column("REPLACING_METERING_DEVICE_GUID", DbType.String));
            //}
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //if (this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "ARCHIVING_REASON_CODE"))
            //{
            //    this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "ARCHIVING_REASON_CODE");
            //}

            //if (this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "ARCHIVING_REASON_GUID"))
            //{
            //    this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "ARCHIVING_REASON_GUID");
            //}

            //if (this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "PLANNED_VERIFICATION"))
            //{
            //    this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "PLANNED_VERIFICATION");
            //}

            //if (this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "ONE_RATE_DEVICE_VALUE"))
            //{
            //    this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "ONE_RATE_DEVICE_VALUE");
            //}

            //if (this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "ARCHIVATION"))
            //{
            //    this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "ARCHIVATION");
            //}

            //if (this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "BASE_VALUE_T1"))
            //{
            //    this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "BASE_VALUE_T1");
            //}

            //if (this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "BASE_VALUE_T2"))
            //{
            //    this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "BASE_VALUE_T2");
            //}

            //if (this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "BASE_VALUE_T3"))
            //{
            //    this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "BASE_VALUE_T3");
            //}

            //if (this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "BEGIN_DATE"))
            //{
            //    this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "BEGIN_DATE");
            //}

            //if (this.Database.ColumnExists("RIS_METERING_DEVICE_DATA", "REPLACING_METERING_DEVICE_GUID"))
            //{
            //    this.Database.RemoveColumn("RIS_METERING_DEVICE_DATA", "REPLACING_METERING_DEVICE_GUID");
            //}
        }
    }
}
