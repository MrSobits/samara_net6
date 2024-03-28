using Bars.B4.Modules.Ecm7.Framework;
using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk._2023.Version_2023092200
{
    [Migration("2023092200")]
    [MigrationDependsOn(typeof(Version_2023031400.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            Database.AddColumn("AGENT_PIR_DEBTOR", new Column("DEBT_CALC", DbType.Int16));
            Database.AddColumn("AGENT_PIR_DEBTOR", new Column("PENALTY_CHARGE", DbType.Int16));

            Database.AddColumn("AGENT_PIR_DEBTOR_REFERENCE_CALCULATION", new Column("TARIF_DEBT_PAY", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("AGENT_PIR_DEBTOR_REFERENCE_CALCULATION", "TARIF_DEBT_PAY");

            Database.RemoveColumn("AGENT_PIR_DEBTOR", "PENALTY_CHARGE");
            Database.RemoveColumn("AGENT_PIR_DEBTOR", "DEBT_CALC");
        }
    }
}