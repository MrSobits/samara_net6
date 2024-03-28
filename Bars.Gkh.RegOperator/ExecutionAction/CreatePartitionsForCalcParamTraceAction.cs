namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;

    using Dapper;

    /// <summary>
    /// Партиционирование больших таблиц и связанных с ними
    /// </summary>
    public class CreatePartitionsForCalcParamTraceAction : BaseExecutionAction
    {
        /// <summary>
        /// Таймаут на команду
        /// </summary>
        private readonly int CommandTimeOut = 3600 * 10;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => this.Name;

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Рефакторинг - Создание партиций для regop_calc_param_trace";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;
            var sessions = container.Resolve<ISessionProvider>();
            var sql = string.Empty;
            using (var connection = sessions.OpenStatelessSession().Connection)
            {
                using (var tr = connection.BeginTransaction())
                {
                    try
                    {
                        //процедура создания партиций для regop_calc_param_trace
                        sql = @"CREATE OR REPLACE FUNCTION public.create_partitions_for_calc_param_trace() 
                                RETURNS void
                                LANGUAGE plpgsql
                                AS $$
                                DECLARE
                                row record;                            
                                table_name varchar;
                                BEGIN 
                                 SET SEARCH_PATH to public;

                         DROP TABLE IF EXISTS t_charge_guids;
                         CREATE TEMP TABLE t_charge_guids AS 
                         SELECT guid, period_id FROM regop_pers_acc_charge;

                         CREATE INDEX ON t_charge_guids(guid);
                         ANALYZE t_charge_guids;

                         DROP TABLE IF EXISTS t_calc_param;
                         CREATE TEMP TABLE t_calc_param AS 
                         SELECT tr.id, tr.object_version, tr.object_create_date, tr.object_edit_date, tr.calc_guid, tr.calc_type, tr.date_start, tr.param_values, tr.date_end, ch.period_id
                         FROM REGOP_CALC_PARAM_TRACE tr JOIN t_charge_guids ch ON tr.calc_guid=ch.guid;

                                 CREATE INDEX ON t_calc_param(period_id);
                                 ANALYZE t_calc_param;  
                                 
                                 FOR row IN 
                                    SELECT DISTINCT id as period_id FROM regop_period ORDER BY 1
                                 LOOP 
                                  table_name:='REGOP_CALC_PARAM_TRACE_PERIOD_'||row.period_id;
                                  
                                   EXECUTE ' CREATE TABLE IF NOT EXISTS  '||table_name||
                                          ' (LIKE REGOP_CALC_PARAM_TRACE INCLUDING ALL, 
                                             CONSTRAINT REGOP_CALC_PARAM_TRACE_CONSTRAIN 
                                            CHECK (period_id ='||row.period_id||' ))
                                            INHERITS (REGOP_CALC_PARAM_TRACE); ' ;                              
                                
                                  
                                    --вставляем данные в партицию
                                  EXECUTE 'INSERT INTO '||table_name|| ' (id, object_version, object_create_date, object_edit_date, calc_guid, calc_type, date_start, param_values, date_end, period_id) 
                                          SELECT id, object_version, object_create_date, object_edit_date, calc_guid, calc_type, date_start, param_values, date_end, period_id
                                          FROM t_calc_param WHERE period_id='||row.period_id;  
                                  END LOOP;
                                END;  $$;";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        sql = @"SELECT TRUE
                            FROM   pg_attribute 
                            WHERE  attrelid = 'public.regop_calc_param_trace'::regclass  
                            AND    attname = 'period_id'
                            AND    NOT attisdropped";
                        if (!connection.ExecuteScalar<bool>(sql, transaction: tr))
                        {
                            sql = @"ALTER TABLE public.regop_calc_param_trace ADD COLUMN period_id BIGINT DEFAULT 0;";
                            connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
                        }
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }

                    tr.Commit();
                }

                using (var tr = connection.BeginTransaction())
                {
                    try
                    {
                        //создаем партиции для regop_calc_param_trace
                        sql = @"SELECT create_partitions_for_calc_param_trace() ; ";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        //очищаем мастер таблицу
                        sql = @"TRUNCATE ONLY public.regop_calc_param_trace ";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        //проверку на соответствие для regop_calc_param_trace не выполняем,
                        //т.к. некоторые записи могут не связаться с regop_pers_acc_charge (они удаляются при закрытии периода)

                        sql = @"DROP FUNCTION IF EXISTS public.regop_calc_param_trace();";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        //процедура для перенаправления вставляемой записи в партицию + вставка в мастер
                        sql = @"CREATE OR REPLACE FUNCTION public.insert_into_partition_by_period_id()
                             RETURNS TRIGGER AS $BODY$	
                            BEGIN
                            	EXECUTE format('INSERT INTO %I SELECT $1.*', TG_TABLE_NAME || '_period_'||NEW.period_id) USING NEW ;		
                              RETURN NEW;
                            END ;
                             $BODY$ 
                            LANGUAGE plpgsql VOLATILE COST 1;";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        //процедура удаления из мастер-таблицы (для NH)
                        sql = @"CREATE OR REPLACE FUNCTION public.delete_from_master_by_period_id()
                             RETURNS TRIGGER AS $BODY$	
                            BEGIN
                            	EXECUTE 'DELETE FROM ONLY '||TG_TABLE_NAME;		
                             RETURN NULL;  
                            END ;
                             $BODY$ 
                            LANGUAGE plpgsql VOLATILE COST 1;";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        //создаем BEFORE триггер на вставку в мастер-таблицу
                        sql = @"CREATE TRIGGER partitioning_trigger_regop_calc_param_trace
                            BEFORE INSERT ON public.regop_calc_param_trace
                            FOR EACH ROW
                            EXECUTE PROCEDURE public.insert_into_partition_by_period_id(); ";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        //создаем AFTER триггер на вставку в мастер-таблицу - костыль для работы с NH (он требует ожидаемое значение affected_rows)
                        sql = @"CREATE TRIGGER partitioning_trigger_regop_calc_param_trace_after
                            AFTER INSERT
                            ON public.regop_calc_param_trace
                            FOR EACH STATEMENT
                            EXECUTE PROCEDURE public.delete_from_master_by_period_id();";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
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