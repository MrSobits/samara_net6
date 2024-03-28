namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2023110700
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023110700")]
    [MigrationDependsOn(typeof(Version_2023092100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_CH_APPCIT_ANSWER_LTEXT",
            new RefColumn("APPCIT_ANSWER_ID", ColumnProperty.NotNull, "GJI_CH_APPCIT_ANSWER_LT_ANSWER", "GJI_APPCIT_ANSWER", "ID"),
            new Column("DESCRIPTION", DbType.Binary),
            new Column("DESCRIPTION2", DbType.Binary));

            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("REGISTRED", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("SENDED", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddRefColumn("GJI_APPCIT_ANSWER", new RefColumn("SIGN_FILE", "GJI_APPCIT_ANSWER_SIGN_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "SIGN_FILE");
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "SENDED");
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "REGISTRED");

            Database.RemoveTable("GJI_CH_APPCIT_ANSWER_LTEXT");
        }
    }
}