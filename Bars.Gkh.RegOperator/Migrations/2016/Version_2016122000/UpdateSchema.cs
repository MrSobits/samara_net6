namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016122000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция RegOperator 2016122000
    /// </summary>
    [Migration("2016122000")]
    [MigrationDependsOn(typeof(Version_2016110200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_PAYMENT_DOC_ACC_LOG",
                new RefColumn("LOG_ID", ColumnProperty.NotNull, "PAYMENT_DOC_LOG_ID", "REGOP_PAYMENT_DOC_LOG", "ID"),
                new Column("HOLDER_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("HOLDER_TYPE", DbType.Int32, ColumnProperty.NotNull));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_PAYMENT_DOC_ACC_LOG");
        }
    }
}
