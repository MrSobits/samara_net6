namespace Bars.Gkh1468.Migrations.Version_2013112800
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013112700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // сущность связи м\у PublicServiceOrg и RealtyObject
            Database.AddEntityTable("GKH_PUBLIC_SERVORG_RO",
                new RefColumn("PUBLIC_SERVORG_ID", ColumnProperty.NotNull, "GKH_PUBLIC_SERVORG_RO_SRO", "GKH_PUBLIC_SERVORG", "ID"),
                new RefColumn("REALITYOBJECT_ID", ColumnProperty.NotNull, "GKH_PUBLIC_SERVORG_RO_RO", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_PUBLIC_SERVORG_RO");
        }
    }
}