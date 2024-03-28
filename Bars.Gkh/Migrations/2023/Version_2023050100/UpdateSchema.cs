namespace Bars.Gkh.Migrations._2023.Version_2023050100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050100")]
    
    [MigrationDependsOn(typeof(_2023.Version_2023040600.UpdateSchema))]

    /// Является Version_2018021600 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_NOTIFY_MESSAGE",
                new Column("IS_DELETE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("TITLE", DbType.String, ColumnProperty.NotNull),
                new Column("TEXT", DbType.String, 15000, ColumnProperty.NotNull),
                new Column("BUTTON_SET", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("USER_ID", ColumnProperty.NotNull, "GKH_NOTIFY_MESSAGE_USER", "B4_USER", "ID"));

            this.Database.AddEntityTable("GKH_NOTIFY_STATS",
                new Column("BUTTON", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("MESSAGE_ID", ColumnProperty.NotNull, "GKH_NOTIFY_STATS_MESSAGE", "GKH_NOTIFY_MESSAGE", "ID"),
                new RefColumn("USER_ID", ColumnProperty.NotNull, "GKH_NOTIFY_STATS_USER", "B4_USER", "ID"));

            this.Database.AddPersistentObjectTable("GKH_NOTIFY_PERMISSION",
                new RefColumn("MESSAGE_ID", ColumnProperty.NotNull, "GKH_NOTIFY_PERMISSION_MESSAGE", "GKH_NOTIFY_MESSAGE", "ID"),
                new RefColumn("ROLE_ID", ColumnProperty.NotNull, "GKH_NOTIFY_PERMISSION_ROLE", "B4_ROLE", "ID"));

            this.Database.AddUniqueConstraint("GKH_NOTIFY_STATS_UNIQUE", "GKH_NOTIFY_STATS", "MESSAGE_ID", "USER_ID");
            this.Database.AddUniqueConstraint("GKH_NOTIFY_PERMISSION_UNIQUE", "GKH_NOTIFY_PERMISSION", "MESSAGE_ID", "ROLE_ID");

            this.Database.AddUniqueConstraint("B4_ROLE_UNIQUE", "B4_ROLE", "NAME");

            this.Database.RemoveTable("GKH_EXECUTION_ACTION");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GKH_NOTIFY_PERMISSION");
            this.Database.RemoveTable("GKH_NOTIFY_STATS");
            this.Database.RemoveTable("GKH_NOTIFY_MESSAGE");
        }
    }
}