namespace Bars.Gkh.Gis.Migrations._2015.Version_2015062201
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015062201")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015032400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "BIL_IMPORT_ADDRESS",
                new RefColumn("FIAS_ADDRESS_UID_ID", "FIAS_ADDRESS_UID_ID_FK", "B4_FIAS_ADDRESS_UID", "ID"),
                new Column("IMPORT_TYPE", DbType.String, 256),
                new Column("IMPORT_FILENAME", DbType.String, 256),
                new Column("ADDRESS_CODE", DbType.String, 20),
                new Column("CITY", DbType.String, 200),
                new Column("STREET", DbType.String, 200),
                new Column("HOUSE", DbType.String, 200),
                new Column("IMPORT_DATE", DbType.DateTime));
        }

        /// <summary>
        /// Отмена миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("BIL_IMPORT_ADDRESS");
        }
    }
}
