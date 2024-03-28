namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016102700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция RegOperator 2016102700
    /// </summary>
    [Migration("2016102700")]
    [MigrationDependsOn(typeof(Version_2016101500.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016101700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("RAA_WALLET_ID", ColumnProperty.Null, "REGOP_ROACC_W_RAA", "REGOP_WALLET", "ID"));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "RAA_WALLET_ID");
        }
    }
}
