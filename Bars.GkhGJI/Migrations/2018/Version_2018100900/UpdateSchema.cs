namespace Bars.GkhGji.Migrations._2018.Version_2018100900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018100900")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2018.Version_2018100800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GJI_APPCIT_ANSWER", new RefColumn("FILE_DOC_INFO_ID", "GJI_ANSW_FI_DOC", "B4_FILE_INFO", "ID"));
            this.Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("ANSWER_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddEntityTable(
             "GJI_DICT_PROD_CALENDAR",
             new Column("Name", DbType.String, 500),
             new Column("PROD_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_PROD_CALENDAR");
            this.Database.RemoveColumn("GJI_APPEAL_CITIZENS", "ANSWER_DATE");
            this.Database.RemoveColumn("GJI_APPCIT_ANSWER", "FILE_DOC_INFO_ID");
        }

    }
}