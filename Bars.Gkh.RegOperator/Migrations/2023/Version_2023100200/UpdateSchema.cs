namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023100200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023100200")]

    [MigrationDependsOn(typeof(Version_2023082100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("EMAIL_NEWSLETTER",
                new Column("HEADER", DbType.String, 500),
                new Column("BODY", DbType.String, 1500),
                new Column("DESTINATIONS", DbType.String, 1000),
                new Column("SUCCESS", DbType.Boolean, ColumnProperty.NotNull, false),
                new RefColumn("ATTACHMENT_ID", "EMAIL_NEWSLETTER_ATTACHMENT_ID", "B4_FILE_INFO", "ID"),
                new Column("SENDER", DbType.String));

            Database.AddEntityTable("EMAIL_NEWSLETTER_LOG",
                new RefColumn("EMAIL_NEWSLETTER_ID", "EMAIL_NEWSLETTER_LOG_EMAIL_NEWSLETTER_ID", "EMAIL_NEWSLETTER", "ID"),
                new Column("DESTINATION", DbType.String),
                new Column("EMAIL_LOG", DbType.String, 1000),
                new Column("SUCCESS", DbType.Boolean));
        }
        public override void Down()
        {
            Database.RemoveTable("EMAIL_NEWSLETTER_LOG");
            Database.RemoveTable("EMAIL_NEWSLETTER");
        }
    }
}
