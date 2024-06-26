﻿namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014060700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014060400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC", new Column("CONTRACT_SEND_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "CONTRACT_SEND_DATE");
        }
    }
}
