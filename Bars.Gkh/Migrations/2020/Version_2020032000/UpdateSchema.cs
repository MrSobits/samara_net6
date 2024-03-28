namespace Bars.Gkh.Migrations._2020.Version_2020032000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020032000")]
    
    [MigrationDependsOn(typeof(Version_2020031200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_WORK", new Column("GIS_GKH_CODE", DbType.String));
            Database.AddColumn("GKH_DICT_WORK", new Column("GIS_GKH_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_WORK", "GIS_GKH_GUID");
            Database.RemoveColumn("GKH_DICT_WORK", "GIS_GKH_CODE");
        }
    }
}