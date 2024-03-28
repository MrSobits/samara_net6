namespace Bars.GkhDi.Migrations.Version_2013032600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013032001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "DI_PERC_CALC",
                new Column("CODE", DbType.String, 50),
                new Column("ACTUAL_VERSION", DbType.Int32),
                new Column("CALC_DATE", DbType.DateTime),
                new Column("PERCENT", DbType.Decimal),
                new Column("POSITION_CNT", DbType.Int32),
                new Column("COMPLETE_POSIT_CNT", DbType.Int32),
                new Column("TYPE_ENTITY_PERC_CALC", DbType.Int32, ColumnProperty.NotNull, 10),
                new RefColumn("PERIOD_DI_ID", "DI_PERC_CALC_PERIOD", "DI_DICT_PERIOD", "ID"));

            Database.AddTable(
                "DI_PERC_DINFO",
                new RefColumn("ID", "DI_PERC_DINFO_ID", "DI_PERC_CALC", "ID"),
                new RefColumn("DIS_INFO_ID", "DI_PERC_CALC_DI", "DI_DISINFO", "ID"));

            Database.AddTable(
               "DI_PERC_REAL_OBJ",
                new RefColumn("ID", "DI_PERC_REAL_OBJ_ID", "DI_PERC_CALC", "ID"),
               new RefColumn("REAL_OBJ_ID", "DI_PERC_CALC_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.AddTable(
               "DI_PERC_SERVICE",
               new RefColumn("ID", "DI_PERC_SERVICE_ID", "DI_PERC_CALC", "ID"),
               new RefColumn("SERVICE_ID", "DI_PERC_CALC_SERV", "DI_BASE_SERVICE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("DI_PERC_CALC");
            Database.RemoveTable("DI_PERC_DINFO");
            Database.RemoveTable("DI_PERC_REAL_OBJ");
            Database.RemoveTable("DI_PERC_SERVICE");
        }
    }
}