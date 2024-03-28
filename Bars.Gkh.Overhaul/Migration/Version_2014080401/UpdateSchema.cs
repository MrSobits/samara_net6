namespace Bars.Gkh.Overhaul.Migration.Version_2014080401
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2014080400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_PAYSIZE",
                new Column("INDICATOR", DbType.Int16, ColumnProperty.NotNull, 10),
                new Column("DATE_START", DbType.Date, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.Date));

            Database.AddEntityTable("OVRHL_PAYSIZE_REC",
                new RefColumn("PAYSIZE_ID", ColumnProperty.NotNull, "OVRHL_PAYSIZE_REC_PSZ", "OVRHL_PAYSIZE", "ID"),
                new RefColumn("MU_ID", ColumnProperty.NotNull, "OVRHL_PAYSIZE_REC_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("DVALUE", DbType.Decimal));

            Database.AddEntityTable("OVRHL_PAYSIZE_REC_RET",
                new RefColumn("RET_ID", ColumnProperty.NotNull, "OVRHL_PAYSIZE_REC_RET_RET", "OVRHL_REAL_ESTATE_TYPE", "ID"),
                new RefColumn("RECORD_ID", ColumnProperty.NotNull, "OVRHL_PAYSIZE_REC_RET_REC", "OVRHL_PAYSIZE_REC", "ID"),
                new Column("DVALUE", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PAYSIZE_REC_RET");
            Database.RemoveTable("OVRHL_PAYSIZE_REC");
            Database.RemoveTable("OVRHL_PAYSIZE");
        }
    }
}