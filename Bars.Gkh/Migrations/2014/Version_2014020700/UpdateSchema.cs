namespace Bars.Gkh.Migrations.Version_2014020700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014020604.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT", new Column("OKTMO", DbType.Int64));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT", "OKTMO");
        }
    }
}