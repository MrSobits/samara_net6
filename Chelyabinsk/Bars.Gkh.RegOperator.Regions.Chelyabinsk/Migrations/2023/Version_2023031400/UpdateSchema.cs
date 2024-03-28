using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk._2023.Version_2023031400
{
    [Migration("2023031400")]
    [MigrationDependsOn(typeof(_2023.Version_2023013100.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            Database.AddColumn("AGENT_PIR_DEBTOR", new RefColumn("ORDERING_FILE_ID", "ORDERING_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("AGENT_PIR_DEBTOR", "ORDERING_FILE_ID");
        }
    }
}