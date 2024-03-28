namespace Bars.Gkh.Migrations.Version_2013100401
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013100400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveIndex("UNIQ_CHARACT_VAL", "GKH_OBJ_CHARACT_VAL");
            Database.RemoveTable("GKH_OBJ_CHARACT_VAL");

            Database.RemoveTable("GKH_OBJ_METDEV_WRK");
            Database.RemoveTable("GKH_OBJ_COMMET_DEV");

            Database.RemoveConstraint("GKH_OBJ_LIFT_WRK", "FK_OBJ_LIFT_WRK_LFT");
            Database.RemoveConstraint("GKH_OBJ_LIFT_WRK", "FK_OBJ_LIFT_WRK_BASE");
            Database.RemoveIndex("IND_OBJ_LIFT_WRK_ELM", "GKH_OBJ_LIFT_WRK");
            Database.RemoveTable("GKH_OBJ_LIFT_WRK");

            Database.RemoveConstraint("GKH_OBJ_CONSTR_EL_WRK", "FK_OBJ_CONSTR_EL_WRK_ELM");
            Database.RemoveConstraint("GKH_OBJ_CONSTR_EL_WRK", "FK_OBJ_CONSTR_EL_WRK_BASE");
            Database.RemoveIndex("IND_OBJ_CONSTR_EL_WRK_ELM", "GKH_OBJ_CONSTR_EL_WRK");
            Database.RemoveTable("GKH_OBJ_CONSTR_EL_WRK");

            Database.RemoveTable("GKH_OBJ_BASE_WRK");
            Database.RemoveTable("GKH_OBJ_LIFT");
            Database.RemoveTable("GKH_DICT_TYPE_LIFT_CABIN");
            Database.RemoveTable("GKH_OBJ_CONSTRUCT_ELEM");
            Database.RemoveTable("GKH_REALOBJ_ELEMENT_OBJ");
            Database.RemoveTable("GKH_DICT_GROUP_EL_WORK");

            Database.RemoveColumn("GKH_DICT_CONEL_GROUP", "GROUP_EL_OBJ_ID");

            Database.RemoveColumn("GKH_DICT_GROUP_ELEM_OBJ", "UNIT_MEASURE_ID");

            Database.RemoveTable("GKH_DICT_ELOBJ_CHARACT");
            Database.RemoveTable("GKH_DICT_GROUP_ELEM_OBJ");
            
            Database.RemoveTable("GKH_DICT_CHARACT_ELEM");
            Database.RemoveTable("GKH_DICT_CHARACTS");
        }

        public override void Down()
        {
            Database.AddEntityTable(
                "GKH_DICT_CHARACTS",
                new RefColumn("UNIT_MEASURE_ID", "GKH_DICT_CHARACTS_UM", "GKH_DICT_UNITMEASURE", "ID"),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("CODE", DbType.String, 300));

            Database.AddEntityTable(
               "GKH_DICT_CHARACT_ELEM",
               new RefColumn("CHARACT_ID", "GKH_DICT_CHARACTS_CH", "GKH_DICT_CHARACTS", "ID"),
               new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
               new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));

            //-----Группы элементов объектов общего имущества
            Database.AddEntityTable(
                "GKH_DICT_GROUP_ELEM_OBJ",
                new Column("NAME", DbType.String, 300),
                new Column("TYPE_GROUP", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("REPAIR_RESOURCE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("ACCEPT_PRESENCE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("TYPE_ENGINEER_SYSTEM", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("METERING_INDICATOR", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new RefColumn("UNIT_MEASURE_ID", "GKH_GROUP_ELEM_OBJ_UM", "GKH_DICT_UNITMEASURE", "ID"),
                 new Column("IS_REQUIRED_HOUSE_LAW", DbType.Boolean, ColumnProperty.NotNull, false),
                 new Column("IS_INCLUD_PROGRAM_SUBJ", DbType.Boolean, ColumnProperty.NotNull, false),
                 new Column("IS_COMPLEX_OBJECT", DbType.Boolean, ColumnProperty.NotNull, false));

            Database.AddIndex("IND_GKH_GROUP_ELEM_OBJ_NAME", false, "GKH_DICT_GROUP_ELEM_OBJ", "NAME");

            Database.AddEntityTable(
                "GKH_DICT_CHARACTS",
                new RefColumn("UNIT_MEASURE_ID", "GKH_DICT_CHARACTS_UM", "GKH_DICT_UNITMEASURE", "ID"),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10));

            Database.AddEntityTable(
               "GKH_DICT_CHARACT_ELEM",
               new RefColumn("CHARACT_ID", "GKH_DICT_CHARACTS_CH", "GKH_DICT_CHARACTS", "ID"),
               new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
               new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));

            Database.AddEntityTable(
               "GKH_DICT_ELOBJ_CHARACT",
               new RefColumn("CHARACT_ID", "GKH_DICT_ELCHARACT_CH", "GKH_DICT_CHARACTS", "ID"),
               new RefColumn("ELOBJ_ID", "GKH_DICT_ELCHARACT_EL", "GKH_DICT_GROUP_ELEM_OBJ", "ID"));

            //-----Группы элементов объектов общего имущества
            Database.AddEntityTable(
                "GKH_DICT_GROUP_EL_WORK",
                new RefColumn("WORK_ID", "GKH_GROUP_EL_WORK_WORK", "GKH_DICT_WORK", "ID"),
                new RefColumn("GROUP_ELEMENT_ID", "GKH_GROUP_EL_WORK_ELEM", "GKH_DICT_GROUP_ELEM_OBJ", "ID"),
                new Column("EXTERNAL_ID", DbType.String, 36));

            Database.AddEntityTable(
             "GKH_REALOBJ_ELEMENT_OBJ",
             new RefColumn("REALITY_OBJECT_ID", "GKH_REALEL_OBJ_RO", "GKH_REALITY_OBJECT", "ID"),
             new RefColumn("ELEMENT_OBJECT_ID", "GKH_REALEL_OBJ_EO", "GKH_DICT_GROUP_ELEM_OBJ", "ID"),
             new Column("IS_EXIST", DbType.Int32, 4, ColumnProperty.NotNull, 30),
             new Column("YEAR_LAST_CR", DbType.Int32));

            Database.AddEntityTable(
               "GKH_OBJ_CHARACT_VAL",
               new Column("VALUE", DbType.String, 300),
               new RefColumn("REALOBJ_ELEMENT_ID", "GKH_CHARACTVAL_EO", "GKH_REALOBJ_ELEMENT_OBJ", "ID"),
               new RefColumn("CHARACT_ID", "GKH_CHARACTVAL_CH", "GKH_DICT_CHARACTS", "ID"));

            Database.AddIndex("UNIQ_CHARACT_VAL", true, "GKH_OBJ_CHARACT_VAL", "REALOBJ_ELEMENT_ID", "CHARACT_ID");

            Database.AddEntityTable("GKH_OBJ_CONSTRUCT_ELEM",
            new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "GKH_OBJ_CONSTR_ELM_OBJ", "GKH_REALITY_OBJECT", "ID"),
            new RefColumn("CONSTRUCT_ELEM_ID", ColumnProperty.NotNull, "GKH_OBJ_CONSTR_ELM_ELM", "GKH_DICT_CONST_ELEMENT", "ID"),
            new RefColumn("FILE_ID", "GKH_OBJ_CONSTR_ELM_FIL", "B4_FILE_INFO", "ID"),
            new Column("YEAR_INSTALLATION", DbType.Int32),
            new Column("REPAIRED", DbType.Int32, 4, ColumnProperty.NotNull, 30),
            new Column("RATE_WEAR", DbType.Decimal),
            new Column("VOLUME", DbType.Decimal),
            new Column("DOCUMENT_DATE", DbType.Date),
            new Column("DOCUMENT_NUMBER", DbType.String, 300),
            new Column("EXTERNAL_ID", DbType.String, 36));

            Database.AddEntityTable("GKH_DICT_TYPE_LIFT_CABIN",
               new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
               new Column("CODE", DbType.Int32),
               new Column("NORMATIVE_LIFETIME", DbType.Decimal));

            Database.AddEntityTable("GKH_OBJ_LIFT",
               new Column("DATE_COMMISSIONING", DbType.DateTime),
               new RefColumn("FILE_ID", "GKH_OBJ_LIFT_FILE", "B4_FILE_INFO", "ID"),
               new Column("SERIAL_NUMBER", DbType.String, 300),
               new Column("STOP_COUNT", DbType.Int32),
               new Column("ROOM_ENTRANCE", DbType.Int32),
               new Column("DOCUMENT_DATE", DbType.DateTime),
               new Column("DOCUMENT_NUM", DbType.String, 300),
               new Column("WEAR", DbType.Decimal),
               new Column("IS_REPAIRED", DbType.Int32, 4, ColumnProperty.NotNull, 30),
               new RefColumn("TYPE_LIFT_CABIN_ID", "GKH_OBJ_LIFT_CABIN", "GKH_DICT_TYPE_LIFT_CABIN", "ID"),
               new RefColumn("REALITY_OBJECT_ID", "GKH_LIFT_OBJ_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.AddEntityTable("GKH_OBJ_BASE_WRK",
               new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "GKH_OBJ_BASE_WRK_OBJ", "GKH_REALITY_OBJECT", "ID"),
               new Column("DATE_LAST_REPAIR", DbType.Date),
               new Column("TYPE_COMPLETE_WORK", DbType.Int32, 4, ColumnProperty.NotNull, 10),
               new Column("TYPE_REPAIR", DbType.Int32, 4, ColumnProperty.NotNull, 10),
               new Column("VOLUME_REPAIR", DbType.Int32, 4, ColumnProperty.NotNull, 10),
               new Column("DESCRIPTION", DbType.String, 1000));

            Database.AddTable("GKH_OBJ_CONSTR_EL_WRK",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJ_CONSTR_EL_ID", DbType.Int64, ColumnProperty.NotNull));

            Database.AddIndex("IND_OBJ_CONSTR_EL_WRK_ELM", false, "GKH_OBJ_CONSTR_EL_WRK", "OBJ_CONSTR_EL_ID");
            Database.AddForeignKey("FK_OBJ_CONSTR_EL_WRK_ELM", "GKH_OBJ_CONSTR_EL_WRK", "OBJ_CONSTR_EL_ID", "GKH_OBJ_CONSTRUCT_ELEM", "ID");
            Database.AddForeignKey("FK_OBJ_CONSTR_EL_WRK_BASE", "GKH_OBJ_CONSTR_EL_WRK", "ID", "GKH_OBJ_BASE_WRK", "ID");

            Database.AddTable("GKH_OBJ_LIFT_WRK",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJ_LIFT_ID", DbType.Int64, ColumnProperty.NotNull));

            Database.AddIndex("IND_OBJ_LIFT_WRK_ELM", false, "GKH_OBJ_LIFT_WRK", "OBJ_LIFT_ID");
            Database.AddForeignKey("FK_OBJ_LIFT_WRK_LFT", "GKH_OBJ_LIFT_WRK", "OBJ_LIFT_ID", "GKH_OBJ_LIFT", "ID");
            Database.AddForeignKey("FK_OBJ_LIFT_WRK_BASE", "GKH_OBJ_LIFT_WRK", "ID", "GKH_OBJ_BASE_WRK", "ID");

            Database.AddEntityTable("GKH_OBJ_COMMET_DEV",
               new RefColumn("MET_DEVICE_ID", ColumnProperty.NotNull, "GKH_OBJ_COMMETDEV_MD", "GKH_DICT_METERING_DEVICE", "ID"),
               new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GKH_OBJ_COMMETDEV_RO", "GKH_REALITY_OBJECT", "ID"),
               new RefColumn("ELEMENT_OBJECT_ID", ColumnProperty.NotNull, "GKH_OBJ_COMMETDEV_EO", "GKH_DICT_GROUP_ELEM_OBJ", "ID"),
               new RefColumn("FILE_INFO_ID", "GKH_OBJ_COMMETDEV_FILE", "B4_FILE_INFO", "ID"),
               new Column("DATE_INSTALLED", DbType.Date, ColumnProperty.NotNull),
               new Column("INVENTORY_NUMBER", DbType.Int32),
               new Column("ESTIMATE_WEAR", DbType.Decimal),
               new Column("DOCUMENT_DATE", DbType.DateTime),
               new Column("DOCUMENT_NUM", DbType.String, 300),
               new Column("IS_REPAIRED", DbType.Int32, 4, ColumnProperty.NotNull, 30));

            Database.AddEntityTable("GKH_OBJ_METDEV_WRK",
                new Column("METDEV_ID", DbType.Int64, ColumnProperty.NotNull));

            Database.AddIndex("IND_OBJ_METDEV_WRK_ELM", false, "GKH_OBJ_METDEV_WRK", "METDEV_ID");
            Database.AddForeignKey("FK_OBJ_METDEV_WRK_LFT", "GKH_OBJ_METDEV_WRK", "METDEV_ID", "GKH_OBJ_COMMET_DEV", "ID");
            Database.AddForeignKey("FK_OBJ_METDEV_WRK_BASE", "GKH_OBJ_METDEV_WRK", "ID", "GKH_OBJ_BASE_WRK", "ID");

            Database.AddRefColumn("GKH_DICT_CONEL_GROUP", new RefColumn("GROUP_EL_OBJ_ID", "GKH_CONEL_GROUP_GREL", "GKH_DICT_GROUP_ELEM_OBJ", "ID"));
        }
    }
}