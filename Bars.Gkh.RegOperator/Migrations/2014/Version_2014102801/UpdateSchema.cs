﻿namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014102801
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014102701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_CHARGE", new Column("RECALC_DECISION", DbType.Decimal, ColumnProperty.NotNull, "0"));
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("RECALC_DECISION", DbType.Decimal, ColumnProperty.NotNull, "0"));
            Database.AddColumn("REGOP_UNACCEPT_CHARGE", new Column("RECALC_DECISION", DbType.Decimal, ColumnProperty.NotNull, "0"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "RECALC_DECISION");
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "RECALC_DECISION");
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "RECALC_DECISION");
        }
    }
}
