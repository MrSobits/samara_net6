namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013101400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013101100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесен в модуль overhaul
            //Database.AddRefColumn(
            //    "OVRHL_ACCOUNT",
            //    new RefColumn("LONG_TERM_OBJ_ID", ColumnProperty.NotNull, "OV_SPEC_ACC_LONG_TO", "OVRHL_LONGTERM_PR_OBJECT", "ID"));
        }

        public override void Down()
        {
            // Database.RemoveRefColumn("OVRHL_ACCOUNT", "LONG_TERM_OBJ_ID");
        }
    }
}