namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014031802
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031802")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014031801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_CALC_PARAM_TRACE", new Column("DATE_END", DbType.Date));

            Database.RenameColumn("REGOP_CALC_PARAM_TRACE", "USED_DATE", "DATE_START");
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_CALC_PARAM_TRACE", "DATE_END");

            Database.RenameColumn("REGOP_CALC_PARAM_TRACE", "DATE_START", "USED_DATE");
        }
    }
}
