namespace Bars.Gkh.Migrations.Version_2014020402
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020402")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014020401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("MO_ADDRESS", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "MO_ADDRESS");
        }
    }
}