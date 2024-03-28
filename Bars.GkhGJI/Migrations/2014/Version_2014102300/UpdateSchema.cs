namespace Bars.GkhGji.Migrations.Version_2014102300
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2014.Version_2014101600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.ColumnExists("GJI_DICT_VIOLATION", "NORMATIVE_DOC_ITEM_ID"))
            {
                return;
            }

            Database.AddRefColumn("GJI_DICT_VIOLATION", new RefColumn("NORMATIVE_DOC_ITEM_ID", ColumnProperty.Null, "NORMATIVE_DOC_ITEM", "GKH_DICT_NORMATIVE_DOC_ITEM", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_VIOLATION", "NORMATIVE_DOC_ITEM_ID");
        }
    }
}