namespace Bars.GkhGji.Migrations._2019.Version_2019072300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019072300")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019071000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {       
            Database.AddColumn("GJI_DICT_PROSECUTOR_OFFICE", new RefColumn("MUNICIPALITY_ID", ColumnProperty.None, "FK_GJI_DICT_PROSECUTOR_OFFICE_MUN", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_PROSECUTOR_OFFICE", "MUNICIPALITY_ID");
            
        }
    }
}