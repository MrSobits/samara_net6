namespace Bars.GkhGji.Migrations._2016.Version_2016122100
{
    using Bars.B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2016122100
    /// </summary>
    [Migration("2016122100")]
    [MigrationDependsOn(typeof(Version_2016101300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
	    public override void Up()
	    {
            this.Database.AddEntityTable(
                "GJI_APP_CITS_SUG",
                new RefColumn("APPEAL_CITIZENS_ID", ColumnProperty.NotNull, "APP_CITS_SUG_APPEAL_CITIZENS_ID", "GJI_APPEAL_CITIZENS", "ID"),
                new RefColumn("CIT_SUG_ID", ColumnProperty.NotNull, "APP_CITS_SUG_CIT_SUG_ID", "GKH_CIT_SUG", "ID"));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
	    public override void Down()
        {
            this.Database.RemoveTable("GJI_APP_CITS_SUG");
        }
    }
}