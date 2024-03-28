namespace Bars.Gkh.Migrations.Version_2013072402
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072402")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_CONST_ELEMENT", new RefColumn("GROUP_ID", "GKH_CONST_EL_GROUP", "GKH_DICT_CONEL_GROUP", "ID"));
            Database.AddColumn("GKH_DICT_CONST_ELEMENT", new Column("IS_MATCHES_VSN", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_CONST_ELEMENT", "GROUP_ID");
            Database.RemoveColumn("GKH_DICT_CONST_ELEMENT", "IS_MATCHES_VSN");
        }
    }
}