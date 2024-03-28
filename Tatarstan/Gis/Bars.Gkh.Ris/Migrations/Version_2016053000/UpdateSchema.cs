namespace Bars.Gkh.Ris.Migrations.Version_2016053000
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    [Migration("2016053000")]
    [MigrationDependsOn(typeof(Version_2016052400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //if (!this.Database.ColumnExists("RIS_PAYMENT_INFO", "RECIPIENT_INN"))
            //{
            //    this.Database.AddColumn("RIS_PAYMENT_INFO", "RECIPIENT_INN", DbType.String, 12);
            //}

            //if (!this.Database.ColumnExists("RIS_PAYMENT_INFO", "BANK_NAME"))
            //{
            //    this.Database.AddColumn("RIS_PAYMENT_INFO", "BANK_NAME", DbType.String, 160);
            //}

            //if (!this.Database.ColumnExists("RIS_PAYMENT_INFO", "RECIPIENT_INN"))
            //{
            //    this.Database.AddColumn("RIS_PAYMENT_INFO", "RECIPIENT_INN", DbType.String, 30);
            //}
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_PAYMENT_INFO", "RECIPIENT_INN");
            //this.Database.RemoveColumn("RIS_PAYMENT_INFO", "BANK_NAME");
            //this.Database.RemoveColumn("RIS_PAYMENT_DOC", "PAYMENT_DOC_NUM");
        }
    }
}