namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016052700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.RegOperator.Enums;

    [Migration("2016052700")]
    [MigrationDependsOn(typeof(Version_2016070401.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_PERIOD_CLS_CHCK",
                new Column("CODE", DbType.String, 255, ColumnProperty.NotNull),
                new Column("IMPL", DbType.String, 255, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("IS_CRITICAL", DbType.Boolean, ColumnProperty.NotNull));

            this.Database.AddPersistentObjectTable(
                "REGOP_PERIOD_CLS_CHCK_HIST",
                new RefColumn("CHECK_ID", ColumnProperty.NotNull, "PER_CLS_CH_HST_CH", "REGOP_PERIOD_CLS_CHCK", "ID"),
                new RefColumn("USER_ID", ColumnProperty.NotNull, "PER_CLS_CH_HST_USR", "B4_USER", "ID"),
                new Column("CHANGE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("IS_CRITICAL", DbType.Boolean, ColumnProperty.NotNull));

            this.Database.AddPersistentObjectTable(
                "REGOP_PERIOD_CLS_CHCK_RES",
                new Column("CODE", DbType.String, 255, ColumnProperty.NotNull),
                new Column("IMPL", DbType.String, 255, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("IS_CRITICAL", DbType.Boolean, ColumnProperty.NotNull),
                new Column("CHECK_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("CHECK_STATE", DbType.Int16, ColumnProperty.NotNull, (int)PeriodCloseCheckStateType.Pending),
                new Column("NOTE", DbType.String, 1000, ColumnProperty.Null),
                new RefColumn("LOG_FILE_ID", ColumnProperty.Null, "PER_CLS_CHK_RES_LOG", "B4_FILE_INFO", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "PER_CLS_CHK_RES_PER", "REGOP_PERIOD", "ID"),
                new RefColumn("USER_ID", ColumnProperty.NotNull, "PER_CLS_CHK_RES_USR", "B4_USER", "ID"),
                new RefColumn("GROUP_ID", ColumnProperty.Null, "PER_CLS_CHK_RES_GRP", "REGOP_PA_GROUP", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("REGOP_PERIOD_CLS_CHCK_RES");
            this.Database.RemoveTable("REGOP_PERIOD_CLS_CHCK_HIST");
            this.Database.RemoveTable("REGOP_PERIOD_CLS_CHCK");
        }
    }
}
