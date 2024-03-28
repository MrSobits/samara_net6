namespace Bars.Gkh.Migrations._2017.Version_2017100600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017100600")]
    [MigrationDependsOn(typeof(Version_2017091100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddPersistentObjectTable("GKH_ENTITY_HISTORY_INFO",
                new Column("EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ACTION_KIND", DbType.Int32, ColumnProperty.NotNull),
                new Column("USERNAME", DbType.String, ColumnProperty.Null),
                new Column("IP_ADDRESS", DbType.String, ColumnProperty.Null),
                new Column("ENTITY_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("PARENT_ENTITY_ID", DbType.Int64, ColumnProperty.NotNull),
                new RefColumn("USER_ID", "GKH_ENTITY_HISTORY_INFO_USER", "B4_USER", "ID"));

            this.Database.AddPersistentObjectTable("GKH_ENTITY_HISTORY_FIELD",
                new Column("OLD_VALUE", DbType.String, ColumnProperty.Null),
                new Column("NEW_VALUE", DbType.String, ColumnProperty.Null),
                new Column("FIELD_NAME", DbType.String, ColumnProperty.NotNull),
                new RefColumn("ENTITY_HISTORY_INFO_ID",
                    ColumnProperty.NotNull,
                    "GKH_ENTITY_HISTORY_FIELD_INFO",
                    "GKH_ENTITY_HISTORY_INFO",
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GKH_ENTITY_HISTORY_INFO");
            this.Database.RemoveTable("GKH_ENTITY_HISTORY_FIELD");
        }
    }
}