namespace Bars.GkhCr.Migrations.Version_2013040900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013040900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013032300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveConstraint("CR_EST_CALC_ESTIMATE", "FK_CR_EST_CAL_EST_UM");
            Database.RemoveConstraint("CR_EST_CALC_RES_STATEM", "FK_CR_EST_CAL_RES_UM");
            Database.RemoveConstraint("CR_OBJ_PERFOMED_WACT_REC", "FK_CR_OBJ_PER_WAC_REC_UM");

            Database.RemoveIndex("IND_CR_EST_CAL_EST_UM", "CR_EST_CALC_ESTIMATE");
            Database.RemoveIndex("IND_CR_EST_CAL_RES_UM", "CR_EST_CALC_RES_STATEM");
            Database.RemoveIndex("IND_CR_OBJ_PER_WAC_REC_UM", "CR_OBJ_PERFOMED_WACT_REC");

            Database.RemoveColumn("CR_EST_CALC_ESTIMATE", "UNIT_MEASURE_ID");
            Database.RemoveColumn("CR_EST_CALC_RES_STATEM", "UNIT_MEASURE_ID");
            Database.RemoveColumn("CR_OBJ_PERFOMED_WACT_REC", "UNIT_MEASURE_ID");

            Database.AddColumn("CR_EST_CALC_ESTIMATE", new Column("UNIT_MEASURE", DbType.String, 300));
            Database.AddColumn("CR_EST_CALC_RES_STATEM", new Column("UNIT_MEASURE", DbType.String, 300));
            Database.AddColumn("CR_OBJ_PERFOMED_WACT_REC", new Column("UNIT_MEASURE", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_EST_CALC_ESTIMATE", "UNIT_MEASURE");
            Database.RemoveColumn("CR_EST_CALC_RES_STATEM", "UNIT_MEASURE");
            Database.RemoveColumn("CR_OBJ_PERFOMED_WACT_REC", "UNIT_MEASURE");

            Database.AddColumn("CR_EST_CALC_ESTIMATE", new Column("UNIT_MEASURE_ID", DbType.Int64, 22));
            Database.AddColumn("CR_EST_CALC_RES_STATEM", new Column("UNIT_MEASURE_ID", DbType.Int64, 22));
            Database.AddColumn("CR_OBJ_PERFOMED_WACT_REC", new Column("UNIT_MEASURE_ID", DbType.Int64, 22));

            Database.AddIndex("IND_CR_EST_CAL_EST_UM", false, "CR_EST_CALC_ESTIMATE", "UNIT_MEASURE_ID");
            Database.AddIndex("IND_CR_EST_CAL_RES_UM", false, "CR_EST_CALC_RES_STATEM", "UNIT_MEASURE_ID");
            Database.AddIndex("IND_CR_OBJ_PER_WAC_REC_UM", false, "CR_OBJ_PERFOMED_WACT_REC", "UNIT_MEASURE_ID");

            Database.AddForeignKey("FK_CR_EST_CAL_EST_UM", "CR_EST_CALC_ESTIMATE", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
            Database.AddForeignKey("FK_CR_EST_CAL_RES_UM", "CR_EST_CALC_RES_STATEM", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
            Database.AddForeignKey("FK_CR_OBJ_PER_WAC_REC_UM", "CR_OBJ_PERFOMED_WACT_REC", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
        }
    }
}