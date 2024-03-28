namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014021001
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014021000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveConstraint("DEC_ACCOUNT_OWNER", "FK_ACC_OWNER_MANORG");
            Database.RemoveColumn("DEC_ACCOUNT_OWNER", "MANORG_ID");

            Database.AddColumn("DEC_ACCOUNT_OWNER", new Column("DECISION_TYPE", DbType.Int32, ColumnProperty.NotNull));
            Database.AddRefColumn("DEC_ACCOUNT_OWNER", new RefColumn("CONTRAGENT_ID", "DEC_ACC_OWNER_CONTR", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("DEC_ACCOUNT_OWNER", "CONTRAGENT_ID");
            Database.RemoveColumn("DEC_ACCOUNT_OWNER", "DECISION_TYPE");
            Database.AddColumn("DEC_ACCOUNT_OWNER", new Column("MANORG_ID", DbType.String, ColumnProperty.Null));
        }
    }
}