namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014073099
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014073099")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014073000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PAYSIZE",
                new Column("DATE_START", DbType.Date, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.Date));

            Database.AddEntityTable("REGOP_PAYSIZE_REC",
                new RefColumn("PAYSIZE_ID", ColumnProperty.NotNull, "REGOP_PAYSIZE_REC_PSZ", "REGOP_PAYSIZE", "ID"),
                new RefColumn("MU_ID", ColumnProperty.NotNull, "REGOP_PAYSIZE_REC_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("DVALUE", DbType.Decimal));

            Database.AddEntityTable("REGOP_PAYSIZE_REC_RET",
                new RefColumn("RET_ID", ColumnProperty.NotNull, "REGOP_PAYSIZE_REC_RET_RET", "OVRHL_REAL_ESTATE_TYPE", "ID"),
                new RefColumn("RECORD_ID", ColumnProperty.NotNull, "REGOP_PAYSIZE_REC_RET_REC", "REGOP_PAYSIZE_REC", "ID"),
                new Column("DVALUE", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PAYSIZE_REC_RET");
            Database.RemoveTable("REGOP_PAYSIZE_REC");
            Database.RemoveTable("REGOP_PAYSIZE");
        }
    }
}
