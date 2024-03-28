using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Regions.Voronezh.Migrations._2019.Version_2019122400
{
    [Migration("20191224800")]
    [MigrationDependsOn(typeof(_2019.Version_2019102800.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_DUTY_PAY_FORC", DbType.Decimal));
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_PENALTY_PAY_FORC", DbType.Decimal));
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_TR__PAY_FORC", DbType.Decimal));
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_BT_PAY_FORC", DbType.Decimal));
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_DUTY_PAY_VOL", DbType.Decimal));
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_PENALTY_PAY_VOL", DbType.Decimal));
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_TR__PAY_VOL", DbType.Decimal));
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_BT_PAY_VOL", DbType.Decimal));
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_DUTY", DbType.Decimal));
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_DEBT_DECISION_SUM", DbType.Decimal));
            

        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_DUTY_PAY_FORC");
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_PENALTY_PAY_FORC");
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_TR__PAY_FORC");
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_BT_PAY_FORC");
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_DUTY_PAY_VOL");
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_PENALTY_PAY_VOL");
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_TR__PAY_VOL");
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_BT_PAY_VOL");
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_DUTY");
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_DEBT_DECISION_SUM");
        }
    }
}