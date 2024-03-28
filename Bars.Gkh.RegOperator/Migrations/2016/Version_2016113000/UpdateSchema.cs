namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016113000
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016113000
    /// </summary>
    [Migration("2016113000")]
    [MigrationDependsOn(typeof(Version_2016111500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Up
        /// </summary>
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"update REGOP_PERS_ACC_RECALC_EVT e
                                            set period_id = rp.id
                                            from regop_period rp
                                            where rp.cis_closed = false 
                                            and e.period_id is null");
        }
    }
}
