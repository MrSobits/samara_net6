namespace Bars.GisIntegration.RegOp.Migrations.Version_2016100700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016100700")]
    [MigrationDependsOn(typeof(Version_2016100601.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "BANK_NAME"))
            //{
            //    this.Database.ChangeColumn("RIS_NOTIFORDEREXECUT", new Column("BANK_NAME", DbType.String, 300));
            //}
            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_NAME"))
            //{
            //    this.Database.ChangeColumn("RIS_NOTIFORDEREXECUT", new Column("RECIPIENT_NAME", DbType.String, 300));
            //}
            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "SUPPLIER_NAME"))
            //{
            //    this.Database.ChangeColumn("RIS_NOTIFORDEREXECUT", new Column("SUPPLIER_NAME", DbType.String, 300));
            //}
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "BANK_NAME"))
            //{
            //    this.Database.ChangeColumn("RIS_NOTIFORDEREXECUT", new Column("BANK_NAME", DbType.String, 160));
            //}
            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_NAME"))
            //{
            //    this.Database.ChangeColumn("RIS_NOTIFORDEREXECUT", new Column("RECIPIENT_NAME", DbType.String, 160));
            //}
            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "SUPPLIER_NAME"))
            //{
            //    this.Database.ChangeColumn("RIS_NOTIFORDEREXECUT", new Column("SUPPLIER_NAME", DbType.String, 160));
            //}
        }
    }
}