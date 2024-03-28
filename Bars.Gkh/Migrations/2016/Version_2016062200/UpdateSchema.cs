namespace Bars.Gkh.Migrations._2016.Version_2016062200
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016062200
    /// </summary>
    [Migration("2016062200")]
    [MigrationDependsOn(typeof(Version_2016060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("PAYMENT_SERV_DATE", DbType.Byte, ColumnProperty.NotNull, 0));
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("PAYMENT_SERV_DATE_THIS_MONTH", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("MD_BEGIN_DATE_THIS_MONTH", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("MD_END_DATE_THIS_MONTH", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "PAYMENT_SERV_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT","PAYMENT_SERV_DATE_THIS_MONTH");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "MD_BEGIN_DATE_THIS_MONTH");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "MD_END_DATE_THIS_MONTH");
        }
    }
}
