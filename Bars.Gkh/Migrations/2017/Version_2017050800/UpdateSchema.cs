namespace Bars.Gkh.Migrations._2017.Version_2017050800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017050800
    /// </summary>
    [Migration("2017050800")]
    [MigrationDependsOn(typeof(Version_2017042100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DATA_INTEGRATION_SESSION",
                new GuidColumn("GUID", ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime),
                new Column("DESCRIPTION", DbType.String),
                new Column("TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("STATE", DbType.Int32, ColumnProperty.NotNull),
                new Column("SUCCESS", DbType.Int32),
                new Column("ERROR_CODE", DbType.String),
                new Column("ERROR_MESSAGE", DbType.String, 5000),
                new Column("TYPE_NAMES", DbType.String, 5000));
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveTable("GKH_DATA_INTEGRATION_SESSION");
        }
    }
}