namespace Bars.Gkh.Migrations._2017.Version_2017041000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017041000
    /// </summary>
    [Migration("2017041000")]
    [MigrationDependsOn(typeof(Version_2017032800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_LICENSE_REGISTRATION_REASON", 
                new Column("NAME", DbType.String, ColumnProperty.NotNull));

            this.Database.AddEntityTable(
                "GKH_LICENSE_REJECT_REASON",
                new Column("NAME", DbType.String, ColumnProperty.NotNull));

            this.Database.AddRefColumn("GKH_MANORG_LIC_REQUEST", 
                new RefColumn("REGISTRATION_REASON_ID", "GKH_MANORG_LIC_REQUEST_REGISTR_REASON_ID", "GKH_LICENSE_REGISTRATION_REASON", "ID"));

            this.Database.AddRefColumn("GKH_MANORG_LIC_REQUEST",
                new RefColumn("REJECT_REASON_ID", "GKH_MANORG_LIC_REQUEST_REJECT_REASON_ID", "GKH_LICENSE_REJECT_REASON", "ID"));
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "REGISTRATION_REASON_ID");
            this.Database.RemoveColumn("GKH_MANORG_LIC_REQUEST", "REJECT_REASON_ID");

            this.Database.RemoveTable("GKH_LICENSE_REGISTRATION_REASON");
            this.Database.RemoveTable("GKH_LICENSE_REJECT_REASON");
        }
    }
}