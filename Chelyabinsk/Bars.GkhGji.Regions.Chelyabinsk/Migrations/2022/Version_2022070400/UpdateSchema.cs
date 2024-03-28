namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2022070400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022070400")]
    [MigrationDependsOn(typeof(Version_2022051900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddRefColumn("GJI_CH_APPCIT_ADMONITION", new RefColumn("REALITY_OBJECT_ID", ColumnProperty.None, "GJI_APP_ADMON_REALITY_OBJECT", "GKH_REALITY_OBJECT", "ID"));
            

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "REALITY_OBJECT_ID");
        }
    }
}