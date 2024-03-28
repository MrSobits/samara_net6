namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014122000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014122000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014121900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PRIVILEGED_CATEGORY", new Column("LIMIT_AREA", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PRIVILEGED_CATEGORY", "LIMIT_AREA");
        }
    }
}
