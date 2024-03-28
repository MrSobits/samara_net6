namespace Bars.Gkh.Migrations._2016.Version_2016110300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Миграция 2016110300
    /// </summary>
    [Migration("2016110300")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016102300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("SET_PAYMENTS_FOUNDATION", DbType.Int32, ColumnProperty.NotNull, 0));
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("REVOCATION_REASON", DbType.String));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "SET_PAYMENTS_FOUNDATION");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "REVOCATION_REASON");
        }
    }
}