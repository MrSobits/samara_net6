namespace Bars.Gkh.Migrations.Version_2013121101
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013121100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_SERVORG_MU",
                new RefColumn("SERVORG_ID", ColumnProperty.NotNull, "GKH_SERVORG_MU_SRO", "GKH_SERVICE_ORGANIZATION", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GKH_SERVORG_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"));

            Database.AddEntityTable("GKH_MANORG_MU",
                new RefColumn("MANORG_ID", ColumnProperty.NotNull, "GKH_MANORG_MU_SRO", "GKH_MANAGING_ORGANIZATION", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GKH_MANORG_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"));
        }


        public override void Down()
        {
            Database.RemoveIndex("IND_GKH_SERVORG_MU_SRO", "GKH_SERVORG_MU");
            Database.RemoveIndex("IND_GKH_SERVORG_MU_MU", "GKH_SERVORG_MU");

            Database.RemoveConstraint("GKH_SERVORG_MU", "FK_GKH_SERVORG_MU_SRO");
            Database.RemoveConstraint("GKH_SERVORG_MU", "FK_GKH_SERVORG_MU_MU");

            Database.RemoveTable("GKH_SERVORG_MU");

            Database.RemoveIndex("IND_GKH_MANORG_MU_SRO", "GKH_MANORG_MU");
            Database.RemoveIndex("IND_GKH_MANORG_MU_MU", "GKH_MANORG_MU");

            Database.RemoveConstraint("GKH_MANORG_MU", "FK_GKH_MANORG_MU_SRO");
            Database.RemoveConstraint("GKH_MANORG_MU", "FK_GKH_MANORG_MU_MU");

            Database.RemoveTable("GKH_MANORG_MU");
        }

    }
}