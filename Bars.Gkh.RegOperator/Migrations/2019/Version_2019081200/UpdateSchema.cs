namespace Bars.Gkh.RegOperator.Migrations._2019.Version_2019081200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019081200")]
   
    [MigrationDependsOn(typeof(Version_2019040100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC", new Column("UNIFIED_ACC_NUM", DbType.String, 10));
            Database.AddColumn("REGOP_PERS_ACC", new Column("SERVICE_ID", DbType.String, 13));
            Database.AddColumn("REGOP_PERS_ACC", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("REGOP_PERS_ACC", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("REGOP_PERS_ACC", "GIS_GKH_GUID");
            Database.RemoveColumn("REGOP_PERS_ACC", "SERVICE_ID");
            Database.RemoveColumn("REGOP_PERS_ACC", "UNIFIED_ACC_NUM");
        }
    }
}