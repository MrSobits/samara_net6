namespace Bars.Gkh.Gis.Migrations._2015.Version_2015102300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2015102300")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015100100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("BIL_DICT_SCHEMA", new Column("ERC_CODE", DbType.Int64));
            this.Database.AddColumn("BIL_DICT_SCHEMA", new Column("SENDER_LOCAL_SCHEMA_PREFIX", DbType.String, 50));
            this.Database.AddColumn("BIL_DICT_SCHEMA", new Column("SENDER_CENTRAL_SCHEMA_PREFIX", DbType.String, 50));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("BIL_DICT_SCHEMA", "ERC_CODE");
            this.Database.RemoveColumn("BIL_DICT_SCHEMA", "SENDER_LOCAL_SCHEMA_PREFIX");
            this.Database.RemoveColumn("BIL_DICT_SCHEMA", "SENDER_CENTRAL_SCHEMA_PREFIX");
        }
    }
}