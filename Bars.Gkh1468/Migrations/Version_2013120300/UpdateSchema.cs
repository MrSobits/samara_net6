namespace Bars.Gkh1468.Migrations.Version_2013120300
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Data;
    using System.Linq;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013112800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            var gkhMigration = this.Database.GetAppliedMigrations().Any(x => x.ModuleId == "Bars.Gkh" && x.Version == "2016052000");

            if (!gkhMigration)
            {
                this.Database.AddRefColumn("GKH_RO_PUB_SERVORG", new RefColumn("FILE_INFO_ID", "GKH_PUBSERV_FILE", "B4_FILE_INFO", "ID"));

                this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("CONTRACT_NUMBER", DbType.String));
                this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("CONTRACT_DATE", DbType.DateTime));
                this.Database.AddColumn("GKH_RO_PUB_SERVORG", new Column("NOTE", DbType.String, 300));
            }
        }

        public override void Down()
        {
            var appliedGkhMigration = this.Database.GetAppliedMigrations().Any(x => x.ModuleId == "Bars.Gkh" && x.Version == "2016052000");

            if (!appliedGkhMigration)
            {
                this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "FILE_INFO_ID");
                this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "CONTRACT_NUMBER");
                this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "CONTRACT_DATE");
                this.Database.RemoveColumn("GKH_RO_PUB_SERVORG", "NOTE");
            }
        }
    }
}