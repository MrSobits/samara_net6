namespace Bars.Gkh.Migrations._2016.Version_2016090100
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016090100")]
    [MigrationDependsOn(typeof(Version_2016081600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("IS_CULTURAL_HERITAGE", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("GKH_ROOM", new Column ("COMMON_PROPERTY_IN_MCD", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddEntityTable("GKH_OBJ_BLOCK",
                new RefColumn("REALITY_OBJECT_ID", "GKH_OBJ_BLOCK_RO", "GKH_REALITY_OBJECT", "ID"),
                new Column("AREA_LIVING", DbType.Decimal),
                new Column("AREA_TOTAL", DbType.Decimal),
                new Column("CADASTRAL_NUMBER", DbType.String),
                new Column("NUMBER", DbType.String));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "IS_CULTURAL_HERITAGE");
            this.Database.RemoveTable("GKH_OBJ_BLOCK");
            this.Database.RemoveColumn("GKH_ROOM", "COMMON_PROPERTY_IN_MCD");
        }
    }
}