namespace Bars.Gkh.Migrations.Version_2014062200
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014062000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Платежный агент
            Database.AddEntityTable(
                "GKH_PAYMENT_AGENT",
                new RefColumn("CONTRAGENT_ID", "GKH_PAYMENT_AGENT_CTR", "GKH_CONTRAGENT", "ID"),
                new Column("CODE", DbType.String, 50));
            //------
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_PAYMENT_AGENT");
        }
    }
}