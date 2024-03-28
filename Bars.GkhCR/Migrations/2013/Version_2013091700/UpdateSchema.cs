namespace Bars.GkhCr.Migrations.Version_2013091700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013090900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("CR_DICT_WORK_STAGE_WORK");
        }

        public override void Down()
        {
            Database.AddEntityTable("CR_DICT_WORK_STAGE_WORK",
                new RefColumn("STAGE_WORK_ID", ColumnProperty.NotNull, "CR_DICT_WORK_STAGE_STG", "CR_DICT_STAGE_WORK", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "CR_DICT_WORK_STAGE_WRK", "GKH_DICT_WORK", "ID"));
        }
    }
}