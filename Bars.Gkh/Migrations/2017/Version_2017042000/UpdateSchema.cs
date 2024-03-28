namespace Bars.Gkh.Migrations._2017.Version_2017042000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017042000
    /// </summary>
    [Migration("2017042000")]
    [MigrationDependsOn(typeof(Version_2017021000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017030600.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017030601.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017032700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017040400.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017040800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017041000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("GKH_EMERGENCY_RESETPROG", new Column("ACTUAL_COST", DbType.Decimal, ColumnProperty.Null));
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_EMERGENCY_RESETPROG", "ACTUAL_COST");
        }
    }
}