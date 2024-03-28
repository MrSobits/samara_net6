namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017011801
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция RegOperator 2017011800
    /// </summary>
    [Migration("2017011801")]
    [MigrationDependsOn(typeof(Version_2017011800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string Query = 
            @"delete 
            from REGOP_PAYMENT_DOC_ACC_LOG
            where HOLDER_TYPE = 1;

            update REGOP_PAYMENT_DOC_ACC_LOG
            set account_id = HOLDER_ID";

        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("REGOP_PAYMENT_DOC_LOG", new RefColumn("PERIOD_ID", "FK_PAY_DOC_LOG_PERIOD", "REGOP_PERIOD", "ID" ));
            this.Database.AddColumn("REGOP_PAYMENT_DOC_ACC_LOG", "ACCOUNT_ID", DbType.Int64, ColumnProperty.Null);

            this.Database.ExecuteNonQuery(UpdateSchema.Query);

            this.Database.AlterColumnSetNullable("REGOP_PAYMENT_DOC_ACC_LOG", "ACCOUNT_ID", false);

            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_ACC_LOG", "HOLDER_ID");
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_ACC_LOG", "HOLDER_TYPE");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_ACC_LOG", "ACCOUNT_ID");
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_LOG", "PERIOD_ID");
        }
    }
}
