namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017042400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using ForeignKeyConstraint = Bars.B4.Modules.Ecm7.Framework.ForeignKeyConstraint;

    /// <summary>
    /// Миграция RegOperator 2017042400
    /// </summary>
    [Migration("2017042400")]
    [MigrationDependsOn(typeof(Version_2017042000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddPersistentObjectTable(
                "REGOP_RO_LOAN_TASK",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "RO_LOAN_TASK_RO_ID", "GKH_REALITY_OBJECT", "ID"),
                new Column("TASK_ID", DbType.Int64, ColumnProperty.NotNull));

            this.Database.AddIndex("IND_RO_LOAN_TASK_TID", false, "REGOP_RO_LOAN_TASK", "TASK_ID");
            this.Database.AddForeignKey("FK_RO_LOAN_TASK_TID", "REGOP_RO_LOAN_TASK", "TASK_ID", "B4_TASK_ENTRY", "ID", ForeignKeyConstraint.Cascade);
        }

        public override void Down()
        {
            this.Database.RemoveTable("REGOP_RO_LOAN_TASK");
        }
    }
}