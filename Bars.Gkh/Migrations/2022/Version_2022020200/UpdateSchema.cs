namespace Bars.Gkh.Migrations._2022.Version_2022020200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022020200")]
    
    [MigrationDependsOn(typeof(_2021.Version_2021110200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_PERSON_PLACEWORK", new RefColumn("FILE_ID", "GKH_PERSON_PLACEWORK_FILE", "B4_FILE_INFO", "ID"));     
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_PERSON_PLACEWORK", "FILE_ID");
        }
    }
}