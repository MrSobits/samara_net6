namespace Bars.GkhGji.Migration.Version_2014071700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014071501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_ACTCHECK_DEFINITION", new Column("DOC_NUMBER", DbType.Int32));
            Database.AddColumn("GJI_PROTOCOL_DEFINITION", new Column("DOC_NUMBER", DbType.Int32));
            Database.AddColumn("GJI_PROTOCOLMHC_DEFINITION", new Column("DOC_NUMBER", DbType.Int32));
            Database.AddColumn("GJI_RESOLUTION_DEFINITION", new Column("DOC_NUMBER", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECK_DEFINITION", "DOC_NUMBER");
            Database.RemoveColumn("GJI_PROTOCOL_DEFINITION", "DOC_NUMBER");
            Database.RemoveColumn("GJI_PROTOCOLMHC_DEFINITION", "DOC_NUMBER");
            Database.RemoveColumn("GJI_RESOLUTION_DEFINITION", "DOC_NUMBER");
        }
    }
}