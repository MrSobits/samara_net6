namespace Bars.GkhDi.Migrations.Version_2016111800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    /// <summary>
    /// Миграция добавления группы полей "Период внесения платы по договору"
    /// </summary>
    [Migration("2016111800")]
    [MigrationDependsOn(typeof(Version_2016082600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("DI_DISINFO_COM_FACILS", "DAY_MONTH_PERIOD_IN", DbType.Int32, ColumnProperty.Null);
            this.Database.AddColumn("DI_DISINFO_COM_FACILS", "DAY_MONTH_PERIOD_OUT", DbType.Int32, ColumnProperty.Null);
            this.Database.AddColumn("DI_DISINFO_COM_FACILS", "IS_LAST_DAY_MONTH_PERIOD_IN", DbType.Boolean, ColumnProperty.Null);
            this.Database.AddColumn("DI_DISINFO_COM_FACILS", "IS_LAST_DAY_MONTH_PERIOD_OUT", DbType.Boolean, ColumnProperty.Null);
            this.Database.AddColumn("DI_DISINFO_COM_FACILS", "IS_NEXT_MONTH_PERIOD_IN", DbType.Boolean, ColumnProperty.Null);
            this.Database.AddColumn("DI_DISINFO_COM_FACILS", "IS_NEXT_MONTH_PERIOD_OUT", DbType.Boolean, ColumnProperty.Null);
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("DI_DISINFO_COM_FACILS", "DAY_MONTH_PERIOD_IN");
            this.Database.RemoveColumn("DI_DISINFO_COM_FACILS", "DAY_MONTH_PERIOD_OUT");
            this.Database.RemoveColumn("DI_DISINFO_COM_FACILS", "IS_LAST_DAY_MONTH_PERIOD_IN");
            this.Database.RemoveColumn("DI_DISINFO_COM_FACILS", "IS_LAST_DAY_MONTH_PERIOD_OUT");
            this.Database.RemoveColumn("DI_DISINFO_COM_FACILS", "IS_NEXT_MONTH_PERIOD_IN");
            this.Database.RemoveColumn("DI_DISINFO_COM_FACILS", "IS_NEXT_MONTH_PERIOD_OUT");
        }
    }
}
