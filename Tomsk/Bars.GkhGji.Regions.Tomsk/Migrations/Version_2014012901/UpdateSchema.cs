namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014012901
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014012900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("GJI_DISPOSAL_SUBJ_VERIF");
            Database.RemoveTable("GJI_DICT_VERIF_SUBJECT");
        }

        public override void Down()
        {
            //-----Справочник Предмет проверки
            Database.AddEntityTable(
                "GJI_DICT_VERIF_SUBJECT",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300));

            Database.AddEntityTable(
                "GJI_DISPOSAL_SUBJ_VERIF",
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "GJI_DISP_SUBJ", "GJI_DISPOSAL", "ID"),
                new RefColumn("SUBJ_VERIF_ID", ColumnProperty.NotNull, "GJI_DISP_SUBJ_VER", "GJI_DICT_VERIF_SUBJECT", "ID"));
        }
    }
}
