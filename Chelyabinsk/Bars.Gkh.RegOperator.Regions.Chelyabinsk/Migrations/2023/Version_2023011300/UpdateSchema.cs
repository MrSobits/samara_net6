using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk._2023.Version_2023011200
{
    [Migration("2023011200")]
    [MigrationDependsOn(typeof(_2022.Version_2022112200.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            Database.AddColumn("AGENT_PIR_DOCUMENT", new Column("AP_DOC_DUTY", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("AGENT_PIR_DOCUMENT", "AP_DOC_DUTY");
        }
    }
}