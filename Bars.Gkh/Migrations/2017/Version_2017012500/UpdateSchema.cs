namespace Bars.Gkh.Migrations._2017.Version_2017012500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017012500
    /// </summary>
    [Migration("2017012500")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016111700.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016112500.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016120500.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016121200.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016121500.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016121501.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016121700.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2017.Version_2017011800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddTable("GKH_RSOCONTRACT_FUEL_ENERGY_ORG", new Column("ID", DbType.Int64, ColumnProperty.NotNull));
            this.Database.AddForeignKey("FK_RSOCONTRACT_FUEL_ENERGY_ORG_ID", "GKH_RSOCONTRACT_FUEL_ENERGY_ORG", "ID", "GKH_RSOCONTRACT_BASE_PARTY", "ID");
            this.Database.AddIndex("IND_RSOCONTRACT_FUEL_ENERGY_ORG_ID", true, "GKH_RSOCONTRACT_FUEL_ENERGY_ORG", "ID");
            
            this.Database.AddRefColumn("GKH_RSOCONTRACT_FUEL_ENERGY_ORG", new RefColumn("FUEL_ENERGY_ORG_ID", ColumnProperty.NotNull, "RSOCONTRACT_FUEL_ENERGY_SERVORG_ID", "GKH_PUBLIC_SERVORG", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GKH_RSOCONTRACT_FUEL_ENERGY_ORG");
        }
    }
}