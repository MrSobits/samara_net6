namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013092400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013092400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013092300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_RO_STRUCT_EL_IN_PRG_2", new Column("FOR_CORRECTION", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", new Column("FOR_CORRECTION", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", "FOR_CORRECTION");
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG_2", "FOR_CORRECTION");
        }
    }
}