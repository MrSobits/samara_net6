namespace Bars.Gkh.Migrations.Version_2013031900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013031900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveIndex("IND_DICT_ZONAINSP_MUNIC_MCP", "GKH_DICT_ZONAINSP_MUNIC");
            Database.AddIndex("IND_ZONA_MUNIC_MCP", false, "GKH_DICT_ZONAINSP_MUNIC", "MUNICIPALITY_ID");

            Database.AddColumn("CONVERTER_USER_EXTERNAL", new Column("USER_PASSWORD_ID", DbType.String, 36));

            Database.ChangeColumn("GKH_MAN_ORG_SERVICE", new Column("NAME", DbType.String, 250));
            Database.ChangeColumn("GKH_MAN_ORG_MEMBERSHIP", new Column("NAME", DbType.String, 250));
            Database.ChangeColumn("GKH_MAN_ORG_MEMBERSHIP", new Column("OFFICIAL_SITE", DbType.String, 250));

            Database.AddRefColumn("GKH_BUILDER", new RefColumn("FILE_LEARN_PLAN_ID", "GKH_BUILD_LPFILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GKH_BUILDER", new RefColumn("FILE_MANSHEDUL_ID", "GKH_BUILD_MSFILE", "B4_FILE_INFO", "ID"));

            Database.AddColumn("GKH_REALITY_OBJECT", new Column("IS_BUILD_SOC_MORTGAGE", DbType.Int32, 4, ColumnProperty.NotNull, 20));

            //-----Добавил справочник группа конструктивного элемента
            Database.AddEntityTable(
                       "GKH_DICT_CONEL_GROUP",
                       new Column("CODE", DbType.String, 100, ColumnProperty.NotNull),
                       new Column("NAME", DbType.String, 250, ColumnProperty.NotNull),
                       new Column("CLASS", DbType.String, 100),
                       new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_CONEL_GR_NAME", false, "GKH_DICT_CONEL_GROUP", "NAME");
            Database.AddIndex("IND_GKH_CONEL_GR_CODE", false, "GKH_DICT_CONEL_GROUP", "CODE");

            //-----
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "CITY");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "RAION");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "RAION_SELECT");

            //-----
            Database.ChangeColumn("GKH_CONTRAGENT", new Column("OGRN_REG", DbType.String, 300));

        }

        public override void Down()
        {
            Database.RemoveColumn("CONVERTER_USER_EXTERNAL", "USER_PASSWORD_ID");

            Database.RemoveColumn("GKH_BUILDER", "FILE_LEARN_PLAN_ID");
            Database.RemoveColumn("GKH_BUILDER", "FILE_MANSHEDUL_ID");

            Database.RemoveColumn("GKH_REALITY_OBJECT", "IS_BUILD_SOC_MORTGAGE");

            Database.RemoveTable("GKH_DICT_CONEL_GROUP");

            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("CITY", DbType.String, 30));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("RAION", DbType.String, 30));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("RAION_SELECT", DbType.Boolean, ColumnProperty.NotNull, false));
        }
    }
}