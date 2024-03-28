namespace Bars.Gkh.Migrations.Version_2013051400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013051400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013050800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_REALITY_OBJECT", new Column("SERIES_HOME", DbType.String, 250));
            Database.ChangeColumn("GKH_OBJ_HOUSE_INFO", new Column("NAME", DbType.String, 100));
            Database.ChangeColumn("GKH_MAN_ORG_DOC", new Column("DOCUMENT_NAME", DbType.String, 300));
            Database.ChangeColumn("GKH_OBJ_SERVICE_ORG", new Column("DOCUMENT_NAME", DbType.String, 300));
        }

        public override void Down()
        {
        }
    }
}