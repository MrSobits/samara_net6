namespace Bars.Gkh.Migrations.Version_2013121100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013121000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Down()
        {
            Database.RemoveIndex("IND_GKH_SUPPLY_RESORG_MU_SRO", "GKH_SUPPLY_RESORG_MU");
            Database.RemoveIndex("IND_GKH_SUPPLY_RESORG_MU_MU", "GKH_SUPPLY_RESORG_MU");

            Database.RemoveConstraint("GKH_SUPPLY_RESORG_MU", "FK_GKH_SUPPLY_RESORG_MU_SRO");
            Database.RemoveConstraint("GKH_SUPPLY_RESORG_MU", "FK_GKH_SUPPLY_RESORG_MU_MU");

            Database.RemoveTable("GKH_SUPPLY_RESORG_MU");
        }

        public override void Up()
        {
            Database.AddEntityTable("GKH_SUPPLY_RESORG_MU",
                new RefColumn("SUPPLY_RESORG_ID", ColumnProperty.NotNull, "GKH_SUPPLY_RESORG_MU_SRO", "GKH_SUPPLY_RESORG", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GKH_SUPPLY_RESORG_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"));
        }
    }
}