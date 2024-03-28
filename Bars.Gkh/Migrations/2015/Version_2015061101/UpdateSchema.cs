namespace Bars.Gkh.Migrations.Version_2015061101
{
    using System;
    using System.Threading;
    using System.Web;

    using global::Bars.B4.Modules.Ecm7.Framework;
	using Bars.B4.Application;
	using Bars.B4.DataAccess;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015061101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015061100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        if (ApplicationContext.Current.Configuration.DbDialect == DbDialect.PostgreSql)
	        {
		        Database.ExecuteNonQuery(@"CREATE OR REPLACE FUNCTION fix_sequence_references()
    RETURNS integer AS
$BODY$
    DECLARE
        problem RECORD;
    BEGIN
        FOR problem IN
        SELECT t.*
            FROM
               (SELECT constraint_column_usage.table_name AS tablename,
                       constraint_column_usage.column_name AS columnname,
                       (constraint_column_usage.table_name || '_id_seq') AS expected_sequencename,
                       replace(replace(columns.column_default,'''::regclass)',''),'nextval(''','') AS sequencename
                FROM information_schema.constraint_column_usage,
                     information_schema.columns
                WHERE constraint_column_usage.table_schema ='public'
                  AND columns.table_schema = 'public'
                  AND columns.table_name=constraint_column_usage.table_name
                  AND constraint_column_usage.column_name = columns.column_name
                  AND columns.column_default IS NOT NULL) t
            WHERE t.sequencename != t.expected_sequencename AND t.sequencename != '0'
        LOOP 
            IF NOT EXISTS (SELECT * FROM information_schema.sequences WHERE sequence_schema = 'public' and sequence_name = problem.expected_sequencename) THEN
                EXECUTE 'CREATE SEQUENCE ' || problem.expected_sequencename || ' INCREMENT 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1;';
            END IF;
            EXECUTE 'ALTER TABLE ' || problem.tablename || ' ALTER COLUMN ' || problem.columnname || ' SET DEFAULT nextval(''' || problem.expected_sequencename || '''::regclass);';
        END LOOP;
        PERFORM rebuilt_sequences();
        RETURN 1;
    END;
$BODY$
LANGUAGE plpgsql VOLATILE
COST 100;");

		        Database.ExecuteNonQuery("select * from fix_sequence_references();");

                throw new Exception();
            }
        }

        public override void Down()
        {
        }
    }
}