namespace Bars.GkhCr.Migrations.Version_2015011400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015011400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2015011200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("CR_COMPETITION", new Column("REVIEW_DATE", DbType.Date));
            Database.ChangeColumn("CR_COMPETITION", new Column("EXEC_DATE", DbType.Date));

            Database.RemoveColumn("CR_COMPETITION", "REVIEW_TIME");
            Database.RemoveColumn("CR_COMPETITION", "EXEC_TIME");
            Database.AddColumn("CR_COMPETITION", new Column("REVIEW_TIME", DbType.DateTime));
            Database.AddColumn("CR_COMPETITION", new Column("EXEC_TIME", DbType.DateTime));

            Database.ChangeColumn("CR_COMPETITION_LOT_BID", new Column("POINTS", DbType.Decimal));
            Database.ChangeColumn("CR_COMPETITION_LOT_BID", new Column("PRICE", DbType.Decimal));
            Database.ChangeColumn("CR_COMPETITION_LOT_BID", new Column("PRICE_NDS", DbType.Decimal)); 
        }

        public override void Down()
        {

        }
    }
}