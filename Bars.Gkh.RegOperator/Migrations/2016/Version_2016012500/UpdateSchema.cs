namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016012500
{
    using System.Data;
    using B4.Utils;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016,01,25,00
    /// </summary>
    [Migration("2016012500")]
    [MigrationDependsOn(typeof(Version_2016011400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("PAYMENTS_TO_CLOSED_PERIODS_IMPORT",
                new Column("PERIOD", DbType.String),
                new Column("SOURCE", DbType.Byte),
                new Column("DOCUMENT_NUM", DbType.String),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("OPERATION_DATE", DbType.DateTime),
                new Column("PAYMENT_AGENT_NAME", DbType.String),
                new Column("PAYMENT_NUMBER_US", DbType.String),
                new Column("TRANSFER_GUID", DbType.String));
            
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("PAYMENTS_TO_CLOSED_PERIODS_IMPORT");
        }
    }
}
