﻿namespace Bars.GkhCr.Migrations.Version_2014060300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014060200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_BUILD_CONTRACT", new Column("BUDGET_MO", DbType.Decimal));
            Database.AddColumn("CR_OBJ_BUILD_CONTRACT", new Column("BUDGET_SUBJ", DbType.Decimal));
            Database.AddColumn("CR_OBJ_BUILD_CONTRACT", new Column("OWNER_MEANS", DbType.Decimal));
            Database.AddColumn("CR_OBJ_BUILD_CONTRACT", new Column("FUND_MEANS", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "BUDGET_MO");
            Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "BUDGET_SUBJ");
            Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "OWNER_MEANS");
            Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "FUND_MEANS");
        }
    }
}