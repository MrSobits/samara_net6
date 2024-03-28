namespace Bars.Gkh.Migrations._2015.Version_2015021600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015021600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015021300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_CLAIM_WORK", new Column("IS_DEBT_PAID", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("CLW_CLAIM_WORK", new Column("DEBT_PAID_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_CLAIM_WORK", "IS_DEBT_PAID");
            Database.RemoveColumn("CLW_CLAIM_WORK", "DEBT_PAID_DATE");
        }
    }
}