namespace Bars.GkhGji.Migrations._2020.Version_2020121200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020121200")]
    [MigrationDependsOn(typeof(Version_2020120701.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_RESOLUTION", new RefColumn("CONSIDERATION_RESULT_ID", "GJI_RESOLUTION_CONSIDERATION_RESULT", "GJI_DICT_CONCEDERATION_RESULT", "ID"));      
        }

        public override void Down()
        {          
            Database.RemoveColumn("GJI_RESOLUTION", "CONSIDERATION_RESULT_ID");
        }
    }
}