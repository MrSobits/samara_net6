namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016011400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015.12.03.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016011400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015120300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("DATE_RECEIPT_OR_WRITE_OFF", DbType.DateTime));
            
        }

        /// <summary>
        /// Вверх
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "DATE_RECEIPT_OR_WRITE_OFF");
        }
    }
}
