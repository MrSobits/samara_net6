namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015102800
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2015.10.28.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015102800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015102700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("REGOP_FS_IMPORT_MAP_ITEM", new RefColumn("PAYMENT_AGENT_ID", ColumnProperty.Null, "PA_FS_IMPORT", "GKH_PAYMENT_AGENT", "ID"));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_FS_IMPORT_MAP_ITEM", "PAYMENT_AGENT_ID");
        }
    }
}
