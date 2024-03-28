namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017110500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017110500")]
    [MigrationDependsOn(typeof(Version_2017110400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
           Database.AddEntityTable(
          "GJI_CH_GIS_GMP",
          new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
          new Column("BILL_DATE", DbType.DateTime, ColumnProperty.NotNull),
          new Column("BILL_FOR", DbType.String, ColumnProperty.NotNull),
          new Column("DOCUMENT_NUMBER", DbType.String, 500),
          new Column("DOCUMENT_SERIAL", DbType.String, 500),
          new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
          new Column("IDENT_TYPE", DbType.Int32, 4, ColumnProperty.None),
          new Column("PAYER_TYPE", DbType.Int32, 4, ColumnProperty.None),
          new Column("ANSWER", DbType.String, 500),
          new Column("ANSWER_GET", DbType.String, 500),
          new Column("INN", DbType.String, 20),
          new Column("CITIZENSHIP", DbType.Boolean),          
          new Column("KBK", DbType.String, 500),
          new Column("KIO", DbType.String, 20),
          new Column("KPP", DbType.String, 20),
          new Column("OKTMO", DbType.String, 20),
          new Column("PAYMENT_TYPE", DbType.String, 500),
          new Column("PURPOSE", DbType.String),
          new Column("STATE", DbType.String, 500),
          new Column("TAX_DATE", DbType.String),
          new Column("TAX_NUM", DbType.String),
          new Column("TAX_PERIOD", DbType.String),
          new Column("TOTAL_AMMOUNT", DbType.Decimal, ColumnProperty.NotNull),
          new Column("ALT_PAYER_IDENTIFIER", DbType.String, 25),
          new Column("GIS_GMP_PAYMENTS_KIND", DbType.Int32, 4, ColumnProperty.None),
          new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_GIS_GMP_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
          new RefColumn("FLDOCTYPE_ID", ColumnProperty.None, "GJI_CH_GIS_GMP_FLDOCTYPE_ID", "GJI_CH_DICT_FLDOC_TYPE", "ID"));

            Database.AddEntityTable(
       "GJI_CH_GIS_GMP_FILE",
       new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
       new RefColumn("GIS_GMP_ID", ColumnProperty.None, "GJI_CH_GIS_GMP_ID", "GJI_CH_GIS_GMP", "ID"),
       new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_GIS_GMP_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_GIS_GMP_FILE");
            Database.RemoveTable("GJI_CH_GIS_GMP");
        }
    }
}