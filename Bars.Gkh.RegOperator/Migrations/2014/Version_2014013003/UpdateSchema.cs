﻿namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014013003
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014013003")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014013002.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_CHARGE", new Column("IS_FIXED", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_CHARGE", "IS_FIXED");
        }
    }
}
