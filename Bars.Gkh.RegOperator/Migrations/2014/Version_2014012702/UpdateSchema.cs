using System.Data;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014012702
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012702")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014012701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC", "OPEN_DATE", DbType.DateTime, ColumnProperty.Null);
            Database.AddColumn("REGOP_PERS_ACC", "CLOSE_DATE", DbType.DateTime, ColumnProperty.Null);
            Database.AddColumn("REGOP_PERS_ACC", "TARIFF", DbType.Decimal, ColumnProperty.Null);
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "OPEN_DATE");
            Database.RemoveColumn("REGOP_PERS_ACC", "CLOSE_DATE");
            Database.RemoveColumn("REGOP_PERS_ACC", "TARIFF");
        }
    }
}
