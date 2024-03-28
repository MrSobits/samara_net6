namespace Bars.Gkh.Migrations._2020.Version_2020102700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020102700")]
    [MigrationDependsOn(typeof(Version_2020101400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string EntityTable = "GKH_FORMAT_DATA_EXPORT_ENTITY";
        private const string InfoTable = "GKH_FORMAT_DATA_EXPORT_INFO";

        /// <inheritdoc />
        public override void Up()
        {

            this.Database.AddEntityTable(UpdateSchema.InfoTable,
                new Column("STATE", DbType.Int32, ColumnProperty.NotNull),
                new Column("OBJECT_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("LOAD_DATE", DbType.DateTime, ColumnProperty.NotNull));

            this.Database.AddEntityTable(UpdateSchema.EntityTable,
                new Column("ENTITY_ID", DbType.String),
                new Column("EXTERNAL_GUID", DbType.Guid),
                new Column("ENTITY_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("EXPORT_DATE", DbType.DateTime),
                new Column("EXPORT_STATE", DbType.Int32, ColumnProperty.NotNull),
                new Column("ERROR_MESSAGE", DbType.String),
                new RefColumn("FORMAT_DATA_EXPORT_INFO_ID", "FORMAT_DATA_EXPORT_INFO_ENTITY",
                    UpdateSchema.InfoTable, "ID"));

        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.EntityTable);
            this.Database.RemoveTable(UpdateSchema.InfoTable);
        }
    }
}