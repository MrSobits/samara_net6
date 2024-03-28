namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013103102
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013103102")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013103101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Database.AddEntityTable(
            //    "OVRHL_REAL_EST_TYPE_RATE",
            //    new Column("SOCIALLY_ACCEPTABLE_RATE", DbType.Decimal),
            //    new Column("NEED_FOR_FUNDING", DbType.Decimal),
            //    new Column("TOTAL_AREA", DbType.Decimal),
            //    new Column("REASONABLE_RATE", DbType.Decimal),
            //    new Column("RATE_DEFICIT", DbType.Decimal),
            //    new RefColumn("REAL_ESTATE_TYPE_ID", "OVRHL_REALESTTYPERATE_REALEST", "OVRHL_REAL_ESTATE_TYPE", "ID"));
        }

        public override void Down()
        {
            //Database.RemoveEntityTable("OVRHL_REAL_EST_TYPE_RATE");
        }
    }
}