namespace Bars.GkhDi.Migrations.Version_2013120400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013111100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_REPAIR_SERVICE", new Column("SUM_FACT", DbType.Decimal, ColumnProperty.Null));
            Database.AddColumn("DI_REPAIR_SERVICE", new Column("PROGRESS_INFO", DbType.String, 1000, ColumnProperty.Null));
            Database.AddColumn("DI_REPAIR_SERVICE", new Column("REJECT_CAUSE", DbType.String, 1000, ColumnProperty.Null));
            Database.AddColumn("DI_REPAIR_SERVICE", new Column("DATE_START", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn("DI_REPAIR_SERVICE", new Column("DATE_END", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_REPAIR_SERVICE", "SUM_FACT");
            Database.RemoveColumn("DI_REPAIR_SERVICE", "PROGRESS_INFO");
            Database.RemoveColumn("DI_REPAIR_SERVICE", "REJECT_CAUSE");
            Database.RemoveColumn("DI_REPAIR_SERVICE", "DATE_START");
            Database.RemoveColumn("DI_REPAIR_SERVICE", "DATE_END");
        }
    }
}