namespace Bars.GkhGji.Migrations._2023.Version_2023042500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023042500")]
    [MigrationDependsOn(typeof(_2023.Version_2023041300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_EMAIL",
                new Column("EMAIL_FROM", DbType.String),
                new Column("SENDER_INFO", DbType.String),
                new Column("LETTER_THEME", DbType.String),
                new Column("EMAIL_DATE", DbType.DateTime),
                new Column("GJI_NUMBER", DbType.String),
                new Column("EMAIL_TYPE", DbType.Int16, ColumnProperty.NotNull, 30),
                new Column("IS_REGISTRED", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("DESCRIPTION", DbType.String),
                new RefColumn("EMAIL_PDF_ID", ColumnProperty.None, "EMAIL_PDF_FILE_ID", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
                "GJI_EMAIL_ATTACHMENT",
                new RefColumn("EMAILGJI_ID", ColumnProperty.NotNull, "ATT_PARENT_EMAILGJI_ID", "GJI_EMAIL", "ID"),
                new RefColumn("ATT_FILE_ID", ColumnProperty.NotNull, "ATT_FILE_INFO_ID", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
                "GJI_EMAIL_LTEXT",
                new RefColumn("EMAILGJI_ID", ColumnProperty.NotNull, "PARENT_EMAILGJI_ID", "GJI_EMAIL", "ID"),
                new Column("CONTENT", DbType.Binary));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_EMAIL_LTEXT");
            Database.RemoveTable("GJI_EMAIL_ATTACHMENT");
            Database.RemoveTable("GJI_EMAIL");
        }
    }
}