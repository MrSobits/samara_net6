namespace Bars.Gkh.Ris.Migrations.Version_2016062400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016062400")]
    [MigrationDependsOn(typeof(Version_2016061700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.RemoveColumn("RIS_CHARTER", "PERIODMETERING_STARTDATE");
            //this.Database.AddColumn("RIS_CHARTER", new Column("PERIOD_METERING_STARTDATE", DbType.Int32));

            //this.Database.RemoveColumn("RIS_CHARTER", "PERIODMETERING_ENDDATE");
            //this.Database.AddColumn("RIS_CHARTER", new Column("PERIOD_METERING_ENDDATE", DbType.Int32));

            //this.Database.RemoveColumn("RIS_CHARTER", "PERIODMETERING_LASTDAY");
            //this.Database.AddColumn("RIS_CHARTER", new Column("PERIOD_METERING_LASTDAY", DbType.Boolean));

            //this.Database.RemoveColumn("RIS_CHARTER", "PAYMENTDATE_STARTDATE");
            //this.Database.AddColumn("RIS_CHARTER", new Column("PAYMENT_DATE_STARTDATE", DbType.Int32));

            //this.Database.RemoveColumn("RIS_CHARTER", "PAYMENTDATE_LASTDAY");
            //this.Database.AddColumn("RIS_CHARTER", new Column("PAYMENT_DATE_LASTDAY", DbType.Boolean));

            //this.Database.RemoveColumn("RIS_CHARTER", "TERMINATECHARTER_DATE");
            //this.Database.AddColumn("RIS_CHARTER", new Column("TERMINATE_CHARTER_DATE", DbType.Boolean));

            //this.Database.RemoveColumn("RIS_CHARTER", "TERMINATECHARTER_REASON");
            //this.Database.AddColumn("RIS_CHARTER", new Column("TERMINATE_CHARTER_REASON", DbType.Boolean));

            //this.Database.ChangeColumn("RIS_CHARTER", new Column("MANAGERS", DbType.String));

            //this.Database.AddColumn("RIS_CHARTER", new Column("PERIOD_METERING_STARTDATE_THIS_MONTH", DbType.Boolean));
            //this.Database.AddColumn("RIS_CHARTER", new Column("PERIOD_METERING_ENDDATE_THIS_MONTH", DbType.Boolean));
            //this.Database.AddColumn("RIS_CHARTER", new Column("PAYMENT_SERV_DATE", DbType.Byte));
            //this.Database.AddColumn("RIS_CHARTER", new Column("PAYMENT_SERV_DATE_THIS_MONTH", DbType.Boolean));

            //if (!this.Database.ColumnExists("RIS_CONTRACTATTACHMENT", "ATTACHMENT_ID"))
            //{
            //    this.Database.AddColumn("RIS_CONTRACTATTACHMENT", new RefColumn("ATTACHMENT_ID", "RIS_CONTRATT_ATTACH", "RIS_ATTACHMENT", "ID"));
            //}
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_CHARTER", "PERIOD_METERING_STARTDATE_THIS_MONTH");
            //this.Database.RemoveColumn("RIS_CHARTER", "PERIOD_METERING_ENDDATE_THIS_MONTH");
            //this.Database.RemoveColumn("RIS_CHARTER", "PAYMENT_SERV_DATE");
            //this.Database.RemoveColumn("RIS_CHARTER", "PAYMENT_SERV_DATE_THIS_MONTH");

            //по остальным обратно не надо, там типы не соответствуют Entity
        }
    }
}
