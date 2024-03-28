//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Bars.Gkh.RegOperator.Migrations._2022.Version_2022072000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022072000")]

    [MigrationDependsOn(typeof(_2022.Version_2022062200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            this.Database.AddColumn("REGOP_DEBTOR", new Column("claim_work_id", DbType.Int64, ColumnProperty.None));
            this.Database.AddColumn("REGOP_DEBTOR", new Column("LASTCLW_DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull,0));
            this.Database.AddColumn("REGOP_DEBTOR", new Column("PAYMENTS_SUM", DbType.Decimal, ColumnProperty.NotNull, 0));
            this.Database.AddColumn("REGOP_DEBTOR", new Column("NEW_CLAIM_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0));
            this.Database.AddColumn("REGOP_DEBTOR", new Column("LAST_DEBT_PERIOD", DbType.String, ColumnProperty.None));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_DEBTOR", "LAST_DEBT_PERIOD");
            this.Database.RemoveColumn("REGOP_DEBTOR", "NEW_CLAIM_DEBT");
            this.Database.RemoveColumn("REGOP_DEBTOR", "PAYMENTS_SUM");
            this.Database.RemoveColumn("REGOP_DEBTOR", "LASTCLW_DEBT_SUM");
            this.Database.RemoveColumn("REGOP_DEBTOR", "claim_work_id");
        }
    }
}
