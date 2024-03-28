namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;

    using Dapper;

    /// <summary>
    /// Действие проставляет is_affect входящим трансферам
    /// </summary>
    public class IsAffectAction : BaseExecutionAction
    {
        /// <summary>
        /// Код действия
        /// </summary>
        public static string Code = "IsAffectAction";

        /// <summary>
        /// Флаг обязательности выполнения
        /// </summary>
        public bool IsMandatory => false;

        /// <summary>
        /// Параметры задачи
        /// </summary>
        public BaseParams ExecutionParams { get; set; }

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Проставление флага is_affect всем входящим трансферам";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Проставление флага is_affect";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => IsAffectAction.Execute;

        /// <summary>
        /// IoC
        /// </summary>
        private static BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;

            ISessionProvider sessions = container.Resolve<ISessionProvider>();

            using (var connection = sessions.OpenStatelessSession().Connection)
            {
                using (var tr = connection.BeginTransaction())
                {
                    try
                    {
                        //3. проставляем всем входящим трансферам is_affect=true
                        connection.Execute(@"
                    --Проставление флага is_affect
                    begin;
                    drop table if exists transfer_indexes;
                    create temp table transfer_indexes 
                        (drop_text text,
                        create_text text);   
                    
                    insert into transfer_indexes (drop_text, create_text)
                    select concat('drop index ', indexname,';') as drop_text, indexdef||';' as create_text
                    	from pg_indexes 
                    	where tablename='regop_transfer' and indexname not like '%pkey%';
                    
                    drop table if exists wallet ;
                    create temp table wallet 
                        (wallet_guid varchar(40));
                    
                    --функция
                    CREATE OR REPLACE FUNCTION set_is_affect() 
                    RETURNS void 
                    LANGUAGE plpgsql
                    AS
                    $$
                    DECLARE
                    _sql_text text;
                    BEGIN 
                    	FOR _sql_text IN 
                    		select drop_text from transfer_indexes
                    	LOOP
                    		execute _sql_text;   
                    	END LOOP;
                     
                    
                    	insert into wallet (wallet_guid)
                    		select wallet_guid from regop_wallet;
                    
                    	CREATE INDEX ind_wallet ON wallet USING btree (wallet_guid);
                    	analyze wallet;
                    
                    	update regop_transfer
                    		set is_affect=true
                    	from wallet
                    	where target_guid=wallet_guid
                    	and (not is_indirect or (reason is null and amount<0));
                    
                    	FOR _sql_text IN 
                    		select create_text from transfer_indexes
                    	LOOP
                    		execute _sql_text;  
                    	END LOOP;
                    END;
                    $$;
                    select set_is_affect();
                    
                    drop function set_is_affect();	
                    drop table if exists wallet ;
                    drop table if exists transfer_indexes;
                    
                    analyze regop_transfer;
                    end;
                    ", transaction: tr, commandTimeout: 10000);
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }

                    tr.Commit();
                }
            }

            return new BaseDataResult();
        }
    }
}