namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019022100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019022100")]
    [MigrationDependsOn(typeof(Version_2019021400.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
          "GJI_CH_PAY_REG_REQUESTS",
          new Column("MESSAGEID", DbType.String, 36, ColumnProperty.Null),
          new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_PAY_REG_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
          new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
          new Column("ANSWER", DbType.String, 500),
          new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
          new Column("PAY_REG_PAYMENTS_KIND", DbType.Int32, 4, ColumnProperty.None),
          new Column("PAY_REG_PAYMENTS_TYPE", DbType.Int32, 4, ColumnProperty.None, 1),
          new Column("PAY_START_DATE", DbType.DateTime, ColumnProperty.None),
          new Column("PAY_END_DATE", DbType.DateTime, ColumnProperty.None));

            Database.AddEntityTable(
          "GJI_CH_PAY_REG_FILE",
          new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
          new RefColumn("PAY_REG_REQUESTS_ID", ColumnProperty.None, "GJI_CH_PAY_REG_REQUESTS_ID", "GJI_CH_PAY_REG_REQUESTS", "ID"),
          new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_PAY_REG_FILE_INFO_ID", "B4_FILE_INFO", "ID"));


            Database.AddEntityTable(
                "GJI_CH_PAY_REG",
                new RefColumn("GIS_GMP_ID", ColumnProperty.Null, "GJI_CH_PAY_REG_GIS_GMP_ID", "GJI_CH_GIS_GMP", "ID"),
                new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("KBK", DbType.String),
                new Column("OKTMO", DbType.String),
                new Column("PAYMENT_DATE", DbType.DateTime, ColumnProperty.None),
                new Column("PAYMENT_ID", DbType.String),
                new Column("PURPOSE", DbType.String),
                new Column("UIN", DbType.String),
                new Column("PAYMENT_ORG", DbType.String),
                new Column("PAYMENT_ORG_DESCR", DbType.String),
                new Column("PAYER_ID", DbType.String),
                new Column("PAYER_ACCOUNT", DbType.String),
                new Column("PAYER_NAME", DbType.String),
                new Column("BDI_STATUS", DbType.String),
                new Column("BDI_PAYT_REASON", DbType.String),
                new Column("BDI_TAXPERIOD", DbType.String),
                new Column("BDI_TAXDOCNUMBER", DbType.String),
                new Column("BDI_TAXDOCDATE", DbType.String),
                new Column("ACCDOCDATE", DbType.DateTime),
                new Column("ACCDOCNO", DbType.String),
                new Column("RECONSILE", DbType.Int32, 4, ColumnProperty.NotNull, 20),
                new Column("STATUS", DbType.Byte));

            CopyGISGMPPaymentsToPayReg();

        }
        public override void Down()
        {
            Database.RemoveTable("GJI_CH_PAY_REG");
            Database.RemoveTable("GJI_CH_PAY_REG_FILE");
            Database.RemoveTable("GJI_CH_PAY_REG_REQUESTS");

        }
        private void CopyGISGMPPaymentsToPayReg()
        {
            var sql = @"INSERT INTO GJI_CH_PAY_REG
                (object_version, object_create_date, object_edit_date, amount, kbk, oktmo, payment_date, payment_id, purpose, uin, gis_gmp_id, reconsile)
                SELECT object_version, object_create_date, object_edit_date, amount, kbk, oktmo, payment_date, payment_id, prpose, uin, gis_gmp_id, reconsile
                FROM GJI_CH_GIS_GMP_PAYMENTS;";

            this.Database.ExecuteNonQuery(sql);

        }
    }

}