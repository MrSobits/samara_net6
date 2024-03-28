namespace Bars.GkhCr.Migrations.Version_2013032102
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032102")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Архив значений в мониторинге смр
            Database.AddEntityTable(
                "CR_OBJ_CMP_ARCHIVE",
                new Column("TYPE_WORK_CR_ID", DbType.Int64, 22),
                new Column("STAGE_WORK_CR_ID", DbType.Int64, 22),
                new Column("MANUFACTURER_NAME", DbType.String, 300),
                new Column("PERCENT_COMPLETION", DbType.Decimal),
                new Column("COST_SUM", DbType.Decimal),
                new Column("COUNT_WORKER", DbType.Decimal),
                new Column("VOLUME_COMPLETION", DbType.Decimal),
                new Column("DATE_CHANGE_REC", DbType.Date),
                new Column("TYPE_ARCHIVE_CMP", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_OBJ_CMP_ARC_TW", false, "CR_OBJ_CMP_ARCHIVE", "TYPE_WORK_CR_ID");
            Database.AddIndex("IND_CR_OBJ_CMP_ARC_SW", false, "CR_OBJ_CMP_ARCHIVE", "STAGE_WORK_CR_ID");
            Database.AddForeignKey("FK_CR_OBJ_CMP_ARC_TW", "CR_OBJ_CMP_ARCHIVE", "TYPE_WORK_CR_ID", "CR_OBJ_TYPE_WORK", "ID");
            Database.AddForeignKey("FK_CR_OBJ_CMP_ARC_SW", "CR_OBJ_CMP_ARCHIVE", "STAGE_WORK_CR_ID", "CR_DICT_STAGE_WORK", "ID");
            //-----

            Database.ChangeColumn("CR_EST_CALC_ESTIMATE", new Column("DOCUMENT_NAME", DbType.String, 700));
            Database.ChangeColumn("CR_OBJ_PERFOMED_WACT_REC", new Column("DOCUMENT_NAME", DbType.String, 700));

            Database.RemoveConstraint("CR_PAYMENT_ORDER", "FK_CR_PAYMENT_FS");
            Database.RemoveColumn("CR_PAYMENT_ORDER", "FIN_SOURCE_ID");

            Database.AddColumn("CR_PAYMENT_ORDER", new Column("TYPE_FIN_SOURCE", DbType.Int32, 4, ColumnProperty.NotNull, 60));
            //------
            Database.AddColumn("CR_OBJECT", new Column("ALLOW_RENEG", DbType.Boolean, ColumnProperty.NotNull, false));
            
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJECT", "ALLOW_RENEG");
            //------
            Database.RemoveConstraint("CR_OBJ_CMP_ARCHIVE", "FK_CR_OBJ_CMP_ARC_SW");
            Database.RemoveConstraint("CR_OBJ_CMP_ARCHIVE", "FK_CR_OBJ_CMP_ARC_TW");
            Database.RemoveTable("CR_OBJ_CMP_ARCHIVE");

            Database.AddColumn("CR_PAYMENT_ORDER", new Column("FIN_SOURCE_ID", DbType.Int64, 22));
            Database.AddForeignKey("FK_CR_PAYMENT_FS", "CR_PAYMENT_ORDER", "FIN_SOURCE_ID", "CR_DICT_FIN_SOURCE", "ID");
        }
    }
}