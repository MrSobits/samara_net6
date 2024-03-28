namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2022111400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2022111400")]
    [MigrationDependsOn(typeof(Version_2022110200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_VDGO_VIOLATORS", new Column("MARK_OF_MESSAGE", DbType.Boolean,false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_VDGO_VIOLATORS", "MARK_OF_MESSAGE");
        }
    }
}