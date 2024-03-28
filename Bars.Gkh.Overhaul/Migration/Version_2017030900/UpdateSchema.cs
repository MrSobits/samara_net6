namespace Bars.Gkh.Overhaul.Migration.Version_2017030900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция Gkh.Overhaul.2017030900
    /// </summary>
    [Migration("2017030900")]
    [MigrationDependsOn(typeof(Version_2017020100.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016121600.UpdateSchema))] // миграция с лифтами
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("GKH_RO_LIFT", new RefColumn("RO_STRUCT_EL_ID", "RO_LIFT_RO_STRUCT_EL_ID", "OVRHL_RO_STRUCT_EL", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_RO_LIFT", "RO_STRUCT_EL_ID");
        }
    }
}