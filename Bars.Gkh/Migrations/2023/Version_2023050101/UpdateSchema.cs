namespace Bars.Gkh.Migrations._2023.Version_2023050101
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050101")]
    
    [MigrationDependsOn(typeof(Version_2023050100.UpdateSchema))]

    /// Является Version_2018022600 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DICT_BASE_HOUSE_EMERGENCY",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_TYPES_HEAT_SOURCE",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_TYPE_INTER_HOUSE_HEATING_SYSTEM",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_TYPES_HEATED_APPLIANCES",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_NETWORK_AND_RISER_MATERIALS",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_NETWORK_INSULATION_MATERIALS",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_TYPES_WATER_DISPOSAL_MATERIAL",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_FOUNDATION_MATERIALS",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_TYPES_WINDOW_MATERIALS",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_TYPES_BEARING_PART_ROOF",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_TYPES_EXTERIOR_WALLS",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_WARMING_LAYERS_ATTICS",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_MATERIAL_ROOF",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_FACADE_DECORATION_MATERIALS",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_WATER_DISPENSERS",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_CATEGORY_CONSUMERS_EQUAL_POPULATION",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GKH_DICT_TYPES_EXTERNAL_FACADE_INSULATION",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String, 255, ColumnProperty.NotNull));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GKH_DICT_BASE_HOUSE_EMERGENCY");
            this.Database.RemoveTable("GKH_DICT_TYPES_HEAT_SOURCE");
            this.Database.RemoveTable("GKH_DICT_TYPE_INTER_HOUSE_HEATING_SYSTEM");
            this.Database.RemoveTable("GKH_DICT_TYPES_HEATED_APPLIANCES");
            this.Database.RemoveTable("GKH_DICT_NETWORK_AND_RISER_MATERIALS");
            this.Database.RemoveTable("GKH_DICT_NETWORK_INSULATION_MATERIALS");
            this.Database.RemoveTable("GKH_DICT_TYPES_WATER_DISPOSAL_MATERIAL");
            this.Database.RemoveTable("GKH_DICT_FOUNDATION_MATERIALS");
            this.Database.RemoveTable("GKH_DICT_TYPES_WINDOW_MATERIALS");
            this.Database.RemoveTable("GKH_DICT_TYPES_EXTERIOR_WALLS");
            this.Database.RemoveTable("GKH_DICT_TYPES_BEARING_PART_ROOF");
            this.Database.RemoveTable("GKH_DICT_WARMING_LAYERS_ATTICS");
            this.Database.RemoveTable("GKH_DICT_MATERIAL_ROOF");
            this.Database.RemoveTable("GKH_DICT_FACADE_DECORATION_MATERIALS");
            this.Database.RemoveTable("GKH_DICT_WATER_DISPENSERS");
            this.Database.RemoveTable("GKH_DICT_CATEGORY_CONSUMERS_EQUAL_POPULATION");
            this.Database.RemoveTable("GKH_DICT_TYPES_EXTERNAL_FACADE_INSULATION");
        }
    }
}