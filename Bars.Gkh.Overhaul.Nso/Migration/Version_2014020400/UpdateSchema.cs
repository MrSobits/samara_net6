namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014020400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014011701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("OVRHL_MISS_DPKR_REC",
               new RefColumn("RO_ID", ColumnProperty.NotNull, "MISS_DPKR_REC_RO", "GKH_REALITY_OBJECT", "ID"),
               new Column("YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
               new Column("CEO_STRING", DbType.String, 4000, ColumnProperty.Null),
               new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
               new Column("REAL_EST_TYPE_NAME", DbType.String, 300, ColumnProperty.NotNull),
               new Column("MARG_REPAIR_COST", DbType.Decimal, ColumnProperty.NotNull),
               new Column("AREA_MKD", DbType.Decimal));
        }

        public override void Down()
        {
            this.Database.RemoveTable("OVRHL_MISS_DPKR_REC");
        }
    }
}