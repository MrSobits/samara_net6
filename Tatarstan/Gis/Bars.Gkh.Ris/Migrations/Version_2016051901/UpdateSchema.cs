namespace Bars.Gkh.Ris.Migrations.Version_2016051901
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016051901")]
    [MigrationDependsOn(typeof(Version_2016051900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //if (!this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "OPERATION"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFICATION_ATTACHMENT", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "EXTERNAL_ID"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFICATION_ATTACHMENT", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "EXTERNAL_SYSTEM_NAME"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFICATION_ATTACHMENT", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "RIS_CONTAINER_ID"))
            //{
            //    this.Database.AddRefColumn("RIS_NOTIFICATION_ATTACHMENT", new RefColumn("RIS_CONTAINER_ID", "RIS_NOTIFICATION_ATTACHMENT_CONTAINER", "RIS_CONTAINER", "ID"));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "RIS_CONTRAGENT_ID"))
            //{
            //    this.Database.AddRefColumn("RIS_NOTIFICATION_ATTACHMENT", new RefColumn("RIS_CONTRAGENT_ID", "RIS_NOTIFICATION_ATTACHMENT_CONTRAGENT", "RIS_CONTRAGENT", "ID"));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "GUID"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFICATION_ATTACHMENT", new Column("GUID", DbType.String, 50));
            //}
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //if (this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "OPERATION"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFICATION_ATTACHMENT", "OPERATION");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "EXTERNAL_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFICATION_ATTACHMENT", "EXTERNAL_ID");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "EXTERNAL_SYSTEM_NAME"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFICATION_ATTACHMENT", "EXTERNAL_SYSTEM_NAME");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "RIS_CONTAINER_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFICATION_ATTACHMENT", "RIS_CONTAINER_ID");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "RIS_CONTRAGENT_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFICATION_ATTACHMENT", "RIS_CONTRAGENT_ID");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFICATION_ATTACHMENT", "GUID"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFICATION_ATTACHMENT", "GUID");
            //}
        }
    }
}
