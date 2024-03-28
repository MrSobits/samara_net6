namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016012100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016.01.21.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016012100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2016.Version_2016011400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Up()
        {
            this.Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "DATE_RECEIPT_OR_WRITE_OFF");
        }

        /// <summary>
        /// Вверх
        /// </summary>
        public override void Down()
        {
            this.Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("DATE_RECEIPT_OR_WRITE_OFF", DbType.DateTime));
        }
    }
}
