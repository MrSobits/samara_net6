namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016121900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция RegOperator 2016121900
    /// </summary>
    [Migration("2016121900")]
    [MigrationDependsOn(typeof(Version_2016090900.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016101900.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016110200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_DISTR_DETAIL", new Column("PAYMENT_ACC", DbType.String));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_DISTR_DETAIL", "PAYMENT_ACC");
        }
    }
}
