namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021701
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //исправление foreignkey
            Database.RemoveConstraint("GJI_ADMINCASE_PROVDOC", "FK_GJI_ADMINCASE_PROVDOC_PR");

            Database.AddForeignKey("FK_GJI_ADMINCASE_PROVDOC_PR", "GJI_ADMINCASE_PROVDOC", "PROVIDED_DOC_ID", "GJI_DICT_PROVIDEDDOCUMENT", "ID");

        }

        public override void Down()
        {
            
        }
    }
}