namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016081001
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016081001
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016081001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2016.Version_2016070401.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeColumn("regop_payment_doc_snapshot", new Column("municipality", DbType.String, 500));
        }
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}
