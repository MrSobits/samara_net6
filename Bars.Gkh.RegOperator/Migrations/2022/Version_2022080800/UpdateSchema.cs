//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Bars.Gkh.RegOperator.Migrations._2022.Version_2022080800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022080800")]

    [MigrationDependsOn(typeof(_2022.Version_2022072000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            this.Database.AddColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", new Column("PENALTY_PAYMENT", DbType.Decimal, ColumnProperty.None));
            this.Database.AddColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", new Column("PENALTY_PAYMENT_DATE", DbType.String, ColumnProperty.None));
            this.Database.ExecuteQuery("UPDATE REGOP_LAWSUIT_REFERENCE_CALCULATION SET PENALTY_PAYMENT = 0");
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", "PENALTY_PAYMENT_DATE");
            this.Database.RemoveColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", "PENALTY_PAYMENT");
        }
    }
}
