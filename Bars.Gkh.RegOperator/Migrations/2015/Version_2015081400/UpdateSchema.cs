namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015081400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015081400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015072900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PERS_ACC_BENEFITS",
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "ROP_PERS_ACC_BEN_P", "REGOP_PERIOD", "ID"),
                new RefColumn("PERS_ACC_ID", ColumnProperty.NotNull, "ROP_PERS_ACC_BEN_ACC", "REGOP_PERS_ACC", "ID"),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0M));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PERS_ACC_BENEFITS");
        }
    }
}
