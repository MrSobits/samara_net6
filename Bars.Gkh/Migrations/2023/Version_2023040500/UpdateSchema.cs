namespace Bars.Gkh.Migrations._2023.Version_2023040600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023040600")]
    
    [MigrationDependsOn(typeof(_2022.Version_2022090500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_INSPECTOR", new Column("ERKNM_TITLE_GUID", DbType.String, 40));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_INSPECTOR", "ERKNM_TITLE_GUID");
        }
    }
}