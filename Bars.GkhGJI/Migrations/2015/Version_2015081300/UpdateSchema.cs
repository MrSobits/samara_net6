namespace Bars.GkhGji.Migrations._2015.Version_2015081300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015081300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015081100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_DICT_AUDIT_PURPOSE",
                new Column("NAME", DbType.String, 250),
                new Column("CODE", DbType.String, 250));

            if (!Database.TableExists("GJI_DICT_SURVEY_SUBJ"))
            {
                Database.AddEntityTable("GJI_DICT_SURVEY_SUBJ",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
                Database.AddIndex("IND_GJI_SURV_SUBJ_NAME", false, "GJI_DICT_SURVEY_SUBJ", "NAME");
                Database.AddIndex("IND_GJI_SURV_SUBJ_CODE", false, "GJI_DICT_SURVEY_SUBJ", "CODE");
            }

            Database.AddEntityTable(
                "GJI_DICT_APURP_SSUBJ",
                new RefColumn("AUDIT_PURP_ID", ColumnProperty.NotNull, "GJI_APURP_AUDIT_PURP", "GJI_DICT_AUDIT_PURPOSE", "ID"),
                new RefColumn("SURV_SUBJ_ID", ColumnProperty.NotNull, "GJI_APURP_SURV_SUBJ", "GJI_DICT_SURVEY_SUBJ", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_AUDIT_PURPOSE");

            Database.RemoveTable("GJI_DICT_SURVEY_SUBJ");

            Database.RemoveColumn("GJI_DICT_APURP_SSUBJ", "AUDIT_PURP_ID");
            Database.RemoveColumn("GJI_DICT_APURP_SSUBJ", "SURV_SUBJ_ID");
            Database.RemoveTable("GJI_DICT_APURP_SSUBJ");
        }
    }
}