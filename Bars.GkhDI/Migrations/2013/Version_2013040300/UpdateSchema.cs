namespace Bars.GkhDi.Migrations.Version_2013040300
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013040300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013032700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Добавил колонку в связи с конвертацией
            Database.AddColumn("DI_DISINFO", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddUniqueConstraint("UNQ_MANORG_PERIOD_DI", "DI_DISINFO", "MANAG_ORG_ID", "PERIOD_DI_ID");

            // Навесил уникальность
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddUniqueConstraint("UNQ_PERIOD_DI_RO", "DI_DISINFO_REALOBJ", "PERIOD_DI_ID", "REALITY_OBJ_ID");
        }

        public override void Down()
        {
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.RemoveConstraint("DI_DISINFO_REALOBJ", "UNQ_PERIOD_DI_RO");
                Database.RemoveConstraint("DI_DISINFO", "UNQ_MANORG_PERIOD_DI");
            }

            Database.RemoveColumn("DI_DISINFO", "EXTERNAL_ID");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "EXTERNAL_ID");
        }
    }
}