namespace Bars.GkhGji.Migrations.Version_2013051400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013051400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013043000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_STATEMENT_REQUEST", new Column("DESCRIPTION", DbType.String, 2000));
            Database.ChangeColumn("GJI_PROTOCOL_VIOLAT", new Column("DESCRIPTION", DbType.String, 1000));
        }

        public override void Down()
        {
        }
    }
}