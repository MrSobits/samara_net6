namespace Bars.GkhDi.Regions.Tatarstan.Migrations.Version_1
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("DI_REPAIR_WRK_LIST_TAT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("REASON_REJECTION", DbType.String, 500),
                new Column("INFO_EXEC", DbType.String, 500)
                );

            Database.AddTable("DI_REPAIR_WRK_DET_TAT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("PLAN_VOLUME", DbType.Decimal),
                new Column("FACT_VOLUME", DbType.Decimal)
                );

            this.Database.AddRefColumn("DI_REPAIR_WRK_DET_TAT", new RefColumn("MEASURE_ID", "WORK_LIST_PPR_UM", "GKH_DICT_UNITMEASURE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_REPAIR_WORK_DETAIL", "MEASURE_ID");
            Database.RemoveTable("DI_REPAIR_WRK_DET_TAT");
            Database.RemoveTable("DI_REPAIR_WRK_LIST_TAT");    
        }
    }
}