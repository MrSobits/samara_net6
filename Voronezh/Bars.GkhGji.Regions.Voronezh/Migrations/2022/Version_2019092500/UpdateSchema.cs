namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2019092500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019092500")]
    [MigrationDependsOn(typeof(Version_2019092400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddRefColumn("GJI_VR_COURT_PRACTICE", new RefColumn("DOCUMENT_ID", ColumnProperty.None, "FK_GJI_VR_COURT_PRACTICE_DOCUMENT_ID", "GJI_DOCUMENT", "ID"));           
        }

        public override void Down()
        {
         
            Database.RemoveColumn("GJI_VR_COURT_PRACTICE", "DOCUMENT_ID");
        }
    }
}