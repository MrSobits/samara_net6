namespace Bars.Gkh.Migrations.Version_2013122600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013122402.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("OWNER_TYPE", DbType.Int32, ColumnProperty.NotNull, 20));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("OWNER_NAME", DbType.String));

            Database.ExecuteNonQuery("UPDATE hcs_house_account SET OWNER_NAME = surname  || ' ' || name || ' ' || patronymic");

            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "SURNAME");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "NAME");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "PATRONYMIC");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "OVRHL_CONTRIB");
        }

        public override void Down()
        {
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "OWNER_TYPE");
            Database.RemoveColumn("HCS_HOUSE_ACCOUNT", "OWNER_NAME");

            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("SURNAME", DbType.String));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("NAME", DbType.String));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("PATRONYMIC", DbType.String));
            Database.AddColumn("HCS_HOUSE_ACCOUNT", new Column("OVRHL_CONTRIB", DbType.Decimal));
        }
    }
}