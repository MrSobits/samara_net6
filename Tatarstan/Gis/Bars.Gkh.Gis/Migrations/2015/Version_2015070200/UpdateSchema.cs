namespace Bars.Gkh.Gis.Migrations._2015.Version_2015070200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015070200")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015062201.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            //Справочник префиксов схем баз данных биллинга
            this.Database.AddPersistentObjectTable("BIL_DICT_SCHEMA",

                new Column("LOCAL_SCHEMA_PREFIX", DbType.String, 50, ColumnProperty.NotNull),
                new Column("CENTRAL_SCHEMA_PREFIX ", DbType.String, 50, ColumnProperty.NotNull),
                new Column("CONNECTION_STRING", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 255),
                new Column("IS_ACTIVE", DbType.Int16, ColumnProperty.NotNull));

            //индексная таблица для доступа к домам по коду дома МЖФ
            this.Database.AddPersistentObjectTable("BIL_HOUSE_CODE_STORAGE",
                new Column("BILLING_HOUSE_CODE", DbType.Int64, ColumnProperty.NotNull),
                new RefColumn("SCHEMA_PREFIX_ID", "BIL_HOUSE_CODE_STORAGE__SCHEMA_PREFIX_ID", "BIL_DICT_SCHEMA", "ID"));

            this.Database.AddIndex("IND_BIL_HOUSE_CODE_STORAGE__BILLING_HOUSE_CODE", true, "BIL_HOUSE_CODE_STORAGE",
                "BILLING_HOUSE_CODE");
            this.Database.AddIndex("IND_BIL_BIL_DICT_SCHEMA__LOCAL_SCHEMA_PREFIX", true, "BIL_DICT_SCHEMA",
                "LOCAL_SCHEMA_PREFIX");
        }

        /// <summary>
        /// Отмена миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("BIL_DICT_SCHEMA");
            this.Database.RemoveTable("BIL_HOUSE_CODE_STORAGE");
        }
    }
}
