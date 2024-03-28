namespace Bars.Gkh.Migrations.Version_2013073100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013073100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013073000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "GKH_OBJ_CHARACT_VAL",
               new Column("VALUE", DbType.String, 300),
               new RefColumn("REALOBJ_ELEMENT_ID", "GKH_CHARACTVAL_EO", "GKH_REALOBJ_ELEMENT_OBJ", "ID"),
               new RefColumn("CHARACT_ID", "GKH_CHARACTVAL_CH", "GKH_DICT_CHARACTS", "ID"));

            Database.AddIndex("UNIQ_CHARACT_VAL", true, "GKH_OBJ_CHARACT_VAL", "REALOBJ_ELEMENT_ID", "CHARACT_ID");

            Database.AddColumn("GKH_DICT_CHARACTS", new Column("CODE", DbType.String, 300));
            Database.AddIndex("CHARACTS_CODE", true, "GKH_DICT_CHARACTS", "CODE");


            Database.AddColumn("GKH_REALITY_OBJECT", new Column("PRIV_DATE_FAPARTMENT", DbType.DateTime));
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("BUILD_YEAR", DbType.Int32));
            Database.AddRefColumn("GKH_REALITY_OBJECT", new RefColumn("TYPE_PROJECT_ID", "GKH_REALOBJ_TPROG", "GKH_DICT_TYPE_PROJ", "ID"));


            Database.AddColumn("GKH_DICT_GROUP_ELEM_OBJ", new Column("IS_REQUIRED_HOUSE_LAW", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GKH_DICT_GROUP_ELEM_OBJ", new Column("IS_INCLUD_PROGRAM_SUBJ", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GKH_DICT_GROUP_ELEM_OBJ", new Column("IS_COMPLEX_OBJECT", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveIndex("UNIQ_CHARACT_VAL", "GKH_OBJ_CHARACT_VAL");
            Database.RemoveTable("GKH_OBJ_CHARACT_VAL");

            Database.RemoveIndex("CHARACTS_CODE", "GKH_DICT_CHARACTS");
            Database.RemoveColumn("GKH_DICT_CHARACTS", "CODE");

            Database.RemoveColumn("GKH_REALITY_OBJECT", "PRIV_DATE_FAPARTMENT");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "BUILD_YEAR");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "TYPE_PROJECT_ID");

            Database.RemoveColumn("GKH_DICT_GROUP_ELEM_OBJ", "IS_REQUIRED_HOUSE_LAW");
            Database.RemoveColumn("GKH_DICT_GROUP_ELEM_OBJ", "IS_INCLUD_PROGRAM_SUBJ");
            Database.RemoveColumn("GKH_DICT_GROUP_ELEM_OBJ", "IS_COMPLEX_OBJECT");
        }
    }
}