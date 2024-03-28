namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014101601
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014101600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_MONEY_LOCK", new Column("SOURCE_NAME", DbType.String, 100));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_MONEY_LOCK", "SOURCE_NAME");
        }
    }
}
