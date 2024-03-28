namespace Bars.Gkh.Ris.Migrations.Version_2016052300
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016052300")]
    [MigrationDependsOn(typeof(Version_2016052000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //if (this.Database.TableExists("RIS_SOURCE_OKI"))
            //{
            //    if (!this.Database.ColumnExists("RIS_SOURCE_OKI", "SOURCE_RKIITEM_ID"))
            //    {
            //        this.Database.AddRefColumn("RIS_SOURCE_OKI", new RefColumn("SOURCE_RKIITEM_ID", "RIS_SOURCE_OKI_SOURCE_RKIITEM_ID", "RIS_RKI_ITEM", "ID"));
            //    }
            //    if (this.Database.ColumnExists("RIS_SOURCE_OKI", "SOURCEOKI"))
            //    {
            //        this.Database.RemoveColumn("RIS_SOURCE_OKI", "SOURCEOKI");
            //    }
            //}

            //if (this.Database.TableExists("RIS_RECEIVER_OKI"))
            //{
            //    if (!this.Database.ColumnExists("RIS_RECEIVER_OKI", "RECEIVER_RKIITEM_ID"))
            //    {
            //        this.Database.AddRefColumn("RIS_RECEIVER_OKI", new RefColumn("RECEIVER_RKIITEM_ID", "RIS_RECEIVER_OKI_RECEIVER_RKIITEM_ID", "RIS_RKI_ITEM", "ID"));
            //    }
            //    if (this.Database.ColumnExists("RIS_RECEIVER_OKI", "RECEIVEROKI"))
            //    {
            //        this.Database.RemoveColumn("RIS_RECEIVER_OKI", "RECEIVEROKI");
            //    }
            //}

            //if (!this.Database.TableExists("RIS_RKI_COMMUNAL_SERVICE"))
            //{
            //    this.Database.AddRisEntityTable("RIS_RKI_COMMUNAL_SERVICE", new[]
            //    {
            //        new RefColumn("RKIITEM_ID", "RIS_RKI_COMMUNAL_SERVICE_RKIITEM_ID", "RIS_RKI_ITEM", "ID"),
            //        new Column("SERVICE_CODE", DbType.String.WithSize(20)),
            //        new Column("SERVICE_GUID", DbType.String.WithSize(36)),
            //        new Column("SERVICE_NAME", DbType.String.WithSize(1200)),
            //    });
            //}

            //if (!this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "OPERATION"))
            //{
            //    this.Database.AddColumn("RIS_RKI_ATTACHMENT", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            //}
            //if (!this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "EXTERNAL_ID"))
            //{
            //    this.Database.AddColumn("RIS_RKI_ATTACHMENT", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull));
            //}
            //if (!this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "EXTERNAL_SYSTEM_NAME"))
            //{
            //    this.Database.AddColumn("RIS_RKI_ATTACHMENT", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            //}
            //if (!this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "RIS_CONTAINER_ID"))
            //{
            //    this.Database.AddRefColumn("RIS_RKI_ATTACHMENT", new RefColumn("RIS_CONTAINER_ID", "RIS_NOTIFICATION_ATTACHMENT_CONTAINER", "RIS_CONTAINER", "ID"));
            //}
            //if (!this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "RIS_CONTRAGENT_ID"))
            //{
            //    this.Database.AddRefColumn("RIS_RKI_ATTACHMENT", new RefColumn("RIS_CONTRAGENT_ID", "RIS_NOTIFICATION_ATTACHMENT_CONTRAGENT", "RIS_CONTRAGENT", "ID"));
            //}
            //if (!this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "GUID"))
            //{
            //    this.Database.AddColumn("RIS_RKI_ATTACHMENT", new Column("GUID", DbType.String, 50));
            //}

            //if (this.Database.ColumnExists("RIS_RKI_ITEM", "HOUSE_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_RKI_ITEM", "HOUSE_ID");
            //}
            //if (!this.Database.ColumnExists("RIS_RKI_ITEM", "FIAS_ADDRESS_ID"))
            //{
            //    this.Database.AddColumn("RIS_RKI_ITEM", new RefColumn("FIAS_ADDRESS_ID", "RIS_RKI_ITEM_FIAS_ADDRESS_ID", "B4_FIAS_ADDRESS", "ID"));
            //}
            //if (!this.Database.ColumnExists("RIS_RKI_ITEM", "COMMISSIONING_YEAR"))
            //{
            //    this.Database.AddColumn("RIS_RKI_ITEM", new Column("COMMISSIONING_YEAR", DbType.Int16));
            //}

            //if (!this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "OPERATION"))
            //{
            //    this.Database.AddColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"));
            //}
            //if (!this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "EXTERNAL_ID"))
            //{
            //    this.Database.AddColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull));
            //}
            //if (!this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "EXTERNAL_SYSTEM_NAME"))
            //{
            //    this.Database.AddColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"));
            //}
            //if (!this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "RIS_CONTAINER_ID"))
            //{
            //    this.Database.AddRefColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", new RefColumn("RIS_CONTAINER_ID", "RIS_NOTIFICATION_ATTACHMENT_CONTAINER_EE", "RIS_CONTAINER", "ID"));
            //}
            //if (!this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "RIS_CONTRAGENT_ID"))
            //{
            //    this.Database.AddRefColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", new RefColumn("RIS_CONTRAGENT_ID", "RIS_NOTIFICATION_ATTACHMENT_CONTRAGENT", "RIS_CONTRAGENT", "ID"));
            //}
            //if (!this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "GUID"))
            //{
            //    this.Database.AddColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", new Column("GUID", DbType.String, 50));
            //}
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //if (this.Database.TableExists("RIS_SOURCE_OKI"))
            //{
            //    if (this.Database.ColumnExists("RIS_SOURCE_OKI", "SOURCE_RKIITEM_ID"))
            //    {
            //        this.Database.RemoveColumn("RIS_SOURCE_OKI", "SOURCE_RKIITEM_ID");
            //    }
            //    if (!this.Database.ColumnExists("RIS_SOURCE_OKI", "SOURCEOKI"))
            //    {
            //        this.Database.AddColumn("RIS_SOURCE_OKI", new Column("SOURCEOKI", DbType.String.WithSize(200)));
            //    }
            //}

            //if (this.Database.TableExists("RIS_RECEIVER_OKI"))
            //{
            //    if (this.Database.ColumnExists("RIS_RECEIVER_OKI", "RECEIVER_RKIITEM_ID"))
            //    {
            //        this.Database.RemoveColumn("RIS_RECEIVER_OKI", "RECEIVER_RKIITEM_ID");
            //    }
            //    if (!this.Database.ColumnExists("RIS_RECEIVER_OKI", "RECEIVEROKI"))
            //    {
            //        this.Database.AddColumn("RIS_RECEIVER_OKI", new Column("RECEIVEROKI", DbType.String.WithSize(200)));
            //    }
            //}

            //if (this.Database.TableExists("RIS_RKI_COMMUNAL_SERVICE"))
            //{
            //    this.Database.RemoveTable("RIS_RKI_COMMUNAL_SERVICE");
            //}

            //if (this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "OPERATION"))
            //{
            //    this.Database.RemoveColumn("RIS_RKI_ATTACHMENT", "OPERATION");
            //}
            //if (this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "EXTERNAL_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_RKI_ATTACHMENT", "EXTERNAL_ID");
            //}
            //if (this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "EXTERNAL_SYSTEM_NAME"))
            //{
            //    this.Database.RemoveColumn("RIS_RKI_ATTACHMENT", "EXTERNAL_SYSTEM_NAME");
            //}
            //if (this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "RIS_CONTAINER_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_RKI_ATTACHMENT", "RIS_CONTAINER_ID");
            //}
            //if (this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "RIS_CONTRAGENT_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_RKI_ATTACHMENT", "RIS_CONTRAGENT_ID");
            //}
            //if (this.Database.ColumnExists("RIS_RKI_ATTACHMENT", "GUID"))
            //{
            //    this.Database.RemoveColumn("RIS_RKI_ATTACHMENT", "GUID");
            //}

            //if (!this.Database.ColumnExists("RIS_RKI_ITEM", "HOUSE_ID"))
            //{
            //    this.Database.AddColumn("RIS_RKI_ITEM", new RefColumn("HOUSE_ID", "RIS_RKI_ITEM_HOUSE_ID", "RIS_HOUSE", "ID"));
            //}
            //if (this.Database.ColumnExists("RIS_RKI_ITEM", "FIAS_ADDRESS_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_RKI_ITEM", "FIAS_ADDRESS_ID");
            //}
            //if (this.Database.ColumnExists("RIS_RKI_ITEM", "COMMISSIONING_YEAR"))
            //{
            //    this.Database.RemoveColumn("RIS_RKI_ITEM", "COMMISSIONING_YEAR");
            //}
            
            //if (this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "OPERATION"))
            //{
            //    this.Database.RemoveColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "OPERATION");
            //}
            //if (this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "EXTERNAL_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "EXTERNAL_ID");
            //}
            //if (this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "EXTERNAL_SYSTEM_NAME"))
            //{
            //    this.Database.RemoveColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "EXTERNAL_SYSTEM_NAME");
            //}
            //if (this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "RIS_CONTAINER_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "RIS_CONTAINER_ID");
            //}
            //if (this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "RIS_CONTRAGENT_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "RIS_CONTRAGENT_ID");
            //}
            //if (this.Database.ColumnExists("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "GUID"))
            //{
            //    this.Database.RemoveColumn("RIS_ATTACHMENTS_ENERGYEFFICIENCY", "GUID");
            //}
        }
    }
}