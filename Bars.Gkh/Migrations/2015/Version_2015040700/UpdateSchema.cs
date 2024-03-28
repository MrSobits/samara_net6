namespace Bars.Gkh.Migrations.Version_2015040700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015040700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015032700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CLW_JUR_INSTITUTION",
                new Column("JUR_INST_TYPE", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("COURT_TYPE", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("NAME", DbType.String, 250),
                new Column("SHORT_NAME", DbType.String, 250),
                new Column("ADDRESS", DbType.String, 500),
                new Column("POST_CODE", DbType.String, 50),
                new Column("PHONE", DbType.String, 250),
                new Column("EMAIL", DbType.String, 250),
                new Column("WEBSITE", DbType.String, 250),
                new Column("JUDGE_SURNAME", DbType.String, 250),
                new Column("JUDGE_NAME", DbType.String, 250),
                new Column("JUDGE_PATRONOMYC", DbType.String, 250),
                new Column("JUDGE_SHORT_FIO", DbType.String, 250),
                new RefColumn("FIAS_ADDRESS_ID", "CLW_JUR_INS_FIAS", "B4_FIAS_ADDRESS", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "CLW_JUR_INS_MU", "GKH_DICT_MUNICIPALITY", "ID"));

            Database.AddEntityTable("CLW_JUR_INST_REAL_OBJ",
                new RefColumn("JUR_INST_ID", ColumnProperty.NotNull, "CLW_JUR_INS_J_INS", "CLW_JUR_INSTITUTION", "ID"),
                new RefColumn("REAL_OBJ_ID", ColumnProperty.NotNull, "CLW_JUR_INS_RO", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("CLW_JUR_INST_REAL_OBJ");
            Database.RemoveTable("CLW_JUR_INSTITUTION");
        }
    }
}