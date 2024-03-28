namespace Bars.GkhGji.Migrations.Version_2013032001
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //---
            Database.AddColumn("GJI_BUISNES_NOTIF", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_DICT_SERV_JURID", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_DICT_KIND_WORK", new Column("EXTERNAL_ID", DbType.String, 36));
            //----

            //добавление поля статус к сущности устав тсж (деятельность тсж - уставы)
            Database.AddRefColumn("GJI_ACT_TSJ_STATUTE", new RefColumn("STATE_ID", "GJI_ACT_TSJ_ST_ST", "B4_STATE", "ID"));
            //удаление поля статус у деятельности тсж
            Database.RemoveColumn("GJI_ACTIVITY_TSJ", "STATE_ID");
            //----

            //поля для конвертации
            Database.AddColumn("GJI_ACT_TSJ_ARTICLE", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_ACTIVITY_TSJ", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_PROT_REAL_OBJ", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_ACT_TSJ_PROTOCOL", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_ACT_TSJ_REAL_OBJ", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_ACT_TSJ_STATUTE", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_DICT_KIND_PROT", new Column("EXTERNAL_ID", DbType.String, 36));
            //увеличиваем длину колонки, т.к. в старой базе были очень длинные названия
            Database.ChangeColumn("GJI_DICT_ARTICLE_TSJ", new Column("NAME", DbType.String, 1000));
            Database.AddColumn("GJI_DICT_ARTICLE_TSJ", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_DICT_ARTICLE_TSJ", new Column("ARTICLE", DbType.String, 250));
            //-----

            Database.AddColumn("GJI_RESOLPROS_ANNEX", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_RESOLPROS_ARTLAW", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_RESOLPROS_ROBJECT", new Column("EXTERNAL_ID", DbType.String, 36));
            //----

            //поля для конвертации
            Database.AddColumn("GJI_DICT_HEATSEASONPERIOD", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_HEATSEASON", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_HEATSEASON_DOCUMENT", new Column("EXTERNAL_ID", DbType.String, 36));
            //----
            Database.AddColumn("GJI_RESOLUTION_PAYFINE", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_PRESENTATION_ANNEX", new Column("EXTERNAL_ID", DbType.String, 36));
            //----
            Database.AddColumn("GJI_ACTCHECK_INSPECTPART", new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GJI_DICT_INSPECTEDPART", new Column("EXTERNAL_ID", DbType.String, 36));
            //---
            //подтематика обращения
            Database.AddEntityTable("GJI_DICT_STAT_SUB_SUBJECT",
                new Column("NAME", DbType.String, 300),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_ST_SUB_SUBJ_N", false, "GJI_DICT_STAT_SUB_SUBJECT", "NAME");
            Database.AddIndex("IND_GJI_ST_SUB_SUBJ_C", false, "GJI_DICT_STAT_SUB_SUBJECT", "CODE");

            //связь между подтематикой и характеристикой
            Database.AddEntityTable("GJI_DICT_STATSUBSUBJ_FEAT",
                new Column("FEATURE_VIOL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("SUB_SUBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_ST_SUB_SUBJ_FEAT_F", false, "GJI_DICT_STATSUBSUBJ_FEAT", "FEATURE_VIOL_ID");
            Database.AddIndex("IND_GJI_ST_SUB_SUBJ_FEAT_S", false, "GJI_DICT_STATSUBSUBJ_FEAT", "SUB_SUBJECT_ID");
            Database.AddForeignKey("FK_GJI_ST_SUB_SUBJ_FEAT_S", "GJI_DICT_STATSUBSUBJ_FEAT", "SUB_SUBJECT_ID", "GJI_DICT_STAT_SUB_SUBJECT", "ID");
            Database.AddForeignKey("FK_GJI_ST_SUB_SUBJ_FEAT_F", "GJI_DICT_STATSUBSUBJ_FEAT", "FEATURE_VIOL_ID", "GJI_DICT_FEATUREVIOL", "ID");

            //подтематика тематики 
            Database.AddEntityTable("GJI_DICT_STATSUBJ_SUBSUBJ",
                new Column("SUBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("SUBSUBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_STSUB_SUSUBJ_S", false, "GJI_DICT_STATSUBJ_SUBSUBJ", "SUBJECT_ID");
            Database.AddIndex("IND_GJI_STSUB_SUSUBJ_SS", false, "GJI_DICT_STATSUBJ_SUBSUBJ", "SUBSUBJECT_ID");
            Database.AddForeignKey("FK_GJI_STSUB_SUSUBJ_SS", "GJI_DICT_STATSUBJ_SUBSUBJ", "SUBSUBJECT_ID", "GJI_DICT_STAT_SUB_SUBJECT", "ID");
            Database.AddForeignKey("FK_GJI_STSUB_SUSUBJ_S", "GJI_DICT_STATSUBJ_SUBSUBJ", "SUBJECT_ID", "GJI_DICT_STATEMENT_SUBJ", "ID");
            //----
            //удаляем неправильный внешний ключ
            Database.RemoveConstraint("gji_appcit_statsubj", "fk_appcitstat_stat");
            Database.RemoveIndex("ind_appcitstat_stat", "gji_appcit_statsubj");

            //добавляем правильный внешний ключ с таким же именем
            Database.AddForeignKey("FK_GJI_APPCIT_ST_ST", "gji_appcit_statsubj", "statement_subject_id", "gji_dict_statement_subj", "id");
            Database.AddIndex("IND_GJI_APPCIT_ST_ST", false, "gji_appcit_statsubj", "statement_subject_id");
            //----
            Database.AddRefColumn("GJI_APPCIT_STATSUBJ", new RefColumn("SUBSUBJECT_ID", "GJI_APPCIT_ST_SUBJ", "GJI_DICT_STAT_SUB_SUBJECT", "ID"));
            Database.AddRefColumn("GJI_APPCIT_STATSUBJ", new RefColumn("FEATURE_ID", "GJI_APPCIT_ST_FEAT", "GJI_DICT_FEATUREVIOL", "ID"));
            //----

            Database.ChangeColumn("GJI_INSPECTION_PROSCLAIM", new Column("DOCUMENT_NUMBER", DbType.String, 250));

            Database.ChangeColumn("GJI_APPEAL_CITIZENS", new Column("GJI_APPEAL_ID", DbType.Int32, ColumnProperty.Null));
            Database.ChangeColumn("GJI_STATEMENT_ANSWER", new Column("DESCRIPTION", DbType.String, 2000));

            //----
            Database.AddColumn("GJI_DICT_STATSUBJ_SUBSUBJ", new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_STATSUBJ_SUBSUBJ", "EXTERNAL_ID");
            //----

            Database.RemoveColumn("GJI_APPCIT_STATSUBJ", "SUBSUBJECT_ID");
            Database.RemoveColumn("GJI_APPCIT_STATSUBJ", "FEATURE_ID");
            //----
            
            Database.RemoveConstraint("GJI_DICT_STATSUBJ_SUBSUBJ", "FK_GJI_STSUB_SUSUBJ_SS");
            Database.RemoveConstraint("GJI_DICT_STATSUBJ_SUBSUBJ", "FK_GJI_STSUB_SUSUBJ_S");
            Database.RemoveConstraint("GJI_DICT_STATSUBSUBJ_FEAT", "FK_GJI_ST_SUB_SUBJ_FEAT_S");
            Database.RemoveConstraint("GJI_DICT_STATSUBSUBJ_FEAT", "FK_GJI_ST_SUB_SUBJ_FEAT_F");
            
            Database.RemoveTable("GJI_DICT_STAT_SUB_SUBJECT");
            Database.RemoveTable("GJI_DICT_STATSUBSUBJ_FEAT");
            Database.RemoveTable("GJI_DICT_STATSUBJ_SUBSUBJ");

            //----
            Database.RemoveColumn("GJI_ACTCHECK_INSPECTPART", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_DICT_INSPECTEDPART", "EXTERNAL_ID");
            //----
            Database.RemoveColumn("GJI_RESOLUTION_PAYFINE", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_PRESENTATION_ANNEX", "EXTERNAL_ID");
            //----
            Database.RemoveColumn("GJI_DICT_HEATSEASONPERIOD", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_HEATSEASON", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_HEATSEASON_DOCUMENT", "EXTERNAL_ID");
            //----
            Database.RemoveColumn("GJI_RESOLPROS_ANNEX", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_RESOLPROS_ARTLAW", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_RESOLPROS_ROBJECT", "EXTERNAL_ID");
        
            //---
            Database.RemoveColumn("GJI_ACT_TSJ_ARTICLE", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_ACTIVITY_TSJ", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_PROT_REAL_OBJ", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_ACT_TSJ_PROTOCOL", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_ACT_TSJ_REAL_OBJ", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_ACT_TSJ_STATUTE", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_DICT_KIND_PROT", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_DICT_ARTICLE_TSJ", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_DICT_ARTICLE_TSJ", "ARTICLE");
            //---
            
            Database.RemoveColumn("GJI_ACT_TSJ_STATUTE", "STATE_ID");
            Database.AddRefColumn("GJI_ACTIVITY_TSJ", new RefColumn("STATE_ID", "GJI_ACT_TSJ_STATE", "B4_STATE", "ID"));
            //----
            Database.RemoveColumn("GJI_BUISNES_NOTIF", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_DICT_SERV_JURID", "EXTERNAL_ID");
            Database.RemoveColumn("GJI_DICT_KIND_WORK", "EXTERNAL_ID");
        }
    }
}