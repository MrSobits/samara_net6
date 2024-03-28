namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015093000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015093000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015091800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("REGOP_PRIVILEGED_CATEGORY", new Column("NAME", DbType.String, 300));
            Database.ChangeColumn("REGOP_PRIVILEGED_CATEGORY", new Column("CODE", DbType.String, 300));
        }

        public override void Down()
        {

        }
    }
}
