using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk._2020.Version_2020092900
{
    [Migration("2020092900")]
    [MigrationDependsOn(typeof(_2020.Version_2020092800.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("LACK_OF_PPROPERTY_ACT", DbType.Boolean));
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("LACK_OF_PPROPERTY_ACT_DATE", DbType.DateTime));
            
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "LACK_OF_PPROPERTY_ACT");
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "LACK_OF_PPROPERTY_ACT_DATE");
        }
    }
}