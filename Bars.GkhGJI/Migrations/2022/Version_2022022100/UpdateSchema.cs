namespace Bars.GkhGji.Migrations._2022.Version_2022022100
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022022100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022012000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_SPECIAL_ACCOUNT_REPORT",
                new Column("EXECUTOR", DbType.String, 50),
                new Column("MONTH", DbType.Int16, 3, ColumnProperty.NotNull),
                new Column("YEAR", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("SERTIFICATE", DbType.String, 5000),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("CREDIT_ORG_ID", DbType.Int64, 22),
                new Column("AUTHOR", DbType.String, 500),
                new Column("AMMOUNT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("FILE_ID", DbType.Int64, 22));

            Database.AddForeignKey("FK_GJI_SAR_CONTR", "GJI_SPECIAL_ACCOUNT_REPORT", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GJI_SAR_CREDIT_ORG", "GJI_SPECIAL_ACCOUNT_REPORT", "CREDIT_ORG_ID", "OVRHL_CREDIT_ORG", "ID");
            Database.AddForeignKey("FK_GJI_SAR_FILE", "GJI_SPECIAL_ACCOUNT_REPORT", "FILE_ID", "B4_FILE_INFO", "ID");

            Database.AddEntityTable(
                "GJI_SPECIAL_ACCOUNT_ROW",
                new Column("AMMOUNT_DEBT", DbType.Decimal),
                new Column("BALLANCE", DbType.Decimal),
                new Column("INCOMING", DbType.Decimal),
                new Column("SPECIAL_ACCOUNT_NUM", DbType.String, 30),
                new Column("TRANSFER", DbType.Decimal),
                new Column("MO_ID", DbType.Int64, 22),
                new Column("RO_ID", DbType.Int64, 22),
                new Column("REPORT_ID", DbType.Int64, 22));

            Database.AddForeignKey("FK_GJI_SA_ROW_MUN", "GJI_SPECIAL_ACCOUNT_ROW", "MO_ID", "GKH_DICT_MUNICIPALITY", "ID");
            Database.AddForeignKey("FK_GJI_SA_ROW_RO", "GJI_SPECIAL_ACCOUNT_ROW", "RO_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GJI_SA_ROW_REPORT", "GJI_SPECIAL_ACCOUNT_ROW", "REPORT_ID", "GJI_SPECIAL_ACCOUNT_REPORT", "ID");

            Database.AddColumn("GJI_SPECIAL_ACCOUNT_ROW", new Column("ACCURACY_AREA", DbType.Decimal, ColumnProperty.None, 0));
            Database.AddColumn("GJI_SPECIAL_ACCOUNT_ROW", new Column("START_DATE", DbType.DateTime));
            Database.AddColumn("GJI_SPECIAL_ACCOUNT_ROW", new Column("CONTRACTS", DbType.String));
            Database.AddColumn("GJI_SPECIAL_ACCOUNT_ROW", new Column("TRANSFER_TOTAL", DbType.Decimal, ColumnProperty.None, 0));
            Database.AddColumn("GJI_SPECIAL_ACCOUNT_ROW", new Column("ACCURED_TOTAL", DbType.Decimal, ColumnProperty.None, 0));
            Database.AddColumn("GJI_SPECIAL_ACCOUNT_ROW", new Column("INCOMING_TOTAL", DbType.Decimal, ColumnProperty.None, 0));
            Database.AddColumn("GJI_SPECIAL_ACCOUNT_ROW", new Column("ACCURED", DbType.Decimal, ColumnProperty.None, 0));
            Database.AddColumn("GJI_SPECIAL_ACCOUNT_REPORT", new Column("DATE_ACCEPT", DbType.DateTime));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
	    public override void Down()
        {
            Database.RemoveTable("GJI_SPECIAL_ACCOUNT_ROW");
            Database.RemoveTable("GJI_SPECIAL_ACCOUNT_REPORT");
        }
    }
}