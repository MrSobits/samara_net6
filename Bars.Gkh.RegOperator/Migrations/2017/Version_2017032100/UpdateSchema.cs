namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017032100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// 2017032100
    /// </summary>
    [Migration("2017032100")]
    [MigrationDependsOn(typeof(Version_2017031500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Up
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PERS_ACC_RECALC_EVT", new Column("RECALC_EVENT_TYPE", DbType.Int32, ColumnProperty.NotNull, 0));
        }

        /// <summary>
        /// Down
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_RECALC_EVT", "RECALC_EVENT_TYPE");
        }
    }
}
