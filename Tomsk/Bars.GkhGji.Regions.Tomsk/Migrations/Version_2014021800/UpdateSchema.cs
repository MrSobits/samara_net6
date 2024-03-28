namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOL_PROS_DEFINITION", "ADDITIONAL_DOCUMENTS", DbType.String, 255);
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOL_PROS_DEFINITION", "ADDITIONAL_DOCUMENTS");
        }
    }
}