namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014021100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014021001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DEC_CREDIT_ORG", new Column("BANK_ACC_NUM", DbType.String, 100, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("DEC_CREDIT_ORG", "BANK_ACC_NUM");
        }
    }
}