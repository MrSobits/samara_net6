namespace Bars.Gkh.Migrations._2021.Version_2021030400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021030400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(_2020.Version_2020122100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("REPLY_TO", DbType.String, 500));
            Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("RPGU_NUMBER", DbType.String, 50));
            Database.AddColumn("GKH_MANORG_LIC_REQUEST", new Column("MESSAGE_ID", DbType.String, 50));
            Database.AddColumn("GKH_MANORG_REQ_RPGU", new Column("MESSAGE_ID", DbType.String, 50));          
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_MANORG_REQ_RPGU", "MESSAGE_ID");
            Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "MESSAGE_ID");
            Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "RPGU_NUMBER");
            Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "REPLY_TO");
        }
    }
}