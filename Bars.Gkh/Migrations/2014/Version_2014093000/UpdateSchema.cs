namespace Bars.Gkh.Migrations.Version_2014093000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Enums;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014093000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014090800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_NORMATIVE_DOC", new Column("CATEGORY", DbType.Int16, ColumnProperty.NotNull, (short) NormativeDocCategory.Overhaul));
            Database.AddEntityTable(
                "GKH_DICT_NORMATIVE_DOC_ITEM",
                new Column("DOC_NUMBER", DbType.String, 100, ColumnProperty.NotNull),
                new Column("DOC_TEXT", DbType.String, 2000, ColumnProperty.Null),
                new RefColumn("NORMATIVE_DOC_ID", "NORMATIVE_DOC", "GKH_DICT_NORMATIVE_DOC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_NORMATIVE_DOC", "CATEGORY");
            Database.RemoveTable("GKH_DICT_NORMATIVE_DOC_ITEM");
        }
    }
}