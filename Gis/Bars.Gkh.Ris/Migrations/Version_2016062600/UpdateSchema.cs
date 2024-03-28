namespace Bars.Gkh.Ris.Migrations.Version_2016062600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016062600")]
    [MigrationDependsOn(typeof(Version_2016062400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("RIS_CONTRACT", new Column("PERIOD_METERING_STARTDATE_THIS_MONTH", DbType.Boolean));
            this.Database.AddColumn("RIS_CONTRACT", new Column("PERIOD_METERING_ENDDATE_THIS_MONTH", DbType.Boolean));
            this.Database.AddColumn("RIS_CONTRACT", new Column("PAYMENT_SERV_DATE", DbType.Byte));
            this.Database.AddColumn("RIS_CONTRACT", new Column("PAYMENT_SERV_DATE_THIS_MONTH", DbType.Boolean));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("RIS_CONTRACT", "PERIOD_METERING_STARTDATE_THIS_MONTH");
            this.Database.RemoveColumn("RIS_CONTRACT", "PERIOD_METERING_ENDDATE_THIS_MONTH");
            this.Database.RemoveColumn("RIS_CONTRACT", "PAYMENT_SERV_DATE");
            this.Database.RemoveColumn("RIS_CONTRACT", "PAYMENT_SERV_DATE_THIS_MONTH");
        }
    }
}
