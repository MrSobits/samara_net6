namespace Bars.GkhGji.Migrations._2021.Version_2021052800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021052800")]
    [MigrationDependsOn(typeof(Version_2021040900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("ADDRESS", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "ADDRESS");
        }
    }
}