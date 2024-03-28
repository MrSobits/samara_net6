namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2023111300
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023111300")]
    [MigrationDependsOn(typeof(Version_2023110800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("REGISTRED", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("SENDED", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddRefColumn("GJI_APPCIT_ANSWER", new RefColumn("SIGN_FILE", "GJI_APPCIT_ANSWER_SIGN_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "SIGN_FILE");
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "SENDED");
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "REGISTRED");
        }
    }
}