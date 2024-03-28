namespace Bars.Gkh.Ris.Migrations.Version_2016052700
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    [Migration("2016052700")]
    [MigrationDependsOn(typeof(Version_2016052400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //if (!this.Database.ColumnExists("RIS_PAYMENT_PERIOD", "IS_APPLIED"))
            //{
            //    this.Database.AddColumn("RIS_PAYMENT_PERIOD", new Column("IS_APPLIED", DbType.Boolean, ColumnProperty.NotNull, false));
            //}
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //if (this.Database.ColumnExists("RIS_PAYMENT_PERIOD", "IS_APPLIED"))
            //{
            //    this.Database.RemoveColumn("RIS_PAYMENT_PERIOD", "IS_APPLIED");
            //}
        }
    }
}