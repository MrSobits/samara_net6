namespace Bars.Gkh.Migration.Version_2015090201
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015090201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015090200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.ColumnExists("GKH_CONTRAGENT", "SENDER_ID"))
            {
                Database.RemoveColumn("GKH_CONTRAGENT", "SENDER_ID");
            }

            if (Database.ColumnExists("GKH_CONTRAGENT", "ORG_VERSION_GUID"))
            {
                Database.RemoveColumn("GKH_CONTRAGENT", "ORG_VERSION_GUID");
            }

            if (Database.ColumnExists("GKH_CONTRAGENT", "ORG_ROOT_ENTITY_GUID"))
            {
                Database.RemoveColumn("GKH_CONTRAGENT", "ORG_ROOT_ENTITY_GUID");
            }
        }

        public override void Down()
        {
            Database.AddColumn("GKH_CONTRAGENT", new Column("ORG_ROOT_ENTITY_GUID", DbType.String, 50));
            Database.AddColumn("GKH_CONTRAGENT", new Column("ORG_VERSION_GUID", DbType.String, 50));
            Database.AddColumn("GKH_CONTRAGENT", new Column("SENDER_ID", DbType.String, 50));
        }
    }
}