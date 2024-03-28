namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021120700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021120700")]
    [MigrationDependsOn(typeof(Version_2021120200.UpdateSchema))]
    public class UpdateSchema: Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable("GJI_ACT_ACTIONISOLATED", "GJI_ACTCHECK", "GJI_ACT_GJI_ACT_ACTIONISOLATED_ID");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_ACT_ACTIONISOLATED");
        }
    }
}