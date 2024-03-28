namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015111600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015.11.16.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015111600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015111300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "IS_DISTRIBUTABLE");
        }

        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_BANK_ACC_STMNT", "IS_DISTRIBUTABLE", DbType.Int16, ColumnProperty.NotNull, 10);
        }
    }
}
