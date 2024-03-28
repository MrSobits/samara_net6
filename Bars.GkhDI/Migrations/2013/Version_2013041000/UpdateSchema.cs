namespace Bars.GkhDi.Migrations.Version_2013041000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013040600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("DI_DICT_GROUP_WORK_PPR", new RefColumn("TEMPLATE_SERVICE_ID", "GROUP_WORK_PPR_TS", "DI_DICT_TEMPL_SERVICE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DICT_GROUP_WORK_PPR", "TEMPLATE_SERVICE_ID");
        }
    }
}