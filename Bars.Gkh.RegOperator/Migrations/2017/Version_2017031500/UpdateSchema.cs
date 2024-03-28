namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017031500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// 2017031500
    /// </summary>
    [Migration("2017031500")]
    [MigrationDependsOn(typeof(Version_2017031300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить
        /// </summary>
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable("REGOP_IMPORT_CHARGE_CANCEL_SOURCE", "REGOP_CHARGE_OPERATION_BASE", "REGOP_IMPORT_CHARGE_CANCEL_SOURCE_ID");
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_IMPORT_CHARGE_CANCEL_SOURCE");
        }
    }
}
