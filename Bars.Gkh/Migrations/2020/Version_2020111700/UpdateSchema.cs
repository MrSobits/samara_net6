namespace Bars.Gkh.Migrations._2020.Version_2020111700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2020111900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2020111600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        { 

            Database.AddEntityTable(
             "GKH_RO_CATEGORYCS",
            new RefColumn("RO_ID", ColumnProperty.None, "GKH_RO_CATEGORYCS_RO", "GKH_REALITY_OBJECT", "ID"),
             new RefColumn("CATEGOTY_ID", ColumnProperty.None, "GKH_RO_CATEGORYCS_CATEGORY", "GKH_CS_CATEGORY", "ID"));

            Database.AddColumn("GKH_CS_TARIF", new RefColumn("CATEGORY_ID", ColumnProperty.None, "GKH_CS_TARIF_CATEGORY_ID", "GKH_CS_CATEGORY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CS_TARIF", "CATEGORY_ID");
            Database.RemoveTable("GKH_RO_CATEGORYCS");
        }
    }
}