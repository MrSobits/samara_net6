namespace Bars.Gkh.Migrations.Version_2013102200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013101601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_DICT_PROBLEM_PLACE", new Column("NAME", DbType.String, 250, ColumnProperty.NotNull));

            Database.RemoveColumn("GKH_CIT_SUG", "PROBLEM_PLACE");

            Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("PROBLEM_PLACE_ID", "GKH_CIT_SUG_PROBLEM_PLACE", "GKH_DICT_PROBLEM_PLACE", "ID"));
            Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("EXECUTOR_MANORG_ID", "GKH_CIT_SUG_MANORG", "GKH_MANAGING_ORGANIZATION", "ID"));
            Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("EXECUTOR_MUNICIPALITY_ID", "GKH_CIT_SUG_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("EXECUTOR_ZONAL_INSP_ID", "GKH_CIT_SUG_ZONAL_INSP", "GKH_DICT_ZONAINSP", "ID"));
            Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("STATE_ID", "GKH_CIT_SUG_STATE", "B4_STATE", "ID"));
            Database.AddColumn("GKH_CIT_SUG", new Column("SUGGESTION_ADDRESS", DbType.String));
            Database.AddColumn("GKH_CIT_SUG", new Column("SUGGESTION_YEAR", DbType.Int64));
            Database.AddColumn("GKH_CIT_SUG", new Column("SUGGESTION_NUMBER", DbType.Int64));

            Database.AddColumn("GKH_CIT_SUG", new Column("BODY", DbType.String));
            Database.AddColumn("GKH_CIT_SUG", new Column("ROOM", DbType.String));
            Database.AddColumn("GKH_CIT_SUG", new Column("APARTMENT", DbType.Int64));
            Database.AddColumn("GKH_CIT_SUG", new Column("MUNICIPALITY_CODE_KLADR", DbType.String));
            Database.AddColumn("GKH_CIT_SUG", new Column("CITY_CODE_KLADR", DbType.String));
            Database.AddColumn("GKH_CIT_SUG", new Column("STREET_CODE_KLADR", DbType.String));
            Database.AddColumn("GKH_CIT_SUG", new Column("HOUSE", DbType.String));
            Database.AddColumn("GKH_CIT_SUG", new Column("ADDRESS_FULL_CODE", DbType.String));

            Database.AddColumn("GKH_CIT_SUG_COMMENT", new Column("QUESTION", DbType.String));
            Database.AddColumn("GKH_CIT_SUG_COMMENT", new Column("ANSWER", DbType.String));
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "TEXT");
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "IS_ANSWER");

            Database.AddEntityTable(
                "GKH_CIT_SUG_FILES",
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUMBER", DbType.String),
                new Column("DESCRIPTION", DbType.String),
                new Column("IS_ANSWER", DbType.Boolean),
                new RefColumn("SUGGESTION_ID", "GKH_CIT_SUG_FILES_SUG", "GKH_CIT_SUG", "ID"),
                new RefColumn("DOCUMENT_FILE_ID", "GKH_CIT_SUG_FILES_FILES", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable(
                "GKH_CIT_SUG_COMMENT_FILES",
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUMBER", DbType.String),
                new Column("DESCRIPTION", DbType.String),
                new Column("IS_ANSWER", DbType.Boolean),
                new RefColumn("SUG_COMMENT_ID", "GKH_SUG_COM_FILES_COMMENT", "GKH_CIT_SUG_COMMENT", "ID"),
                new RefColumn("DOCUMENT_FILE_ID", "GKH_SUG_COMMENT_FILES", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.AddColumn("GKH_CIT_SUG", new Column("PROBLEM_PLACE", DbType.String));

            Database.RemoveColumn("GKH_CIT_SUG", "PROBLEM_PLACE_ID");
            Database.RemoveColumn("GKH_CIT_SUG", "EXECUTOR_MANORG_ID");
            Database.RemoveColumn("GKH_CIT_SUG", "EXECUTOR_MUNICIPALITY_ID");
            Database.RemoveColumn("GKH_CIT_SUG", "EXECUTOR_ZONAL_INSP_ID");
            Database.RemoveColumn("GKH_CIT_SUG", "STATE_ID");
            Database.RemoveColumn("GKH_CIT_SUG", "SUGGESTION_ADDRESS");
            Database.RemoveColumn("GKH_CIT_SUG", "SUGGESTION_YEAR");
            Database.RemoveColumn("GKH_CIT_SUG", "SUGGESTION_NUMBER");

            Database.RemoveColumn("GKH_CIT_SUG", "BODY");
            Database.RemoveColumn("GKH_CIT_SUG", "ROOM");
            Database.RemoveColumn("GKH_CIT_SUG", "APARTMENT");
            Database.RemoveColumn("GKH_CIT_SUG", "MUNICIPALITY_CODE_KLADR");
            Database.RemoveColumn("GKH_CIT_SUG", "CITY_CODE_KLADR");
            Database.RemoveColumn("GKH_CIT_SUG", "STREET_CODE_KLADR");
            Database.RemoveColumn("GKH_CIT_SUG", "HOUSE");
            Database.RemoveColumn("GKH_CIT_SUG", "ADDRESS_FULL_CODE");

            Database.RemoveTable("GKH_DICT_PROBLEM_PLACE");
            
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "QUESTION");
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "ANSWER");
            Database.AddColumn("GKH_CIT_SUG_COMMENT", new Column("TEXT", DbType.String));
            Database.AddColumn("GKH_CIT_SUG_COMMENT", new Column("IS_ANSWER", DbType.Boolean));

            Database.RemoveTable("GKH_CIT_SUG_FILES");
            Database.RemoveTable("GKH_CIT_SUG_COMMENT_FILES");
        }
    }
}