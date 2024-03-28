namespace Bars.Gkh.Gis.Migrations._2015.Version_2015091400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015091400")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015090900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            //старая таблица
            //есть свежая миграция, которая создает эту таблицу по-правильному


            //Справочник услуг из нижних банков данных биллинга 
            //некая свалка данных, полученных из таблиц XXX_kernel.services
            this.Database.AddPersistentObjectTable("BIL_DICT_SERVICE",
                //nzp_serv
                new Column("BILLING_SERVICE_CODE", DbType.Int64, ColumnProperty.NotNull),
                new Column("SERVICE_NAME", DbType.String, 200),
                new Column("MEASURE", DbType.String, 200),
                new RefColumn("SCHEMA_PREFIX_ID", "BIL_DICT_SERVICES__SCHEMA_PREFIX_ID", "BIL_DICT_SCHEMA", "ID")
            );

            this.Database.AddIndex("IND_BIL_DICT_SERVICE__BILLING_SERVICE_CODE", false, "BIL_DICT_SERVICE",
                "BILLING_SERVICE_CODE");
        }

        /// <summary>
        /// Отмена миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("BIL_DICT_SERVICE");
        }
    }
}
