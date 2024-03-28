namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017032200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2017032200
    /// </summary>
    [Migration("2017032200")]
    [MigrationDependsOn(typeof(_2016.Version_2016111800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017020200.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017020700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017030100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "DELIVERY_AGENT", DbType.String, 250);
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "DELIVERY_AGENT");
        }
    }
}
