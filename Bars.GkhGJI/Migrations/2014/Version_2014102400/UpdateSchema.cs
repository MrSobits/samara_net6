namespace Bars.GkhGji.Migrations.Version_2014102400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014102300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.ColumnExists("GJI_DICT_VIOLATION", "NORMATIVE_DOC_ITEM_ID"))
            {
                Database.RemoveColumn("GJI_DICT_VIOLATION", "NORMATIVE_DOC_ITEM_ID");
            }

            Database.AddEntityTable("GJI_DICT_VIOL_NORMDITEM",
                new RefColumn("VIOLATION_ID", ColumnProperty.NotNull, "VIOLATION", "GJI_DICT_VIOLATION", "ID"),
                new RefColumn("NORMATIVEDOCITEM_ID", ColumnProperty.NotNull, "NORMATIVEDOCITEM", "GKH_DICT_NORMATIVE_DOC_ITEM", "ID"));
        }

        public override void Down()
        {
            Database.AddRefColumn("GJI_DICT_VIOLATION", new RefColumn("NORMATIVE_DOC_ITEM_ID", ColumnProperty.Null, "NORMATIVE_DOC_ITEM", "GKH_DICT_NORMATIVE_DOC_ITEM", "ID"));

            Database.RemoveTable("GJI_DICT_VIOLATION_NORMATIVEDOCITEM");
        }
    }
}