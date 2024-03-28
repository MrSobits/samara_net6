namespace Bars.Gkh.Repair.Migrations.Version_2014022701
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Repair.Migrations.Version_2014022700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Вид работы объекта текущего ремонта
            Database.AddEntityTable(
                "RP_TYPE_WORK",
                new RefColumn("RP_OBJECT_ID", ColumnProperty.NotNull, "RP_TYPE_WORK_RPOBJ", "RP_OBJECT", "ID"),
                new RefColumn("RP_WORK_CRP_ID", ColumnProperty.NotNull, "RP_TYPE_WORK_WK_CRP", "GKH_DICT_WORK_CUR_REPAIR", "ID"),
                new Column("VOLUME", DbType.Decimal),
                new Column("SUM", DbType.Decimal),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime),
                new Column("VOLUME_COMPLETION", DbType.Decimal),
                new Column("PERCENT_COMPLETION", DbType.Decimal),
                new Column("COST_SUM", DbType.Decimal),
                new RefColumn("BUILDER_ID", ColumnProperty.Null, "RP_TYPE_WORK_BUILDER", "GKH_BUILDER", "ID")
                );
            //-----
        }

        public override void Down()
        {
            Database.RemoveTable("RP_TYPE_WORK");
        }
    }
}