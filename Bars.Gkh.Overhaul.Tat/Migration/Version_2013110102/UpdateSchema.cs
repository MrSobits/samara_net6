namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013110102
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110102")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013110101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RenameColumn("OVRHL_CURR_PRIORITY", "WEIGHT", "C_ORDER");

            Database.AddColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", new Column("CRITERIA", DbType.String, 2000, ColumnProperty.Null));

            Database.RemoveColumn("OVRHL_PRG_VERSION", "MU_ID");
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", "CRITERIA");

            Database.RenameColumn("OVRHL_CURR_PRIORITY", "C_ORDER", "WEIGHT");
        }
    }
}