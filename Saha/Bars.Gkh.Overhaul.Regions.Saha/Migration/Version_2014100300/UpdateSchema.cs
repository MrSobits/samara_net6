namespace Bars.Gkh.Overhaul.Regions.Saha.Migration.Version_2014100300
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100300")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_REAL_EST_TYPE_RATE", new Column("SOCIALLY_ACCEPTABLE_RATE_NOTLIV", DbType.Decimal));
            Database.AddColumn("OVRHL_REAL_EST_TYPE_RATE", new Column("TOTAL_AREA_NOTLIV", DbType.Decimal));
            Database.AddColumn("OVRHL_REAL_EST_TYPE_RATE", new Column("REASONABLE_RATE_NOTLIV", DbType.Decimal));
            Database.AddColumn("OVRHL_REAL_EST_TYPE_RATE", new Column("RATE_DEFICIT_NOTLIV", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_REAL_EST_TYPE_RATE", "SOCIALLY_ACCEPTABLE_RATE_NOTLIV");
            Database.RemoveColumn("OVRHL_REAL_EST_TYPE_RATE", "TOTAL_AREA_NOTLIV");
            Database.RemoveColumn("OVRHL_REAL_EST_TYPE_RATE", "REASONABLE_RATE_NOTLIV");
            Database.RemoveColumn("OVRHL_REAL_EST_TYPE_RATE", "RATE_DEFICIT_NOTLIV");
        }
    }
}