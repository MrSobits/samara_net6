namespace Bars.Gkh.Gis.Migrations._2015.Version_2015091702
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015091702")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015091701.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {

            //реестр тарифов биллинга
            this.Database.AddPersistentObjectTable("BIL_TARIF_STORAGE",
                //nzp_supp
                new Column("SUPPLIER_CODE", DbType.Int64),
                new Column("SUPPLIER_NAME", DbType.String, 200),

                //nzp_frm
                new Column("FORMULA_TYPE_CODE", DbType.Int64),
                new Column("FORMULA_CODE", DbType.Int64),
                new Column("FORMULA_NAME", DbType.String, 200),

                new Column("TARIF_CODE", DbType.Int64),
                new Column("TARIF_NAME", DbType.String, 200),

                new Column("TARIF_TYPE_CODE", DbType.Int64),
                new Column("TARIF_TYPE_NAME", DbType.String, 200),

                new Column("TARIF_VALUE", DbType.Decimal),
                new Column("TARIF_START_DATE", DbType.Date),
                new Column("TARIF_END_DATE", DbType.Date),

                new Column("LS_COUNT", DbType.Int32),

                new RefColumn("BIL_DICT_SERVICE_ID", "BIL_TARIF_STORAGE__BIL_DICT_SERVICE", "BIL_DICT_SERVICE", "ID"),
                new RefColumn("GKH_REALITY_OBJECT_ID", "BIL_TARIF_STORAGE__GKH_REALITY_OBJECT",
                    "GKH_REALITY_OBJECT", "ID")
            );

            this.Database.AddIndex("IND_BIL_TARIF_STORAGE__SUPPLIER_CODE", false, "BIL_TARIF_STORAGE",
                "SUPPLIER_CODE");
            this.Database.AddIndex("IND_BIL_TARIF_STORAGE__TARIF_START_END_DATE", false, "BIL_TARIF_STORAGE",
                new[] { "BIL_DICT_SERVICE_ID", "TARIF_START_DATE", "TARIF_END_DATE" });
        }

        /// <summary>
        /// Отмена миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("BIL_TARIF_STORAGE");
        }
    }
}
