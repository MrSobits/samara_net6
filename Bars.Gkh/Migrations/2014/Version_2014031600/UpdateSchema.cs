namespace Bars.Gkh.Migrations.Version_2014031600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014030601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("MAN_ORGS", DbType.String, 2000));
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("TYPES_CONTRACT", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "MAN_ORGS");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "TYPES_CONTRACT");
        }
    }
}