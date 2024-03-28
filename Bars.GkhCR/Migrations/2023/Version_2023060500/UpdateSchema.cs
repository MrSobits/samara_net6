using Bars.B4.Modules.Ecm7.Framework;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023060500
{
    [Migration("2023060500")]
    [MigrationDependsOn(typeof(_2023.Version_2023050500.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", new Column("FACT_VOLUME", DbType.Decimal));       
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "FACT_VOLUME");        
        }
    }
}