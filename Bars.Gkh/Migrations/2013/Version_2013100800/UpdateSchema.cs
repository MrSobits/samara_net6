namespace Bars.Gkh.Migrations.Version_2013100800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013100401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("IS_REPAIR_INADVISABLE", DbType.Boolean));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "IS_REPAIR_INADVISABLE");
        }
    }
}