﻿namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.Utils;

    using Dapper;

    /// <summary>
    /// Партиционирование regop_pers_acc_period_summ
    /// </summary>
    public class CreatePartitionsForAcountPeriodSummaryAction : BaseExecutionAction
    {
        /// <summary>
        /// Таймаут на команду
        /// </summary>
        private readonly int CommandTimeOut = 3600 * 3;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Рефакторинг - Создание партиций для regop_pers_acc_period_summ";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Рефакторинг - Создание партиций для regop_pers_acc_period_summ";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;
            var sessions = container.Resolve<ISessionProvider>();
            using (var connection = sessions.OpenStatelessSession().Connection)
            {
                using (var tr = connection.BeginTransaction())
                {
                    try
                    {
                        //процедура создания партиций по полю - для простых случаев
                        var sql = @"CREATE OR REPLACE FUNCTION public.create_partitions_by_name(t_name varchar,p_field varchar) 
                            RETURNS void
                            LANGUAGE plpgsql
                            AS $$
                            DECLARE
                            row record;
                            affected_rows BIGINT DEFAULT 0;
                            table_name varchar;
                            BEGIN 
                             SET SEARCH_PATH to public;
                             FOR row IN 
                            		SELECT DISTINCT id as period_id FROM regop_period ORDER BY 1
                             LOOP 
                            	table_name:=t_name||'_PERIOD_'||row.period_id;
                            
                            	RAISE NOTICE '%','CREATE partition';
                             	EXECUTE ' CREATE TABLE IF NOT EXISTS  '||table_name||
                            					' (LIKE '||t_name||' INCLUDING ALL, 
                            						 CONSTRAINT  '||t_name||'_constrain 
                            						CHECK ('||p_field||' ='||row.period_id||' ))
                            					  INHERITS ('||t_name||'); ' ;
                            
                            
                            	RAISE NOTICE '%','insert into partition';
                             --вставляем данные в партицию
                            	EXECUTE 'INSERT INTO '||table_name|| ' 
                            				  SELECT *
                            					FROM '||t_name||' 
                            					WHERE '||p_field||'='||row.period_id;	
                            				
                            			GET DIAGNOSTICS affected_rows = ROW_COUNT;	
                            			RAISE NOTICE 'month=%',row.period_id||'; Вставлено записей='||affected_rows;
                            	END LOOP;
                            END; $$; ";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        //создаем партиции
                        sql = @"SELECT create_partitions_by_name('regop_pers_acc_period_summ', 'period_id');";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        if (this.TableExists(connection, "regop_penalty_change"))
                        {
                            //снимаем FK на эту таблицу, т.к. связанные таблицы подлежат удалению в дальнейшем 
                            sql = @"ALTER TABLE regop_penalty_change DROP CONSTRAINT IF EXISTS fk_regop_penalty_change_aps;";
                            connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
                        }
                        if (this.TableExists(connection, "regop_saldo_change"))
                        {
                            sql = @"ALTER TABLE regop_saldo_change DROP CONSTRAINT IF EXISTS fk_regop_summary_saldo_change_sm;";
                            connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
                        }
                        if (this.TableExists(connection, "regop_summary_saldo_change"))
                        {
                            sql = @"ALTER TABLE regop_summary_saldo_change DROP CONSTRAINT IF EXISTS fk_regop_summary_saldo_change_sm2;";
                            connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                            sql = @"ALTER TABLE regop_summary_saldo_change DROP CONSTRAINT IF EXISTS fk_regop_summary_saldo_change_sm;";
                            connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
                        }

                        //получаем кол-во строк в мастер-таблице
                        sql = @"SELECT count(1) FROM ONLY public.regop_pers_acc_period_summ";
                        var countInMaster = connection.ExecuteScalar<long>(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        //очищаем мастер таблицу
                        sql = @"TRUNCATE ONLY public.regop_pers_acc_period_summ ";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        //получаем кол-во строк в партициях
                        sql = @"SELECT count(1) FROM public.regop_pers_acc_period_summ";
                        var countInPartitions = connection.ExecuteScalar<long>(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        if (countInPartitions != countInMaster)
                        {
                            throw new Exception("Кол-во строк в мастер-таблице regop_pers_acc_period_summ не совпадает с кол-вом строк в партициях");
                        }

                        sql = @"DROP FUNCTION IF EXISTS public.create_partitions_by_name(varchar,varchar);";
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
                        sql = @"CREATE TRIGGER partitioning_trigger_regop_pers_acc_period_summ
                            BEFORE INSERT ON public.regop_pers_acc_period_summ
                            FOR EACH ROW
                            EXECUTE PROCEDURE public.insert_into_partition_by_period_id(); ";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        //создаем AFTER триггер на вставку в мастер-таблицу - костыль для работы с NH (он требует ожидаемое значение affected_rows)
                        sql = @"CREATE TRIGGER partitioning_trigger_regop_pers_acc_period_summ_after
                            AFTER INSERT
                            ON public.regop_pers_acc_period_summ
                            FOR EACH STATEMENT
                            EXECUTE PROCEDURE public.delete_from_master_by_period_id();";
                        connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                        var listCommands = new List<string>();

                        sql =
                            $@"SELECT CONCAT('ALTER TABLE ',schemaname,'.',tablename,
                           ' ADD CONSTRAINT fk_',tablename,'_pa FOREIGN KEY (account_id)
                           REFERENCES ',schemaname,'.','regop_pers_acc ', '(id)',';') as command
                           FROM pg_tables WHERE schemaname='public' AND tablename LIKE 'regop_pers_acc_period_summ_period_%' ";

                        using (var reader = connection.ExecuteReader(sql, transaction: tr))
                        {
                            while (reader.Read())
                            {
                                listCommands.Add(reader["command"].ToString());
                            }
                        }

                        sql =
                            $@"SELECT CONCAT('ALTER TABLE ',schemaname,'.',tablename,
                           ' ADD CONSTRAINT fk_',tablename,'_period FOREIGN KEY (period_id)
                           REFERENCES ',schemaname,'.','regop_period ', '(id)',';') as command
                           FROM pg_tables WHERE schemaname='public' AND tablename LIKE 'regop_pers_acc_period_summ_period_%' ";

                        using (var reader = connection.ExecuteReader(sql, transaction: tr))
                        {
                            while (reader.Read())
                            {
                                listCommands.Add(reader["command"].ToString());
                            }
                        }

                        foreach (var command in listCommands)
                        {
                            connection.Execute(command, transaction: tr);
                        }
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

        /// <summary>
        /// Проверить существования таблицы
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private bool TableExists(IDbConnection conn, string tableName)
        {
            return conn.ExecuteScalar<bool>($"SELECT COUNT(1)>0 FROM pg_tables WHERE tablename={tableName.ToStringWithQuote()}");
        }
    }
}