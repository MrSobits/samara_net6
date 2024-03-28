namespace Sobits.GisGkh.Migrations._2020.Version_2020012100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Sobits.GisGkh.Map;
    using System.Data;

    [Migration("2020012100")]
    [MigrationDependsOn(typeof(_2019.Version_2019111100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                GisGkhPayDocMap.TableName,
                new RefColumn("ACCOUNT", "REGOP_PERS_ACC_GIS_GKH_PAY_DOC_ID", "REGOP_PERS_ACC", "ID"),
                new RefColumn("PERIOD", "REGOP_PERIOD_GIS_GKH_PAY_DOC_ID", "REGOP_PERIOD", "ID"),
                new Column("PAY_DOC_ID", DbType.String, 18),
                new Column("PAY_DOC_TRANSPORT_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveTable(GisGkhPayDocMap.TableName);
        }
    }
}