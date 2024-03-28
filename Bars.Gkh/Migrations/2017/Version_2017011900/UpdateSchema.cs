namespace Bars.Gkh.Migrations._2017.Version_2017011900
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция создания таблиц техпаспорта жилого дома
    /// </summary>
    [Migration("2017011900")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2017.Version_2017011100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            if (!this.Database.TableExists("TP_TEH_PASSPORT"))
            {
                //-----Технический паспорт
                this.Database.AddEntityTable(
                    "TP_TEH_PASSPORT",
                    new RefColumn("REALITY_OBJ_ID", ColumnProperty.NotNull, "TP_TEH_PASSPORT_RO", "GKH_REALITY_OBJECT", "ID"),
                    new Column("EXTERNAL_ID", DbType.String, 36));
            }

            if (!this.Database.TableExists("TP_TEH_PASSPORT_VALUE"))
            {
                //-----Значение техпаспорта
                this.Database.AddEntityTable(
                    "TP_TEH_PASSPORT_VALUE",
                    new RefColumn("TEH_PASSPORT_ID", ColumnProperty.NotNull, "TP_TEH_PASSPORT_VALUE", "TP_TEH_PASSPORT", "ID"),
                    new Column("FORM_CODE", DbType.String, 250, ColumnProperty.NotNull),
                    new Column("CELL_CODE", DbType.String, 250, ColumnProperty.NotNull),
                    new Column("VALUE", DbType.String, 250),
                    new Column("EXTERNAL_ID", DbType.String, 36));

                //-----
                this.Database.AddIndex("IND_TEH_PASS_COMPOSITE_KEY", true, "TP_TEH_PASSPORT_VALUE", "TEH_PASSPORT_ID", "FORM_CODE", "CELL_CODE");
            }
        }

        /// <inheritdoc />
        public override void Down()
        {
        }
    }
}
