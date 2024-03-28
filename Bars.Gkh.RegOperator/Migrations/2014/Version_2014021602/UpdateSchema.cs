namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021602
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_RO_LOAN", new Column("PLAN_MONTH_COUNT", DbType.Int32, ColumnProperty.NotNull, "0"));
            Database.RemoveColumn("REGOP_RO_LOAN", "PLAN_END_DATE");
        }

        public override void Down()
        {
            Database.AddColumn("REGOP_RO_LOAN", new Column("PLAN_END_DATE", DbType.AnsiString));
            Database.RemoveColumn("REGOP_RO_LOAN", "PLAN_MONTH_COUNT");
        }
    }
}
