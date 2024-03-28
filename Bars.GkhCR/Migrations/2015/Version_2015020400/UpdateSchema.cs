namespace Bars.GkhCr.Migrations.Version_2015020400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015020400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2015012200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CR_DICT_OFFICIAL",
                new RefColumn("OPERATOR_ID", ColumnProperty.NotNull, "CR_DICT_OFFICIAL_OPER", "GKH_OPERATOR", "ID"),
                new Column("FIO", DbType.String, ColumnProperty.NotNull, 300),
                new Column("CODE", DbType.String, ColumnProperty.NotNull, 300));

            Database.AddEntityTable("CR_OBJ_INSPECTION",
                new RefColumn("TYPE_WORK_ID", ColumnProperty.NotNull, "CR_OBJ_INSPECTION_TW", "CR_OBJ_TYPE_WORK", "ID"),
                new RefColumn("OFFICIAL_ID", "CR_OBJ_INSPECTION_OFC", "CR_DICT_OFFICIAL", "ID"),
                new FileColumn("FILE_ID", "CR_OBJ_INSPECTION_FIL"),
                
                new Column("DOCUMENT_NUMBER", DbType.String, 100),
                new Column("PLAN_DATE", DbType.Date),
                new Column("INSPECTION_STATE", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("FACT_DATE", DbType.Date),
                new Column("REASON", DbType.String, 1000),
                new Column("DESCRIPTION", DbType.String, 2000));

            Database.AddRefColumn("CR_OBJ_CONTRACT", new RefColumn("TYPE_WORK_ID", "CR_OBJ_CONTRACT_TW", "CR_OBJ_TYPE_WORK", "ID"));
            Database.AddRefColumn("CR_OBJ_DOCUMENT_WORK", new RefColumn("TYPE_WORK_ID", "CR_OBJ_DOCUMENT_WORK_TW", "CR_OBJ_TYPE_WORK", "ID"));
            Database.AddRefColumn("CR_OBJ_PROTOCOL", new RefColumn("TYPE_WORK_ID", "CR_OBJ_PROTOCOL_TW", "CR_OBJ_TYPE_WORK", "ID"));
            Database.AddRefColumn("CR_OBJ_BUILD_CONTRACT", new RefColumn("TYPE_WORK_ID", "CR_OBJ_BUILD_CONTRACT_TW", "CR_OBJ_TYPE_WORK", "ID"));
            Database.AddRefColumn("CR_OBJ_DEFECT_LIST", new RefColumn("TYPE_WORK_ID", "CR_OBJ_DEFECT_LIST_TW", "CR_OBJ_TYPE_WORK", "ID"));

            Database.AddRefColumn("CR_OBJ_TYPE_WORK", new StateColumn("STATE_ID", "CR_OBJ_TYPE_WORK_STA"));
        }

        public override void Down()
        {
            Database.RemoveTable("CR_DICT_OFFICIAL");

            Database.RemoveColumn("CR_OBJ_CONTRACT", "TYPE_WORK_ID");
            Database.RemoveColumn("CR_OBJ_DOCUMENT_WORK", "TYPE_WORK_ID");
            Database.RemoveColumn("CR_OBJ_PROTOCOL", "TYPE_WORK_ID");
            Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "TYPE_WORK_ID");
            Database.RemoveColumn("CR_OBJ_DEFECT_LIST", "TYPE_WORK_ID");
            Database.RemoveColumn("CR_OBJ_TYPE_WORK", "STATE_ID");

            Database.RemoveTable("CR_OBJ_INSPECTION");
        }
    }
}