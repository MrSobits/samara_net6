namespace Bars.Gkh.Gis.Migrations._2014.Version_1
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("1")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //----- Таблица справочника индикаторов
            this.Database.AddPersistentObjectTable(
                "GIS_REAL_ESTATE_INDICATOR",
                new Column("NAME", DbType.String, 100, ColumnProperty.NotNull)
                );
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_REAL_ESTATE_INDICATOR");
        }
    }
}