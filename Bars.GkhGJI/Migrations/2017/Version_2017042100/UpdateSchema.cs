namespace Bars.GkhGji.Migrations._2017.Version_2017042100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017042100")]
    [MigrationDependsOn(typeof(Version_2017040300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (!this.Database.TableExists("GJI_BASEJURPERSON_CONTRAGENT"))
            {
                this.Database.AddEntityTable(
                    "GJI_BASEJURPERSON_CONTRAGENT",
                    new RefColumn("BASEJURPERSON_ID", ColumnProperty.NotNull, "GJI_JURCONTR_JUR", "GJI_INSPECTION_JURPERSON", "ID"),
                    new RefColumn("CONTRAGENT_ID", "GJI_JURCONT_CONTR", "GKH_CONTRAGENT", "ID"));
            }

            if (!this.Database.TableExists("GJI_NSO_DISP_VERIFSUBJ"))
            {
                this.Database.AddEntityTable(
                    "GJI_NSO_DISP_VERIFSUBJ",
                    new RefColumn("SURVEY_SUBJECT_ID", ColumnProperty.Null, "GJI_DISP_VERIFSUBJ_SSD", "GJI_DICT_SURVEY_SUBJ", "ID"),
                    new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "GJI_DISP_VERIFSUBJ_D", "GJI_DISPOSAL", "ID"));
            }

            if (!this.Database.TableExists("GJI_NSO_DISPOSAL_SURVEY_OBJ"))
            {
                this.Database.AddEntityTable(
                    "GJI_NSO_DISPOSAL_SURVEY_OBJ",
                    new RefColumn("DISPOSAL_ID", "GJI_NSO_DISP_SURVEY_OBJ_D", "GJI_DISPOSAL", "ID"),
                    new RefColumn("SURVEY_OBJ_ID", "GJI_NSO_DISP_SURVEY_OBJ_O", "GJI_DICT_SURVEY_OBJ", "ID"));
            }

            if (!this.Database.TableExists("GJI_NSO_DISPOSAL_SURVEY_OBJ"))
            {
                this.Database.AddEntityTable(
                    "GJI_NSO_DISPOSAL_INSPFOUND",
                    new RefColumn("DISPOSAL_ID", "GJI_NSO_DISP_IFOUND_D", "GJI_DISPOSAL", "ID"),
                    new RefColumn("INSPFOUND_ID", "GJI_NSO_DISP_IFOUND_F", "GKH_DICT_NORMATIVE_DOC", "ID"));
            }

            if (!this.Database.TableExists("GJI_NSO_DISPOSAL_INSPFOUNDCHECK"))
            {
                this.Database.AddEntityTable(
                    "GJI_NSO_DISPOSAL_INSPFOUNDCHECK",
                    new RefColumn("DISPOSAL_ID", "GJI_NSO_DISP_IFOUND_DC", "GJI_DISPOSAL", "ID"),
                    new RefColumn("INSPFOUND_ID", "GJI_NSO_DISP_IFOUND_FC", "GKH_DICT_NORMATIVE_DOC", "ID"));
            }

            if (!this.Database.TableExists("GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC"))
            {
                this.Database.AddEntityTable(
                    "GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC",
                    new RefColumn("FOUND_CHECK_ID", "GJI_FOUND_CHECK_NORM_DOC_FOUND_CHECK_ID", "GJI_NSO_DISPOSAL_INSPFOUNDCHECK", "ID"),
                    new RefColumn("DOC_ITEM_ID", "GJI_FOUND_CHECK_NORM_DOC_DOC_ITEM_ID", "GKH_DICT_NORMATIVE_DOC_ITEM", "ID"));
            }

            if (!this.Database.TableExists("GJI_NSO_DISPOSAL_ADMREG"))
            {
                this.Database.AddEntityTable(
                    "GJI_NSO_DISPOSAL_ADMREG",
                    new RefColumn("DISPOSAL_ID", "GJI_NSO_DISP_ADMREG_D", "GJI_DISPOSAL", "ID"),
                    new RefColumn("ADMREG_ID", "GJI_NSO_DISP_ADMREG_AR", "GKH_DICT_NORMATIVE_DOC", "ID"));
            }

            if (!this.Database.TableExists("GJI_NSO_ACT_PROVDOC"))
            {
                this.Database.AddEntityTable(
                    "GJI_NSO_ACT_PROVDOC",
                    new Column("DATE_PROVIDED", DbType.DateTime),
                    new RefColumn("PROVDOC_ID", ColumnProperty.NotNull, "GJI_NSO_ACT_PROVDOC_P", "GJI_DICT_PROVIDEDDOCUMENT", "ID"),
                    new RefColumn("ACT_ID", ColumnProperty.NotNull, "GJI_NSO_ACT_PROVDOC_A", "GJI_ACTCHECK", "ID"));
            }
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_BASEJURPERSON_CONTRAGENT");
            this.Database.RemoveTable("GJI_NSO_DISP_VERIFSUBJ");
            this.Database.RemoveTable("GJI_NSO_DISPOSAL_SURVEY_OBJ");
            this.Database.RemoveTable("GJI_NSO_DISPOSAL_INSPFOUND");
            this.Database.RemoveTable("GJI_NSO_DISPOSAL_INSPFOUNDCHECK");
            this.Database.RemoveTable("GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC");
            this.Database.RemoveTable("GJI_NSO_DISPOSAL_ADMREG");
            this.Database.RemoveTable("GJI_NSO_ACT_PROVDOC");
        }
    }
}