namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019070400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019070400")]
    [MigrationDependsOn(typeof(Version_2019070300.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("ALT_PAYER_IDENTIFIER_SENT", DbType.String, 25));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("BILL_DATE_SENT", DbType.DateTime));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("BILL_FOR_SENT", DbType.String, 255));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("KBK_SENT", DbType.String, 500));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("OKTMO_SENT", DbType.String, 20));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("LEGAL_ACT", DbType.String, 255));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("LEGAL_ACT_SENT", DbType.String, 255));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("PAYER_NAME", DbType.String, 255));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("PAYER_NAME_SENT", DbType.String, 255));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("PAYT_REASON_SENT", DbType.String, 2));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("STATE_SENT", DbType.String, 500));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("TAX_DATE_SENT", DbType.String, 255));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("TAX_NUM_SENT", DbType.String, 255));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("TAX_PERIOD_SENT", DbType.String, 255));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("TOTAL_AMMOUNT_SENT", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_GMP", "ALT_PAYER_IDENTIFIER_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "BILL_DATE_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "BILL_FOR_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "KBK_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "OKTMO_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "LEGAL_ACT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "LEGAL_ACT_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PAYER_NAME");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PAYER_NAME_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PAYT_REASON_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "STATE_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "TAX_DATE_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "TAX_NUM_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "TAX_PERIOD_SENT");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "TOTAL_AMMOUNT_SENT");
        }
    }
}


