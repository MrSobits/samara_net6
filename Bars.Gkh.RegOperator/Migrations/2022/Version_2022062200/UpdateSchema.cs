//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Bars.Gkh.RegOperator.Migrations._2022.Version_2022062200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022062200")]

    [MigrationDependsOn(typeof(_2022.Version_2022060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            this.Database.AddColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", new Column("PENALTIES", DbType.Decimal, ColumnProperty.None));
            this.Database.AddColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", new Column("ACCRUAL_PENALTIES", DbType.String, ColumnProperty.None));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", "PENALTIES");
            this.Database.RemoveColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", "ACCRUAL_PENALTIES");
        }
    }
}
