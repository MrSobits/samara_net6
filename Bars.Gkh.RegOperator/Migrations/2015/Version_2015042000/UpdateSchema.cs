namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015042000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015042000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015040800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("RD_STATUS", DbType.Int32, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "RD_STATUS");
        }
    }
}
