namespace Bars.GkhGji.Regions.Habarovsk.Migrations.Version_2022031000
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022031000")]
    [MigrationDependsOn(typeof(Version_2022030400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_VR_COURT_PRACTICE", new RefColumn("REQUEST_ID", ColumnProperty.None, "GJI_VR_COURT_PRACTICE_LIC_STMNT", "GJI_MKD_LIC_STATEMENT", "ID"));


        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_VR_COURT_PRACTICE", "REQUEST_ID");
        }
    }
}