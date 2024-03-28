namespace Bars.Gkh.Migrations._2022.Version_2022090500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022090500")]
    
    [MigrationDependsOn(typeof(_2022.Version_2022053000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_INSPECTOR", new Column("ERKNM_POSITION_GUID", DbType.String, 40));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_INSPECTOR", "ERKNM_POSITION_GUID");
        }
    }
}