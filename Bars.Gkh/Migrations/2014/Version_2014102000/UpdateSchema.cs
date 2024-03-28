namespace Bars.Gkh.Migrations.Version_2014102000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014101700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            
            if (!Database.TableExists("GKH_DICT_MULTIGLOSSARY"))
            {
                //-----Универсальный справочник
                // Таблица перенесена из модуля GKH1468, условие для обратной совместимости
                Database.AddEntityTable("GKH_DICT_MULTIGLOSSARY",
                    new Column("CODE", DbType.String, 200, ColumnProperty.NotNull),
                    new Column("NAME", DbType.String, 2000, ColumnProperty.NotNull));
                Database.AddIndex("IND_GKH_DICT_MGLOSS_CODE", true, "GKH_DICT_MULTIGLOSSARY", "CODE");
            }

            if (!Database.TableExists("GKH_DICT_MULTIITEM"))
            {
                // Таблица перенесена из модуля GKH1468. Условие создано для обратной совместимости
                Database.AddEntityTable("GKH_DICT_MULTIITEM",
                    new RefColumn("GLOSSARY_ID", ColumnProperty.NotNull, "GKH_DICT_MULTIITEM_G", "GKH_DICT_MULTIGLOSSARY", "ID"),
                    new Column("KEY", DbType.String, 200, ColumnProperty.NotNull),
                    new Column("VALUE", DbType.String, 2000, ColumnProperty.NotNull));
            } 
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_DICT_MULTIITEM");

            Database.RemoveIndex("IND_GKH_DICT_MGLOSS_CODE", "GKH_DICT_MULTIGLOSSARY");
            Database.RemoveTable("GKH_DICT_MULTIGLOSSARY");
        }
    }
}