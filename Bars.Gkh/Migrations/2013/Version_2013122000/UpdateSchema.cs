namespace Bars.Gkh.Migrations.Version_2013122000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013121800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("SURNAME", DbType.String));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("NAME", DbType.String));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("PATRONYMIC", DbType.String));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("OPEN_ACC_DATE", DbType.DateTime));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("CLOSE_ACC_DATE", DbType.DateTime));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("PERCENTAGE", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("OVRHL_CONTRIB", DbType.Decimal));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("CONTRACT_DATE", DbType.DateTime));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("FIAS_MAIL_ADDRESS_ID", DbType.Int64, 22));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("FIAS_FULL_ADDRESS_ID", DbType.Int64, 22));
            Database.AddIndex("IND_HOUSE_ACCOUNT_MADR", false, "HCS_HOUSE_ACCOUNT", "FIAS_MAIL_ADDRESS_ID");
            Database.AddForeignKey("FK_HOUSE_ACCOUNT_MADR", "HCS_HOUSE_ACCOUNT", "FIAS_MAIL_ADDRESS_ID", "B4_FIAS_ADDRESS", "ID");
            Database.AddIndex("IND_HOUSE_ACCOUNT_FADR", false, "HCS_HOUSE_ACCOUNT", "FIAS_FULL_ADDRESS_ID");
            Database.AddForeignKey("FK_HOUSE_ACCOUNT_FADR", "HCS_HOUSE_ACCOUNT", "FIAS_FULL_ADDRESS_ID", "B4_FIAS_ADDRESS", "ID");
        }
        
        public override void Down()
        {
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "SURNAME");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "NAME");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "PATRONYMIC");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "OPEN_ACC_DATE");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "CLOSE_ACC_DATE");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "PERCENTAGE");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "OVRHL_CONTRIB");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "CONTRACT_DATE");
            Database.RemoveIndex("IND_HOUSE_ACCOUNT_MADR", "HCS_HOUSE_ACCOUNT");
            Database.RemoveConstraint("HCS_HOUSE_ACCOUNT", "FK_HOUSE_ACCOUNT_MADR");
            Database.RemoveIndex("IND_HOUSE_ACCOUNT_FADR", "HCS_HOUSE_ACCOUNT");
            Database.RemoveConstraint("HCS_HOUSE_ACCOUNT", "FK_HOUSE_ACCOUNT_FADR");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "FIAS_MAIL_ADDRESS_ID");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "FIAS_FULL_ADDRESS_ID");
        }
    }
}