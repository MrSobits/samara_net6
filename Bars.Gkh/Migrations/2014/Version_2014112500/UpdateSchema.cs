namespace Bars.Gkh.Migrations.Version_2014112500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014112400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.ColumnExists("GKH_DICT_NORMATIVE_DOC_ITEM", "NUMBER"))
            {
                Database.RenameColumn("GKH_DICT_NORMATIVE_DOC_ITEM", "NUMBER", "DOC_NUMBER");
            }

            if (Database.ColumnExists("GKH_DICT_NORMATIVE_DOC_ITEM", "TEXT"))
            {
                Database.RenameColumn("GKH_DICT_NORMATIVE_DOC_ITEM", "TEXT", "DOC_TEXT");
            }
        }

        public override void Down()
        {
            if (Database.ColumnExists("GKH_DICT_NORMATIVE_DOC_ITEM", "DOC_TEXT"))
            {
                Database.RenameColumn("GKH_DICT_NORMATIVE_DOC_ITEM", "DOC_TEXT", "TEXT");
            }

            if (Database.ColumnExists("GKH_DICT_NORMATIVE_DOC_ITEM", "DOC_NUMBER"))
            {
                Database.RenameColumn("GKH_DICT_NORMATIVE_DOC_ITEM", "DOC_NUMBER", "NUMBER");
            }
        }
    }
}