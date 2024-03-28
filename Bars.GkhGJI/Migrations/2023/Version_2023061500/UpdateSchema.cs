namespace Bars.GkhGji.Migrations._2023.Version_2023061500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023061500")]
    [MigrationDependsOn(typeof(_2023.Version_2023052300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "GKH_ENTITY_OLDVALUE_HISTORY",
               new Column("ENTITY_ID", DbType.Int64, ColumnProperty.NotNull),
               new Column("OLDVALUE", DbType.Binary),             
               new Column("ENTITY_TYPE", DbType.Int16, ColumnProperty.NotNull, 1));

            Database.AddIndex("IND_GKH_ENTITY_OVHISTORY_ENTITY_ID", false, "GKH_ENTITY_OLDVALUE_HISTORY", "ENTITY_ID");
            Database.AddIndex("IND_GKH_ENTITY_OVHISTORY_ENTITY_TYPE", false, "GKH_ENTITY_OLDVALUE_HISTORY", "ENTITY_TYPE");

            Database.AddEntityTable(
              "GKH_ENTITY_CHANGE_LOG",
              new Column("ENTITY_ID", DbType.Int64, ColumnProperty.NotNull),
              new Column("OPERATOR_ID", DbType.Int64, ColumnProperty.NotNull),
              new Column("ENTITY_VALUE", DbType.String, 150, ColumnProperty.None),
              new Column("OLDVALUE", DbType.String, 250, ColumnProperty.None),
              new Column("NEWVALUE", DbType.String, 250, ColumnProperty.None),
              new Column("PROPERTY_TYPE", DbType.String, 150, ColumnProperty.None),
              new Column("PROPERTY_NAME", DbType.String, 150, ColumnProperty.None),
              new Column("OPERATOR_LOGIN", DbType.String, 150, ColumnProperty.None),
              new Column("OPERATOR_NAME", DbType.String, 150, ColumnProperty.None),
              new Column("ENTITY_TYPE", DbType.Int16, ColumnProperty.NotNull, 1),
              new Column("OPERATION_TYPE", DbType.Int16, ColumnProperty.NotNull, 10));

            Database.AddIndex("IND_GKH_ENTITY_CHANGE_LOG_ENTITY_ID", false, "GKH_ENTITY_CHANGE_LOG", "ENTITY_ID");
            Database.AddIndex("IND_GKH_ENTITY_CHANGE_LOG_ENTITY_TYPE", false, "GKH_ENTITY_CHANGE_LOG", "ENTITY_TYPE");
            Database.AddIndex("IND_GKH_ENTITY_CHANGE_LOG_OPERATOR_LOGIN", false, "GKH_ENTITY_CHANGE_LOG", "OPERATOR_LOGIN");
            Database.AddIndex("IND_GKH_ENTITY_CHANGE_LOG_OPERATION_TYPE", false, "GKH_ENTITY_CHANGE_LOG", "OPERATION_TYPE");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_ENTITY_OLDVALUE_HISTORY");
            this.Database.RemoveTable("GKH_ENTITY_CHANGE_LOG");
        }
    }
}