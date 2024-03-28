namespace Bars.Gkh.Migrations.Version_2013072700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GKH_DICT_NORMATIVE_DOC",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.Int32, ColumnProperty.NotNull));

            Database.RemoveColumn("GKH_DICT_CONST_ELEMENT", "IS_MATCHES_VSN");
            Database.AddRefColumn("GKH_DICT_CONST_ELEMENT", new RefColumn("NORM_DOC_ID", "GKH_CONST_EL_NDOC", "GKH_DICT_NORMATIVE_DOC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_CONST_ELEMENT", "NORM_DOC_ID");
            Database.RemoveTable("GKH_DICT_NORMATIVE_DOC");
            Database.AddColumn("GKH_DICT_CONST_ELEMENT", new Column("IS_MATCHES_VSN", DbType.Boolean, ColumnProperty.NotNull, false));
        }
    }
}