namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122701
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021122701")]
    [MigrationDependsOn(typeof(Version_2021122700.UpdateSchema))]
    public class UpdateSchema: Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddRefColumn("GJI_INSPECTION_ACTIONISOLATED", new RefColumn("ACTION_ISOLATED_ID", ColumnProperty.NotNull, "FK_TASK_GJI_INSPECTION_ID", "GJI_INSPECTION", "ID"));
            this.Database.RemoveColumn("GJI_INSPECTION_ACTIONISOLATED","INSPECTION_OBJECT");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_INSPECTION_ACTIONISOLATED","ACTION_ISOLATED_ID");
            this.Database.AddColumn("GJI_INSPECTION_ACTIONISOLATED", new Column("INSPECTION_OBJECT", DbType.Int32));
        }
    }
}