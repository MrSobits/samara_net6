namespace Bars.GisIntegration.RegOp.Migrations.Version_2016100500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    
    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016100500")]
    [MigrationDependsOn(typeof(Version_2016100400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //if (!this.Database.ColumnExists("RIS_PAYMENT_DOC", "PAYMENT_DOC_NUM"))
            //{
            //    this.Database.AddColumn("RIS_PAYMENT_DOC", new Column("PAYMENT_DOC_NUM", DbType.String, 100));
            //}
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //if (this.Database.ColumnExists("RIS_PAYMENT_DOC", "PAYMENT_DOC_NUM"))
            //{
            //    this.Database.RemoveColumn("RIS_PAYMENT_DOC", "PAYMENT_DOC_NUM");
            //}
        }
    }
}