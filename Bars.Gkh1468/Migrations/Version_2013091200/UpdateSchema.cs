namespace Bars.Gkh1468.Migrations.Version_2013091200
{
    using System.Data;
    using System.Linq;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!this.Database.TableExists("GKH_RO_PUB_SERVORG"))
            {
                this.Database.AddEntityTable(
                 "GKH_RO_PUB_SERVORG",
                 new RefColumn("PUB_SERVORG_ID", "GKH_RO_PUB_SERVORG", "GKH_PUBLIC_SERVORG", "ID"),
                 new RefColumn("REAL_OBJ_ID", "GKH_RO_PUB_S_ORG_RO", "GKH_REALITY_OBJECT", "ID"),
                 new Column("DATE_START", DbType.DateTime),
                 new Column("DATE_END", DbType.DateTime));
            }
        }

        public override void Down()
        {
            var gkhMigration = this.Database.GetAppliedMigrations().Any(x => x.ModuleId == "Bars.Gkh" && x.Version == "2016052000");

            if (!gkhMigration)
            {
                this.Database.RemoveTable("GKH_RO_PUB_SERVORG");
            }
        }
    }
}