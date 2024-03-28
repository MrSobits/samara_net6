namespace Bars.Gkh.Migrations._2020.Version_2020051900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020051900")]
    
    [MigrationDependsOn(typeof(Version_2020042400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_INSPECTOR", new Column("GIS_GKH_GUID", DbType.String, 36));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_INSPECTOR", "GIS_GKH_GUID");
        }
    }
}