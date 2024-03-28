namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017112700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017112700")]
    [MigrationDependsOn(typeof(Version_2017092600.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017101001.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017101600.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017111000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("CLW_RESTRUCT_SCHEDULE",
                new Column("TOTAL_DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PLANED_PAYMENT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PLANED_PAYMENT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PAYMENT_DATE", DbType.DateTime),
                new Column("PAYMENT_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("IS_EXPIRED", DbType.Boolean, ColumnProperty.NotNull, false),
                new RefColumn("RESTRUCT_DEBT_ID", ColumnProperty.NotNull, "CLW_RESTRUCT_SCHEDULE_RESTRUCT_DEBT", "CLW_RESTRUCT_DEBT", "ID"),
                new RefColumn("PERS_ACC_ID", ColumnProperty.NotNull, "CLW_RESTRUCT_SCHEDULE_PERS_ACC", "REGOP_PERS_ACC", "ID")
            );
        }

        public override void Down()
        {
            this.Database.RemoveTable("CLW_RESTRUCT_SCHEDULE");
        }
    }
}