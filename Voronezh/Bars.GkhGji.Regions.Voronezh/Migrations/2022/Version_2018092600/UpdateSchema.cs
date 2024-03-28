namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2018092600
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2018092600")]
    [MigrationDependsOn(typeof(Version_2017112200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {

            Database.AddEntityTable(
             "GJI_EMAIL_LIST",
             new Column("ANSWER_NUMBER", DbType.String, 50),
             new Column("APPEAL_DATE", DbType.DateTime, ColumnProperty.NotNull),
             new Column("APPEAL_NUMBER", DbType.String, 20),
             new Column("MAIL_TO", DbType.String, 50),
             new Column("SEND_DATE", DbType.DateTime, ColumnProperty.NotNull),
             new Column("TITLE", DbType.String, 50),
             new RefColumn("FILE_INFO_ID", ColumnProperty.None, "GJI_EMAIL_LIST_APPEAL_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_EMAIL_LIST");
        }
    }
}