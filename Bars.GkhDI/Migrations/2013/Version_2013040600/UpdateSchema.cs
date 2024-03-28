namespace Bars.GkhDi.Migrations.Version_2013040600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013040600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013040400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "DI_ARCH_PERC_CALC",
               new Column("CODE", DbType.String, 50),
               new Column("ACTUAL_VERSION", DbType.Int32),
               new Column("CALC_DATE", DbType.DateTime),
               new Column("PERCENT", DbType.Decimal),
               new Column("POSITION_CNT", DbType.Int32),
               new Column("COMPLETE_POSIT_CNT", DbType.Int32),
               new Column("TYPE_ENTITY_PERC_CALC", DbType.Int32, ColumnProperty.NotNull, 10),
               new RefColumn("PERIOD_DI_ID", "DI_ARCH_PERC_PERIOD", "DI_DICT_PERIOD", "ID"));

            Database.AddTable(
                "DI_ARCH_PERC_DINFO",
                new RefColumn("ID", "DI_ARCH_PERC_DI_ID", "DI_PERC_CALC", "ID"),
                new RefColumn("DIS_INFO_ID", "DI_ARCH_PERC_DI", "DI_DISINFO", "ID"));

            Database.AddTable(
               "DI_ARCH_PERC_REAL_OBJ",
                new RefColumn("ID", "DI_ARCH_PERC_RO_ID", "DI_PERC_CALC", "ID"),
                new RefColumn("REAL_OBJ_ID", "DI_ARCH_PERC_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.AddIndex("IND_DI_ARCH_PERC_CODE", false, "DI_ARCH_PERC_CALC", "CODE");
            Database.AddIndex("IND_DI_ARCH_PERC_DATE", false, "DI_ARCH_PERC_CALC", "CALC_DATE");

            Database.RemoveColumn("DI_PERC_CALC", "PERIOD_DI_ID");
        }

        public override void Down()
        {
            Database.RemoveTable("DI_ARCH_PERC_CALC");
            Database.RemoveTable("DI_ARCH_PERC_DINFO");
            Database.RemoveTable("DI_ARCH_PERC_REAL_OBJ");
        }
    }
}