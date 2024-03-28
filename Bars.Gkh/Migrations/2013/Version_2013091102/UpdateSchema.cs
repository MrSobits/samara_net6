namespace Bars.Gkh.Migrations.Version_2013091102
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091102")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013091101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //сущность связи м\у supplyResourceOrg и Municipality

            Database.AddEntityTable("GKH_SUPPLY_RESORG_MU",
                new RefColumn("SUPPLY_RESORG_ID", ColumnProperty.NotNull, "GKH_SUPPLY_RESORG_MU_SRO", "GKH_SUPPLY_RESORG", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GKH_SUPPLY_RESORG_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_SUPPLY_RESORG_MU");
        }
    }
}