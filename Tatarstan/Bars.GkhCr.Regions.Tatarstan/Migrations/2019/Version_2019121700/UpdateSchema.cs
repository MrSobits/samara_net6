namespace Bars.GkhCr.Regions.Tatarstan.Migrations._2019.Version_2019121700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019121700")]
    [MigrationDependsOn(typeof(Version_2019121600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string ObjectOutdoorCrTableName = "CR_OBJECT_OUTDOOR";
        private const string ObjectOutdoorTypeWorkTableName = "CR_OBJ_OUTDOOR_TYPE_WORK";
        private const string ObjectOutdoorTypeWorkHistoryTableName = "CR_OBJ_OUTDOOR_TYPE_WORK_HIST";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.ObjectOutdoorCrTableName,
                new Column("DATE_END_BUILDER", DbType.Date),
                new Column("DATE_START_WORK", DbType.Date),
                new Column("DATE_END_WORK", DbType.Date),
                new Column("COMISSIONING_DATE", DbType.Date),
                new Column("SUM_DEV_PSD", DbType.Decimal),
                new Column("SUM_SMR", DbType.Decimal),
                new Column("SUM_SMR_APPROVED", DbType.Decimal),
                new Column("DESCRIPTION", DbType.String),
                new Column("MAX_AMOUNT", DbType.Decimal),
                new Column("FACT_AMOUNT_SPENT", DbType.Decimal),
                new Column("FACT_START_DATE", DbType.Date),
                new Column("FACT_END_DATE", DbType.Date),
                new Column("WARRANTY_END_DATE", DbType.Date),
                new Column("GJI_NUM", DbType.String),
                new Column("DATE_ACCEPT_GJI", DbType.Date),
                new Column("DATE_GJI_REG", DbType.Date),
                new Column("DATE_STOP_WORK_GJI", DbType.Date),
                new RefColumn("PROGRAM_OUTDOOR_ID", ColumnProperty.Null, 
                    "CR_OBJECT_OUTDOOR_PROGRAM_OUTDOOR", "CR_DICT_PROGRAM_OUTDOOR", "ID"),
                new RefColumn("BEFORE_DELETE_PROGRAM_OUTDOOR_ID", ColumnProperty.Null,
                    "CR_OBJECT_OUTDOOR_BEFORE_DELETE_PROGRAM_OUTDOOR", "CR_DICT_PROGRAM_OUTDOOR", "ID"),
                new RefColumn("REALITY_OBJECT_OUTDOOR_ID", ColumnProperty.NotNull,
                    "CR_OBJECT_OUTDOOR_REALITY_OBJECT_OUTDOOR", "GKH_REALITY_OBJECT_OUTDOOR", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.Null,
                    "CR_OBJECT_OUTDOOR_STATE", "B4_STATE", "ID"));

            //-----Вид работ
            this.Database.AddEntityTable(
                UpdateSchema.ObjectOutdoorTypeWorkTableName,
                new Column("VOLUME", DbType.Decimal),
                new Column("SUM", DbType.Decimal),
                new Column("DESCRIPTION", DbType.String),
                new Column("IS_ACTIVE", DbType.Boolean),
                new RefColumn("OBJECT_OUTDOOR_ID", ColumnProperty.NotNull, 
                    "CR_OBJ_OUTDOOR_TYPE_WORK_OBJECT_OUTDOOR", UpdateSchema.ObjectOutdoorCrTableName, "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull,
                    "CR_OBJ_OUTDOOR_TYPE_WORK_WORK", "CR_DICT_WORK_OUTDOOR", "ID"));

            this.Database.AddEntityTable(UpdateSchema.ObjectOutdoorTypeWorkHistoryTableName,
                new Column("VOLUME", DbType.Decimal),
                new Column("SUM", DbType.Decimal),
                new Column("USER_NAME", DbType.String),
                new Column("TYPE_ACTION", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("TYPE_WORK_ID", ColumnProperty.NotNull,
                    "CR_OBJ_OUTDOOR_TYPE_WORK_HIST_TYPE_WORK", UpdateSchema.ObjectOutdoorTypeWorkTableName, "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.ObjectOutdoorTypeWorkHistoryTableName);
            this.Database.RemoveTable(UpdateSchema.ObjectOutdoorTypeWorkTableName);
            this.Database.RemoveTable(UpdateSchema.ObjectOutdoorCrTableName);
        }
    }
}
