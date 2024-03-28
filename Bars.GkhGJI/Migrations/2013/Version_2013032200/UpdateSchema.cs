namespace Bars.GkhGji.Migrations.Version_2013032200
{
    using System;
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013032001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_DICT_KIND_CHECK",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddIndex("IND_GJI_KINDCH_C", false, "GJI_DICT_KIND_CHECK", "CODE");
            Database.AddIndex("IND_GJI_KINDCH_N", false, "GJI_DICT_KIND_CHECK", "NAME");

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                var pgColumns = new[] { "OBJECT_VERSION", "OBJECT_CREATE_DATE", "OBJECT_EDIT_DATE", "NAME", "CODE", "DESCRIPTION" };

                Database.Insert("GJI_DICT_KIND_CHECK", pgColumns, new[] { "0", DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.ToString("dd.MM.yyyy"), "Плановая выездная", "1", "Плановая выездная" });
                Database.Insert("GJI_DICT_KIND_CHECK", pgColumns, new[] { "0", DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.ToString("dd.MM.yyyy"), "Внеплановая выездная", "2", "Внеплановая выездная" });
                Database.Insert("GJI_DICT_KIND_CHECK", pgColumns, new[] { "0", DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.ToString("dd.MM.yyyy"), "Плановая документарная", "3", "Плановая документарная" });
                Database.Insert("GJI_DICT_KIND_CHECK", pgColumns, new[] { "0", DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.ToString("dd.MM.yyyy"), "Внеплановая документарная", "4", "Внеплановая документарная" });
                Database.Insert("GJI_DICT_KIND_CHECK", pgColumns, new[] { "0", DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.ToString("dd.MM.yyyy"), "Инспекционная", "5", "Инспекционная" });
                Database.Insert("GJI_DICT_KIND_CHECK", pgColumns, new[] { "0", DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.ToString("dd.MM.yyyy"), "Мониторинг", "6", "Мониторинг" });
                Database.Insert("GJI_DICT_KIND_CHECK", pgColumns, new[] { "0", DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.ToString("dd.MM.yyyy"), "Плановая документарная и выездная", "7", "Плановая документарная и выездная" });
                Database.Insert("GJI_DICT_KIND_CHECK", pgColumns, new[] { "0", DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.ToString("dd.MM.yyyy"), "Визуальное обследование", "8", "Визуальное обследование" });
                Database.Insert("GJI_DICT_KIND_CHECK", pgColumns, new[] { "0", DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.ToString("dd.MM.yyyy"), "Внеплановая документарная и выездная", "9", "Внеплановая документарная и выездная" });
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                var oraColumns = new[] { "ID", "OBJECT_VERSION", "OBJECT_CREATE_DATE", "OBJECT_EDIT_DATE", "NAME", "CODE", "DESCRIPTION" };

                Database.Insert("GJI_DICT_KIND_CHECK", oraColumns, new[] { "1", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "Плановая выездная", "1", "Плановая выездная" });
                Database.Insert("GJI_DICT_KIND_CHECK", oraColumns, new[] { "2", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "Внеплановая выездная", "2", "Внеплановая выездная" });
                Database.Insert("GJI_DICT_KIND_CHECK", oraColumns, new[] { "3", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "Плановая документарная", "3", "Плановая документарная" });
                Database.Insert("GJI_DICT_KIND_CHECK", oraColumns, new[] { "4", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "Внеплановая документарная", "4", "Внеплановая документарная" });
                Database.Insert("GJI_DICT_KIND_CHECK", oraColumns, new[] { "5", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "Инспекционная", "5", "Инспекционная" });
                Database.Insert("GJI_DICT_KIND_CHECK", oraColumns, new[] { "6", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "Мониторинг", "6", "Мониторинг" });
                Database.Insert("GJI_DICT_KIND_CHECK", oraColumns, new[] { "7", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "Плановая документарная и выездная", "7", "Плановая документарная и выездная" });
                Database.Insert("GJI_DICT_KIND_CHECK", oraColumns, new[] { "8", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "Визуальное обследование", "8", "Визуальное обследование" });
                Database.Insert("GJI_DICT_KIND_CHECK", oraColumns, new[] { "9", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "Внеплановая документарная и выездная", "9", "Внеплановая документарная и выездная" });
            }

            //-----изменение колонки в таблице связи типа обследования и вида обследования
            Database.AddColumn("GJI_DICT_KIND_INSPECTION", new Column("KIND_CHECK_ID", DbType.Int64, 22, ColumnProperty.NotNull, 1));

            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "1" }, "TYPE_CHECK=10");
            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "2" }, "TYPE_CHECK=20");
            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "3" }, "TYPE_CHECK=30");
            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "4" }, "TYPE_CHECK=40");
            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "5" }, "TYPE_CHECK=50");
            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "8" }, "TYPE_CHECK=60");

            Database.RemoveColumn("GJI_DICT_KIND_INSPECTION", "TYPE_CHECK");

            Database.AddIndex("IND_GJI_KIND_INS_KC", false, "GJI_DICT_KIND_INSPECTION", "KIND_CHECK_ID");
            Database.AddForeignKey("FK_GJI_KIND_INS_KC", "GJI_DICT_KIND_INSPECTION", "KIND_CHECK_ID", "GJI_DICT_KIND_CHECK", "ID");
            //-----
        }

        public override void Down()
        {
            //-----проставляем обратно типы проверок
            Database.AddColumn("GJI_DICT_KIND_INSPECTION", new Column("TYPE_CHECK", DbType.Int32, 4, ColumnProperty.NotNull, 10));

            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "10" }, "KIND_CHECK_ID=1");
            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "20" }, "KIND_CHECK_ID=2");
            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "30" }, "KIND_CHECK_ID=3");
            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "40" }, "KIND_CHECK_ID=4");
            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "50" }, "KIND_CHECK_ID=5");
            Database.Update("GJI_DICT_KIND_INSPECTION", new[] { "KIND_CHECK_ID" }, new[] { "60" }, "KIND_CHECK_ID=8");

            Database.RemoveConstraint("GJI_DICT_KIND_INSPECTION", "FK_GJI_KIND_INS_KC");
            Database.RemoveIndex("IND_GJI_KIND_INS_KC", "GJI_DICT_KIND_INSPECTION");
            Database.RemoveColumn("GJI_DICT_KIND_INSPECTION", "KIND_CHECK_ID");

            Database.AddIndex("IND_GJI_KIND_INS_TC", false, "GJI_DICT_KIND_INSPECTION", "TYPE_CHECK");
            //-----

            Database.RemoveTable("GJI_DICT_KIND_CHECK");
        }
    }
}