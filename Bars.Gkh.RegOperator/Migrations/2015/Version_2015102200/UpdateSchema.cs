namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015102200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015.10.22.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015102200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015101800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("FACT_RECEIVER_NUMBER", DbType.String, 100));
            this.Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("EXT_ACC_NUMBER", DbType.String, 100));
            this.Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("IS_DETERMINATE_MANUALLY", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddRefColumn("REGOP_IMPORTED_PAYMENT", new RefColumn("PERS_ACC_ID", "ROP_IMP_PAY_PERS_ACC", "REGOP_PERS_ACC", "ID"));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "FACT_RECEIVER_NUMBER");
            this.Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "EXT_ACC_NUMBER");
            this.Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "IS_DETERMINATE_MANUALLY");
            this.Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "PERS_ACC_ID");
        }
    }
}
