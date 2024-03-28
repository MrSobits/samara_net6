namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017011800
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2017011800
    /// </summary>
    [Migration("2017011800")]
    [MigrationDependsOn(typeof(_2016.Version_2016112500.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016112900.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016113000.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016120200.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016121300.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016121600.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016121900.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017011200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddIndex("IND_PAYMENT_DOC_SNAP_TYPE_HOLDER", false, "REGOP_PAYMENT_DOC_SNAPSHOT", "OWNER_TYPE", "HOLDER_ID");

            this.Database.AddUniqueConstraint("UNIQUE_BUILDER_CONFIG_PATH", "REGOP_BUILDER_CONFIG", "PATH");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveIndex("IND_PAYMENT_DOC_SNAP_TYPE_HOLDER", "REGOP_PAYMENT_DOC_SNAPSHOT");

            this.Database.RemoveConstraint("REGOP_BUILDER_CONFIG", "UNIQUE_BUILDER_CONFIG_PATH");
        }
    }
}
