using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk._2022.Version_2022071100
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022071100")]
    [MigrationDependsOn(typeof(_2022.Version_2022070700.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
       
        public override void Up()
        {
            Database.AddEntityTable("AGENT_PIR_DEBTOR",
                   new Column("AGENT_PIR_DEBTOR_STATUS",DbType.Int32),
                   new RefColumn("AGENT_PIR_ID", "AGENT_PIR_DEBTOR_AGENT_PIR", "AGENT_PIR", "ID"),
                   new RefColumn("AGENT_PIR_DEBTOR_PA_ID", "AGENT_PIR_DEBTOR_PA_REGOP_PERS_ACC", "REGOP_PERS_ACC", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("AGENT_PIR_DEBTOR");            
        }
    }
}