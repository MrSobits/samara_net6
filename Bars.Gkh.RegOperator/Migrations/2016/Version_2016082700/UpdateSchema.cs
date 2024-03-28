namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016082700
{
    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016082700
    /// </summary>
    [Migration("2016082700")]
    [MigrationDependsOn(typeof(Version_2016081001.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_BANK_STMNT_OP",
                new Column("CODE", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("BANK_STMNT_ID", ColumnProperty.NotNull, "BANK_STMNT", "REGOP_BANK_ACC_STMNT", "ID"),
                new RefColumn("OP_ID", ColumnProperty.NotNull, "MONEY_OPERATION", "REGOP_MONEY_OPERATION", "ID"));
        }
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_BANK_STMNT_OP");
        }
    }
}
