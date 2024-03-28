namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2023121900
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023121900")]
    [MigrationDependsOn(typeof(Version_2023110700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("SMEV_STAGE", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_GMP", "SMEV_STAGE");
        }
    }
}