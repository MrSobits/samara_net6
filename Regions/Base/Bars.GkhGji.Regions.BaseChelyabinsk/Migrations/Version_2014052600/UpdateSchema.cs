namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014052600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014052500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_BASEJURPERSON_CONTRAGENT",
                new RefColumn("BASEJURPERSON_ID", ColumnProperty.NotNull, "GJI_JURCONTR_JUR", "GJI_INSPECTION_JURPERSON", "ID"),
                new RefColumn("CONTRAGENT_ID", "GJI_JURCONT_CONTR", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_BASEJURPERSON_CONTRAGENT");
        }
    }
}