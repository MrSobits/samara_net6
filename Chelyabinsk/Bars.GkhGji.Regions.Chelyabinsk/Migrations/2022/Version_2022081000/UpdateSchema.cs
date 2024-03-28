namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2022081000
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022081000")]
    [MigrationDependsOn(typeof(Version_2022080500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
           "GJI_CH_APPCIT_ADMON_APPEAL",
           new RefColumn("APPCIT_ADMONITION_ID", ColumnProperty.None, "GJI_CH_APPEAL_APPCIT_ADMONITION_ID", "GJI_CH_APPCIT_ADMONITION", "ID"),
           new RefColumn("APPCIT_ID", ColumnProperty.None, "GJI_CH_APPEAL_APPCIT_ID", "GJI_APPEAL_CITIZENS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_APPCIT_ADMON_APPEAL");
        }
    }
}