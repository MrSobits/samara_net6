namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015102700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015.10.27.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015102700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015102200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("ADDRESS_BY_IMPORT", DbType.String, 500));
            this.Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("OWNER_BY_IMPORT", DbType.String, 250));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "ADDRESS_BY_IMPORT");
            this.Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "OWNER_BY_IMPORT");
        }
    }
}
