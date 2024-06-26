﻿namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014031300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014030601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_SALDO_CHANGE", new Column("GUID", DbType.String, 40));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_SALDO_CHANGE", "GUID");
        }
    }
}
