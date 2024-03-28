namespace Bars.Gkh.Migrations.Version_2013112700
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013112600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // сущность связи м\у supplyResourceOrg и RealtyObject
            Database.AddEntityTable("GKH_SUPPLY_RESORG_RO",
                new RefColumn("SUPPLY_RESORG_ID", ColumnProperty.NotNull, "GKH_SUPPLY_RESORG_RO_SRO", "GKH_SUPPLY_RESORG", "ID"),
                new RefColumn("REALITYOBJECT_ID", ColumnProperty.NotNull, "GKH_SUPPLY_RESORG_RO_RO", "GKH_REALITY_OBJECT", "ID"));

            //ViewManager.Drop(Database, "Gkh");
            //ViewManager.Create(Database, "Gkh");
        }

        public override void Down()
        {
            ViewManager.Drop(Database, "Gkh");

            Database.RemoveTable("GKH_SUPPLY_RESORG_RO");
        }
    }
}