namespace Bars.Gkh.Ris.Migrations.Version_2016051700
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016051700")]
    [MigrationDependsOn(typeof(Version_2016051600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //if (!this.Database.ColumnExists("RIS_CHARTER", "THIS_MONTH_PAYMENT_DOCDATE"))
            //{
            //    this.Database.AddColumn("RIS_CHARTER", new Column("THIS_MONTH_PAYMENT_DOCDATE", DbType.Boolean));
            //}

            //if (!this.Database.ColumnExists("RIS_CONTRACTATTACHMENT", "CHARTER_ID"))
            //{
            //    this.Database.AddColumn("RIS_CONTRACTATTACHMENT", new RefColumn("CHARTER_ID", "RIS_CONTRACTATT_CHART", "RIS_CHARTER", "ID"));
            //}

            
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //if (this.Database.ColumnExists("RIS_CHARTER", "THIS_MONTH_PAYMENT_DOCDATE"))
            //{
            //    this.Database.RemoveColumn("RIS_CHARTER", "THIS_MONTH_PAYMENT_DOCDATE");
            //}

            //if (this.Database.ColumnExists("RIS_CONTRACTATTACHMENT", "CHARTER_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_CONTRACTATTACHMENT", "CHARTER_ID");
            //}
        }
    }
}
