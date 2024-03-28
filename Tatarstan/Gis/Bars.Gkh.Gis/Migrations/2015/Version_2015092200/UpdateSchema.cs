namespace Bars.Gkh.Gis.Migrations._2015.Version_2015092200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015092200")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015092100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddPersistentObjectTable("BIL_NORMATIVE_STORAGE",
                new RefColumn("BIL_DICT_SERVICE_ID", "BIL_NORMATIVE_STORAGE__BIL_DICT_SERVICE", "BIL_DICT_SERVICE", "ID"),
                new Column("NORMATIVE_TYPE_CODE", DbType.Int64),
                new Column("NORMATIVE_TYPE_NAME", DbType.String, 200),
                new Column("NORMATIVE_CODE", DbType.Int64),
                new Column("NORMATIVE_NAME", DbType.String, 200),
                new Column("NORMATIVE_DESCRIPTION", DbType.String, 200),
                //в следующей миграции это поле будет изменено на String
                new Column("NORMATIVE_VALUE", DbType.Decimal),
                new Column("NORMATIVE_START_DATE", DbType.DateTime),
                new Column("NORMATIVE_END_DATE", DbType.DateTime));

            this.Database.AddIndex("IND_BIL_NORMATIVE_STORAGE__NORMTYPE_CODE", false, "BIL_NORMATIVE_STORAGE",
                "NORMATIVE_TYPE_CODE");
            this.Database.AddIndex("IND_BIL_NORMATIVE_STORAGE__NORM_CODE", false, "BIL_NORMATIVE_STORAGE",
                "NORMATIVE_CODE");
        }

        public override void Down()
        {
            this.Database.RemoveTable("BIL_NORMATIVE_STORAGE");
        }
    }
}