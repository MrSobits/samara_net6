//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Bars.Gkh.RegOperator.Migrations._2021.Version_2021072100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021072100")]

    [MigrationDependsOn(typeof(_2020.Version_2020091000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            Database.AddEntityTable("REGOP_MOBILE_APP_COMPARSION",
                new Column("PERSONALACCOUNT_NUM", DbType.String, ColumnProperty.None),
                new Column("MOBILEACCOUNT_NUM", DbType.String, ColumnProperty.None),
                new Column("EXTERNALACCOUNT_NUM", DbType.String, ColumnProperty.None),
                new Column("OPERATION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("FIO_ACCOUNT_OWNER", DbType.String, ColumnProperty.None),
                new Column("FIO_MOBILE_APP_ACCOUNT_OWNER", DbType.String, ColumnProperty.None),
                new Column("FIO_SYSTEM_USER", DbType.String, ColumnProperty.None),
                new Column("IS_VIEWED", DbType.Boolean, ColumnProperty.NotNull,false),
                new Column("IS_WORKED_OUT", DbType.Boolean, ColumnProperty.NotNull,false),
                new Column("DECISION_TYPE", DbType.Int32, ColumnProperty.NotNull,1)
                );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_MOBILE_APP_COMPARSION");
        }
    }
}
