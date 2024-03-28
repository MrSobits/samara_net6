namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016030100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016030100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2014022400.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_CONSTRUCTION_OBJECT",
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("MUNICIPALITY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("STL_MUNICIPALITY_ID", DbType.Int64, 22),
                new Column("ADDRESS", DbType.String, 1000),
                new Column("SUM_SMR", DbType.Decimal.WithSize(10, 2)),
                new Column("SUM_DEV_PSD", DbType.Decimal.WithSize(10, 2)),
                new Column("DATE_END_BUILDER", DbType.Date),
                new Column("DATE_START_WORK", DbType.Date),
                new Column("DATE_STOP_WORK", DbType.Date),
                new Column("DATE_RESUME_WORK", DbType.Date),
                new Column("REASON_STOP_WORK", DbType.String, 1000),
                new Column("DATE_COMMISSIONING", DbType.DateTime),
                new Column("LIMIT_ON_HOUSE", DbType.Decimal.WithSize(10, 2)),
                new Column("TOTAL_AREA", DbType.Decimal.WithSize(10, 2)),
                new Column("NUM_APARTMENTS", DbType.Int32),
                new Column("RESETTLE_PROG_NUM_APARTS", DbType.Int32),
                new Column("NUM_FLOORS", DbType.Int32),
                new Column("NUM_ENTRANCES", DbType.Int32),
                new Column("NUM_LIFTS", DbType.Int32),
                new Column("TYPE_ROOF", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("FIAS_ADDRESS_ID", DbType.Int64, 22),
                new Column("ROOFING_MATERIAL_ID", DbType.Int64, 22),
                new Column("WALL_MATERIAL_ID", DbType.Int64, 22));

            this.Database.AddIndex("IND_GKH_CONSTRUCTION_OBJECT_MCP", false, "GKH_CONSTRUCTION_OBJECT", "MUNICIPALITY_ID");
            this.Database.AddIndex("IND_GKH_CONSTRUCTION_OBJECT_STL_MCP", false, "GKH_CONSTRUCTION_OBJECT", "STL_MUNICIPALITY_ID");
            this.Database.AddIndex("IND_GKH_CONSTRUCTION_OBJECT_STATE", false, "GKH_CONSTRUCTION_OBJECT", "STATE_ID");
            this.Database.AddIndex("IND_GKH_CONSTRUCTION_OBJECT_ADR", false, "GKH_CONSTRUCTION_OBJECT", "FIAS_ADDRESS_ID");
            this.Database.AddIndex("IND_GKH_CONSTRUCTION_OBJECT_RMT", false, "GKH_CONSTRUCTION_OBJECT", "ROOFING_MATERIAL_ID");
            this.Database.AddIndex("IND_GKH_CONSTRUCTION_OBJECT_WMT", false, "GKH_CONSTRUCTION_OBJECT", "WALL_MATERIAL_ID");

            this.Database.AddForeignKey("FK_GKH_CONSTRUCTION_OBJECT_MCP", "GKH_CONSTRUCTION_OBJECT", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            this.Database.AddForeignKey("FK_GKH_CONSTRUCTION_OBJECT_STL_MCP", "GKH_CONSTRUCTION_OBJECT", "STL_MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            this.Database.AddForeignKey("FK_GKH_CONSTRUCTION_OBJECT_STATE", "GKH_CONSTRUCTION_OBJECT", "STATE_ID", "B4_STATE", "ID");
            this.Database.AddForeignKey("FK_GKH_CONSTRUCTION_OBJECT_ADR", "GKH_CONSTRUCTION_OBJECT", "FIAS_ADDRESS_ID", "B4_FIAS_ADDRESS", "ID");
            this.Database.AddForeignKey("FK_GKH_CONSTRUCTION_OBJECT_RMT", "GKH_CONSTRUCTION_OBJECT", "ROOFING_MATERIAL_ID", "GKH_DICT_ROOFING_MATERIAL", "ID");
            this.Database.AddForeignKey("FK_GKH_CONSTRUCTION_OBJECT_WMT", "GKH_CONSTRUCTION_OBJECT", "WALL_MATERIAL_ID", "GKH_DICT_WALL_MATERIAL", "ID");


            //this.Database.AddEntityTable("CONSTR_OBJ_MONITORING_SMR",
            //    new Column("OBJECT_ID", DbType.Int64, 22),
            //    new Column("STATE_ID", DbType.Int64, 22),
            //    new Column("EXTERNAL_ID", DbType.String, 36));

            //this.Database.AddIndex("IND_CONSTR_OBJ_MON_CMP_OBJ", false, "CONSTR_OBJ_MONITORING_SMR", "OBJECT_ID");
            //this.Database.AddIndex("IND_CONSTR_OBJ_MON_CMP_ST", false, "CONSTR_OBJ_MONITORING_SMR", "STATE_ID");

            //this.Database.AddForeignKey("FK_CONSTR_OBJ_MON_CMP_OBJ", "CONSTR_OBJ_MONITORING_SMR", "OBJECT_ID", "GKH_CONSTRUCTION_OBJECT", "ID");
            //this.Database.AddForeignKey("FK_CONSTR_OBJ_MON_CMP_ST", "CONSTR_OBJ_MONITORING_SMR", "STATE_ID", "B4_STATE", "ID");
        }

        public override void Down()
        {
            this.Database.RemoveConstraint("GKH_CONSTRUCTION_OBJECT", "FK_GKH_CONSTRUCTION_OBJECT_MCP");
            this.Database.RemoveConstraint("GKH_CONSTRUCTION_OBJECT", "FK_GKH_CONSTRUCTION_OBJECT_STL_MCP");
            this.Database.RemoveConstraint("GKH_CONSTRUCTION_OBJECT", "FK_GKH_CONSTRUCTION_OBJECT_STATE");
            this.Database.RemoveConstraint("GKH_CONSTRUCTION_OBJECT", "FK_GKH_CONSTRUCTION_OBJECT_ADR");
            this.Database.RemoveConstraint("GKH_CONSTRUCTION_OBJECT", "FK_GKH_CONSTRUCTION_OBJECT_RMT");
            this.Database.RemoveConstraint("GKH_CONSTRUCTION_OBJECT", "FK_GKH_CONSTRUCTION_OBJECT_WMT");

            //this.Database.RemoveConstraint("CONSTR_OBJ_MONITORING_SMR", "FK_CONSTR_OBJ_MON_CMP_OBJ");
            //this.Database.RemoveConstraint("CONSTR_OBJ_MONITORING_SMR", "FK_CONSTR_OBJ_MON_CMP_ST");

            this.Database.RemoveTable("GKH_CONSTRUCTION_OBJECT");
            //this.Database.RemoveTable("CONSTR_OBJ_MONITORING_SMR");
        }
    }
}
