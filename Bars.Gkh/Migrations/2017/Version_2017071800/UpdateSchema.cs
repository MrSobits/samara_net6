namespace Bars.Gkh.Migrations._2017.Version_2017071800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017071800")]
    [MigrationDependsOn(typeof(Version_2017071000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_LOCAL_ADMIN_ROLE_RELATIONS",
                new RefColumn("PARENT_ROLE_ID", ColumnProperty.NotNull, "GKH_LOCAL_ADMIN_ROLE_RELATIONS_PARENT_ROLE", "B4_ROLE", "ID"),
                new RefColumn("CHILD_ROLE_ID", ColumnProperty.NotNull, "GKH_LOCAL_ADMIN_ROLE_RELATIONS_CHILD_ROLE", "B4_ROLE", "ID"));

            this.Database.AddPersistentObjectTable("GKH_LOCAL_ADMIN_ROLE_PERMISSION",
                new Column("PERMISSION_ID", DbType.String, 500, ColumnProperty.NotNull),
                new RefColumn("ROLE_ID", ColumnProperty.NotNull, "GKH_LOCAL_ADMIN_ROLE_PERMISSION_ROLE", "B4_ROLE", "ID"));

            this.Database.AddPersistentObjectTable("GKH_LOCAL_ADMIN_STATE_PERMISSION",
                new Column("PERMISSION_ID", DbType.String, 500, ColumnProperty.NotNull),
                new RefColumn("ROLE_ID", ColumnProperty.NotNull, "GKH_LOCAL_ADMIN_STATE_PERMISSION_ROLE", "B4_ROLE", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.NotNull, "GKH_LOCAL_ADMIN_STATE_PERMISSION_STATE", "B4_STATE", "ID"));

            this.Database.AddPersistentObjectTable("GKH_LOCAL_ADMIN_STATEFUL_ENTITY",
                new Column("TYPE_ID", DbType.String, 100, ColumnProperty.NotNull),
                new RefColumn("ROLE_ID", ColumnProperty.NotNull, "GKH_LOCAL_ADMIN_STATEFUL_ENTITY_ROLE", "B4_ROLE", "ID"));

            this.Database.AddUniqueConstraint("UNIQUE_GKH_LOCAL_ADMIN_ROLE_RELATIONS", "GKH_LOCAL_ADMIN_ROLE_RELATIONS", "PARENT_ROLE_ID", "CHILD_ROLE_ID");
            this.Database.AddUniqueConstraint("UNIQUE_GKH_LOCAL_ADMIN_ROLE_PERMISSION", "GKH_LOCAL_ADMIN_ROLE_PERMISSION", "ROLE_ID", "PERMISSION_ID");
            this.Database.AddUniqueConstraint("UNIQUE_GKH_LOCAL_ADMIN_STATEFUL_ENTITY", "GKH_LOCAL_ADMIN_STATEFUL_ENTITY", "ROLE_ID", "TYPE_ID");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_LOCAL_ADMIN_ROLE_RELATIONS");
            this.Database.RemoveTable("GKH_LOCAL_ADMIN_ROLE_PERMISSION");
            this.Database.RemoveTable("GKH_LOCAL_ADMIN_STATE_PERMISSION");
        }
    }
}