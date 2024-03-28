namespace Bars.Gkh.Regions.Voronezh.Migrations._2018.Version_2018022400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2018022400")]
    [MigrationDependsOn(typeof(Version_2018022100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            var primaryTable = new SchemaQualifiedObjectName { Name = "DATEAREAOWNER", Schema = "IMPORT" };
            var foreignTable = new SchemaQualifiedObjectName { Name = "GKH_VR_ACCOUNT_COMPARSION", Schema = "PUBLIC" };

            Database.AddEntityTable("GKH_VR_ACCOUNT_COMPARSION",
                   new Column("DATA_AREA_OWNER_ID", DbType.Int64, ColumnProperty.NotNull),
                   new Column("IS_MERGED", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                   new Column("IS_CHANGED", DbType.Int32, 4, ColumnProperty.NotNull, 20),
                   new RefColumn("ACCOUNT_ID", ColumnProperty.None, "GKH_VR_ACCOUNT_COMPARSION_REGOP_PERS_ACC", "REGOP_PERS_ACC", "ID"),
                   new Column("ACC_NUM", DbType.String),
                   new RefColumn("ROOM_ID", ColumnProperty.None, "GKH_VR_ACCOUNT_COMPARSION_ROOM", "GKH_ROOM", "ID"),
                   new Column("DATA_UPDATE_DATE", DbType.DateTime),
                   new Column("DATA_UPDATE_DATE_FROM", DbType.DateTime));

            //Database.AddForeignKey("GKH_VR_COMPARSION_DATA_AREA_OWNER", foreignTable, "DATA_AREA_OWNER_ID", primaryTable, "ID");



        }
        public override void Down()
        {
            Database.RemoveTable("GKH_VR_ACCOUNT_COMPARSION");
 
        }
    }
}