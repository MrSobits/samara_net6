namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015111100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015.11.11.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015111100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015102800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("BANK_STATEMENT", DbType.String, 500));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "BANK_STATEMENT");
        }
    }
}
