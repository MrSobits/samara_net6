namespace Bars.GkhGji.Migrations._2020.Version_2020062300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020062300")]
    [MigrationDependsOn(typeof(Version_2020052600.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("APROOVE_NUMBER", DbType.String, 36));
            Database.AddColumn("GJI_DISPOSAL", new Column("APROOVE_DATE", DbType.DateTime));
            Database.AddRefColumn("GJI_DISPOSAL", new RefColumn("APROOVE_FILE_ID", "GJI_DISPOSAL_APPROVE_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL", "APROOVE_FILE_ID");
            Database.RemoveColumn("GJI_DISPOSAL", "APROOVE_DATE");
            Database.RemoveColumn("GJI_DISPOSAL", "APROOVE_NUMBER");
        }
    }
}