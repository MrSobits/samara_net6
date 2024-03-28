namespace Bars.Gkh.Gis.Migrations._2015.Version_2015091701
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015091701")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015091700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.RemoveIndex("IND_BIL_DICT_SERVICE__BILLING_SERVICE_CODE", "BIL_DICT_SERVICE");

            this.Database.RemoveTable("BIL_DICT_SERVICE");

            //Справочник услуг из нижних банков данных биллинга 
            //некая свалка данных, полученных из таблиц XXX_kernel.services
            this.Database.AddPersistentObjectTable("BIL_DICT_SERVICE",
                //nzp_serv
                new Column("SERVICE_CODE", DbType.Int64, ColumnProperty.NotNull),
                new Column("SERVICE_NAME", DbType.String, 200),


                new Column("SERVICE_TYPE_CODE", DbType.Int64),
                new Column("SERVICE_TYPE_NAME", DbType.String, 200),

                //nzp_measure
                new Column("MEASURE_CODE", DbType.Int64),
                new Column("MEASURE_NAME", DbType.String, 200),

                new RefColumn("BIL_DICT_SCHEMA_ID", "BIL_DICT_SERVICE__BIL_DICT_SCHEMA", "BIL_DICT_SCHEMA", "ID"),
                new RefColumn("GIS_SERVICE_DICTIONARY_ID", "BIL_DICT_SERVICE__GIS_SERVICE_DICTIONARY",
                    "GIS_SERVICE_DICTIONARY", "ID")
            );

            this.Database.AddIndex("IND_BIL_DICT_SERVICE__SERVICE_CODE", false, "BIL_DICT_SERVICE",
                "SERVICE_CODE");
        }

        /// <summary>
        /// Отмена миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveIndex("IND_BIL_DICT_SERVICE__SERVICE_CODE", "BIL_DICT_SERVICE");

            this.Database.RemoveTable("BIL_DICT_SERVICE");

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
    }
}