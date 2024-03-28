namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016090900
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция RegOperator 2016090900
    /// </summary>
    [Migration("2016090900")]
    [MigrationDependsOn(typeof(Version_2016090800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.RemoveColumn("REGOP_CHARGE_OPERATION_BASE", "DOC_NUM");

            this.Database.AddRefColumn("REGOP_CHARGE_OPERATION_BASE", new FileColumn("DOC_ID", ColumnProperty.Null, "REGOP_CHARGE_DOC"));

            const string deletePenaltyCopyTransfersQuery = @"delete from regop_transfer 
                where reason like 'Отмена начисления пени'
                and is_copy_for_source=true;";

            this.Database.ExecuteNonQuery(deletePenaltyCopyTransfersQuery);

            const string updatePenaltyTransfersQuery = @"update regop_transfer 
                set source_guid=target_guid, 
                target_guid=source_guid
                where reason like 'Отмена начисления пени';";

            this.Database.ExecuteNonQuery(updatePenaltyTransfersQuery);
        }
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_CHARGE_OPERATION_BASE", "DOC_ID");
        }
    }
}
