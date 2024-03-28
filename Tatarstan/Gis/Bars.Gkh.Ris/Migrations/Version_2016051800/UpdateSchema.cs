namespace Bars.Gkh.Ris.Migrations.Version_2016051800
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016051800")]
    [MigrationDependsOn(typeof(Version_2016051701.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //this.Database.AddEntityTable(
            //    "RIS_ADDAGREEMENTPAYMENT", 
            //    new Column("AGREEMENTPAYMENTVERSION", DbType.String, 50, ColumnProperty.NotNull), 
            //    new Column("DATEFROM", DbType.DateTime, ColumnProperty.NotNull), 
            //    new Column("DATETO", DbType.DateTime, ColumnProperty.NotNull), 
            //    new Column("BILL", DbType.Decimal),
            //    new Column("DEBT", DbType.Decimal),
            //    new Column("PAID", DbType.Decimal));

            //var tableName = "RIS_TRUSTDOCATTACHMENT";
            //if (this.Database.TableExists(tableName))
            //{
            //    if (!this.Database.ColumnExists(tableName, "OPERATION"))
            //    {
            //        this.Database.AddColumn(tableName, "OPERATION", DbType.Int16, ColumnProperty.NotNull, "0");
            //    }

            //    if (!this.Database.ColumnExists(tableName, "EXTERNAL_ID"))
            //    {
            //        this.Database.AddColumn(tableName, "EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull);
            //    }

            //    if (!this.Database.ColumnExists(tableName, "EXTERNAL_SYSTEM_NAME"))
            //    {
            //        this.Database.AddColumn(tableName, "EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'");
            //    }

            //    if (!this.Database.ColumnExists(tableName, "RIS_CONTAINER_ID"))
            //    {
            //        this.Database.AddRefColumn(tableName, new RefColumn("RIS_CONTAINER_ID", tableName + "_CONTAINER", "RIS_CONTAINER", "ID"));
            //    }

            //    if (!this.Database.ColumnExists(tableName, "RIS_CONTRAGENT_ID"))
            //    {
            //        this.Database.AddRefColumn(tableName, new RefColumn("RIS_CONTRAGENT_ID", tableName + "_CONTRAGENT", "RIS_CONTRAGENT", "ID"));
            //    }

            //    if (!this.Database.ColumnExists(tableName, "GUID"))
            //    {
            //        this.Database.AddColumn(tableName, "GUID", DbType.String, 50);
            //    }

            //    this.Database.AddColumn(tableName, "EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'");
            //    this.Database.AddRefColumn(tableName, new RefColumn("ADDAGREEMENTPAYMENT_ID", tableName + "_ADDAGREEMENTPAYMENT", "RIS_ADDAGREEMENTPAYMENT", "ID"));
            //}
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //var tableName = "RIS_TRUSTDOCATTACHMENT";
            //if (this.Database.TableExists(tableName))
            //{
            //    this.Database.RemoveColumn(tableName, "ADDAGREEMENTPAYMENT_ID");
            //    this.Database.RemoveColumn(tableName, "GUID");
            //    this.Database.RemoveColumn(tableName, "RIS_CONTRAGENT_ID");
            //    this.Database.RemoveColumn(tableName, "RIS_CONTAINER_ID");
            //    this.Database.RemoveColumn(tableName, "EXTERNAL_SYSTEM_NAME");
            //    this.Database.RemoveColumn(tableName, "EXTERNAL_ID");
            //    this.Database.RemoveColumn(tableName, "OPERATION");
            //}
            //this.Database.RemoveTable("RIS_ADDAGREEMENTPAYMENT");
        }
    }
}