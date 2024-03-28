namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017030100
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2017030100
    /// </summary>
    [Migration("2017030100")]
    [MigrationDependsOn(typeof(Version_2017021000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string Query = @"
                    begin;
                    drop table if exists snap_indexes;
                    create temp table snap_indexes 
                        (drop_text text,
                        create_text text);   
                    
                    insert into snap_indexes (drop_text, create_text)
                    select concat('drop index ', indexname,';') as drop_text, indexdef||';' as create_text
                    	from pg_indexes 
                    	where tablename='regop_payment_doc_snapshot' and indexname not like '%pkey%';                  
                    
                    CREATE OR REPLACE FUNCTION create_pay_doc_type() 
                    RETURNS void 
                    LANGUAGE plpgsql
                    AS
                    $$
                    DECLARE
                    _sql_text text;
                    BEGIN 
                    	FOR _sql_text IN 
                    		select drop_text from snap_indexes
                    	LOOP
                    		execute _sql_text;   
                    	END LOOP;
                     
                        ALTER TABLE public.regop_payment_doc_snapshot 
                            ADD COLUMN doc_type SMALLINT NOT NULL DEFAULT 0;
                    
                    	FOR _sql_text IN 
                    		select create_text from snap_indexes
                    	LOOP
                    		execute _sql_text;  
                    	END LOOP;
                    END;
                    $$;
                    select create_pay_doc_type();
                    
                    drop function create_pay_doc_type();
                    drop table if exists snap_indexes;
                    
                    analyze regop_payment_doc_snapshot;
                    end;";

        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            var oldTimeout = this.Database.CommandTimeout;
            this.Database.CommandTimeout = 2 * 60 * 60;

            this.Database.ExecuteNonQuery(Query);
            this.Database.CommandTimeout = oldTimeout;
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "DOC_TYPE");
        }
    }
}
