namespace Bars.Gkh.Gis.Migrations._2016.Version_2016040200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016040200")]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            //Справочник УО из нижних банков данных биллинга 
            //некая свалка данных, полученных из таблиц ххх_data.s_area, xxx_data.prm_7
            this.Database.AddPersistentObjectTable(
                "BIL_MANORG_STORAGE",

                //nzp_area
                new Column("MANORG_CODE", DbType.Int32, ColumnProperty.NotNull),
                new Column("MANORG_NAME", DbType.String, 150),

                new Column("MANORG_INN", DbType.String, 150),
                new Column("MANORG_KPP", DbType.String, 150),
                new Column("MANORG_OGRN", DbType.String, 150),
                new Column("MANORG_ADDRESS", DbType.String, 150),

                new RefColumn("BIL_DICT_SCHEMA_ID", "BIL_MANORG_STORAGE__BIL_DICT_SCHEMA", "BIL_DICT_SCHEMA", "ID"));
        }

        /// <summary>
        /// Отмена миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("BIL_MANORG_STORAGE");
        }
    }
}