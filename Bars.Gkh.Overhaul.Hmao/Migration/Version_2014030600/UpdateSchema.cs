namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014030600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014030400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", new Column("POINT_PARAMS", DbType.String, 2000, ColumnProperty.Null, "'[]'"));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("POINT_PARAMS", DbType.String, 2000, ColumnProperty.Null, "'[]'"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL_IN_PRG_3", "POINT_PARAMS");
            Database.RemoveColumn("OVRHL_VERSION_REC", "POINT_PARAMS");
        }
    }
}