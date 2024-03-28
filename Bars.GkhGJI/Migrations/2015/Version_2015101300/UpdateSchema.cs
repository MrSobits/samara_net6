namespace Bars.GkhGji.Migrations._2015.Version_2015101300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015101300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015100800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.TableExists("GJI_DICT_ANNEX_APPEAL_LIC_ISS"))
            {
                Database.RemoveTable("GJI_DICT_ANNEX_APPEAL_LIC_ISS");
            }
        }

        public override void Down()
        {
            
        }
    }
}