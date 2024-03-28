namespace Bars.GkhGji.Migrations._2019.Version_2019120900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019120900")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019112400.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_DICT_VIOLATION", new RefColumn("PARENT_VIOLATION", "FK_VIOLATION_VIOLATION", "GJI_DICT_VIOLATION", "ID"));  
        }

        public override void Down()
        {          
            Database.RemoveColumn("GJI_DICT_VIOLATION", "PARENT_VIOLATION");
        }
    }
}