
using System.Data;

namespace Bars.GkhCr.Migrations.Version_2014041500
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014040900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_FIN_SOURCE_RES", new Column("BUDGET_MU_INCOME", DbType.Decimal));
            Database.AddColumn("CR_OBJ_FIN_SOURCE_RES", new Column("BUDGET_SUB_INCOME", DbType.Decimal));
            Database.AddColumn("CR_OBJ_FIN_SOURCE_RES", new Column("FUND_RES_INCOME", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_FIN_SOURCE_RES", "BUDGET_MU_INCOME");
            Database.RemoveColumn("CR_OBJ_FIN_SOURCE_RES", "BUDGET_SUB_INCOME");
            Database.RemoveColumn("CR_OBJ_FIN_SOURCE_RES", "FUND_RES_INCOME");
        }
    }
}