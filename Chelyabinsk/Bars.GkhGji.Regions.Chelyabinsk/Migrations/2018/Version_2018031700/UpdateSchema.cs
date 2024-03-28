namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2018031700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2018031700")]
    [MigrationDependsOn(typeof(Version_2017121600.UpdateSchema))]
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