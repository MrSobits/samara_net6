namespace Bars.GisIntegration.Base.Migrations.Version_2019012800
{
    using System;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2019012800")]
    [MigrationDependsOn(typeof(Version_2016120700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GIS_GKH_DICT_DATA",
              new Column("RegistryNumber", DbType.Int32, ColumnProperty.NotNull),
              new Column("Name", DbType.String, 255, ColumnProperty.NotNull),
              new Column("Modified", DbType.DateTime, ColumnProperty.NotNull),
              new Column("RawReply", DbType.String, Int32.MaxValue, ColumnProperty.NotNull),
              new Column("LastRequest", DbType.DateTime, ColumnProperty.NotNull)
              );
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("GIS_GKH_DICT_DATA");
        }
    }
}