//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Bars.Gkh.RegOperator.Migrations._2022.Version_2022060600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022060600")]

    [MigrationDependsOn(typeof(_2021.Version_2021072100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            this.Database.AddColumn("REGOP_DEBTOR", new Column("PROCESSED_BY_AGENT", DbType.Int32, ColumnProperty.None));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_DEBTOR", "PROCESSED_BY_AGENT");
        }
    }
}
