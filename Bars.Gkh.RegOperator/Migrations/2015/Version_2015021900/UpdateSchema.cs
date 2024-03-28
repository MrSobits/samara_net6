namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015021900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015021900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015021700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            Database.AddEntityTable("REGOP_PERS_ACC_RECALC_EVT",
                new Column("PERS_ACC_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("RECALC_PROV", DbType.String, 500, ColumnProperty.Null),
                new Column("RECALC_TYPE", DbType.String, 100, ColumnProperty.NotNull),
                new Column("EVT_DATE", DbType.DateTime, ColumnProperty.NotNull)
                );

            Database.AddIndex("IDX_REGOP_PERSACCREVT_TYP", false, "REGOP_PERS_ACC_RECALC_EVT", "RECALC_TYPE");
            Database.AddIndex("IDX_REGOP_PERSACCREVT_PID", false, "REGOP_PERS_ACC_RECALC_EVT", "PERS_ACC_ID");
        }

        public override void Down()
        {
            Database.RemoveIndex("IDX_REGOP_PERSACCREVT_PID", "REGOP_PERS_ACC_RECALC_EVT");
            Database.RemoveIndex("IDX_REGOP_PERSACCREVT_TYP", "REGOP_PERS_ACC_RECALC_EVT");
            Database.RemoveTable("REGOP_PERS_ACC_RECALC_EVT");
        }

        #endregion
    }
}
