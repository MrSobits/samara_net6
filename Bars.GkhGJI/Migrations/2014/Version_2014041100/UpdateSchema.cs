namespace Bars.GkhGji.Migrations.Version_2014041100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014041001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Справочник План проверок юр. лиц
            Database.AddEntityTable(
                "GJI_DICT_PLANACTION",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));
            Database.AddIndex("IND_GJI_DICT_PLANACTION_NAME", false, "GJI_DICT_PLANACTION", "NAME");
            //-----

            //-----Основание проверки ГЖИ - Проверка по плану мероприятий
            Database.AddTable(
                "GJI_INSPECTION_PLANACTION",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PLAN_ID", DbType.Int64, 22),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime),
                new Column("COUNT_DAYS", DbType.Int16, 2),
                new Column("REQUIREMENT", DbType.String, 2000),
                new Column("PERSON_ADDRESS", DbType.String, 500));
            Database.AddIndex("IND_GJI_INSP_PLNACT_P", false, "GJI_INSPECTION_PLANACTION", "PLAN_ID");
            Database.AddForeignKey("FK_GJI_INSP_PLNACT_P", "GJI_INSPECTION_PLANACTION", "PLAN_ID", "GJI_DICT_PLANACTION", "ID");
            Database.AddForeignKey("FK_GJI_INSP_PLNACT_I", "GJI_INSPECTION_PLANACTION", "ID", "GJI_INSPECTION", "ID");
            //-----
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_INSPECTION_PLANACTION", "FK_GJI_INSPECTION_PLANACTION_P");
            Database.RemoveConstraint("GJI_INSPECTION_PLANACTION", "FK_GJI_INSPECTION_PLANACTION_I");

            Database.RemoveTable("GJI_INSPECTION_PLANACTION");
            Database.RemoveTable("GJI_DICT_PLANACTION");
        }
    }
}