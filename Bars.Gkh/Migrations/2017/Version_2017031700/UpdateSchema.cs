namespace Bars.Gkh.Migrations._2017.Version_2017031700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция Gkh.2017031700
    /// </summary>
    [Migration("2017031700")]
    [MigrationDependsOn(typeof(Version_2017030602.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_DICT_CENTRAL_HEATING_STATION",
                new Column("NAME", DbType.String, 50),
                new Column("ABBREVIATION", DbType.String, 300),
                new RefColumn("ADDRESS_ID", "IND_GKH_CENTRAL_HEATING_ADDR", "B4_FIAS_ADDRESS", "ID"));

            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "CTP_NAME");

            this.Database.AddRefColumn(
                "GKH_REALITY_OBJECT",
                new RefColumn("CTP_NAME_ID", "GKH_REALITY_OBJECT_CPT_ID", "GKH_DICT_CENTRAL_HEATING_STATION", "ID"));
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "FIAS_ADDRESS_CTP_ID");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_DICT_CENTRAL_HEATING_STATION");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "FIAS_ADDRESS_CTP_ID");
        }
    }
}