namespace Bars.Gkh.Ris.Migrations.Version_2016053100
{
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016.05.31.00
    /// </summary>
    [Migration("2016053100")]
    [MigrationDependsOn(typeof(Version_2016053001.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //if (!this.Database.ColumnExists("RIS_ACKNOWLEDGMENT", "NOTIFY_ID"))
            //{
            //    this.Database.AddRefColumn("RIS_ACKNOWLEDGMENT", new RefColumn("NOTIFY_ID", "FK_NOTIFY_ID", "RIS_NOTIFORDEREXECUT", "ID"));
            //}

            //if (!this.Database.ColumnExists("RIS_ACKNOWLEDGMENT", "PAY_DOC_ID"))
            //{
            //    this.Database.AddRefColumn("RIS_ACKNOWLEDGMENT", new RefColumn("PAY_DOC_ID", "FK_PAY_DOC_ID", "RIS_PAYMENT_DOC", "ID"));
            //}
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //if (this.Database.ColumnExists("RIS_ACKNOWLEDGMENT", "NOTIFY_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_ACKNOWLEDGMENT", "NOTIFY_ID");
            //}

            //if (this.Database.ColumnExists("RIS_ACKNOWLEDGMENT", "PAY_DOC_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_ACKNOWLEDGMENT", "PAY_DOC_ID");
            //}
        }
    }
}