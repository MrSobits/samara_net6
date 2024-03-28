namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014040100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014032701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC", new Column("CAREA", DbType.Decimal, ColumnProperty.NotNull,0));
            Database.AddColumn("REGOP_PERS_ACC", new Column("LAREA", DbType.Decimal, ColumnProperty.NotNull,0));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "CAREA");
            Database.RemoveColumn("REGOP_PERS_ACC", "LAREA");
        }
    }
}
