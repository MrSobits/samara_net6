namespace Bars.GkhCr.Migrations.Version_2013120600
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013100800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("CR_OBJ_PERFOMED_WORK_ACT", new RefColumn("COST_FILE_ID","CR_ACT_COST_FILE_ID","B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "COST_FILE_ID");
        }
    }
}