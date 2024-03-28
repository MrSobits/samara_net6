namespace Bars.Gkh.Repair.Migrations.Version_2014022702
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022702")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Repair.Migrations.Version_2014022701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "RP_TYPE_WORK_ARCH",
                new RefColumn("REPAIR_WORK_ID", ColumnProperty.NotNull, "RP_WORK_ARCH_RW", "RP_TYPE_WORK", "ID"),
                new Column("PERCENT_COMPLETION", DbType.Decimal),
                new Column("COST_SUM", DbType.Decimal),
                new Column("VOLUME_COMPLETION", DbType.Decimal),
                new Column("DATE_CHANGE_REC", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveTable("RP_TYPE_WORK_ARCH");
        }
    }
}