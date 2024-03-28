namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.ExecutionActionResolver;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;
    using Bars.Gkh.RegOperator.Domain.Repository;

    using Castle.Windsor;

    using Dapper;

    /// <summary>
    /// Партиционирование больших таблиц и связанных с ними
    /// </summary>
    public class CreatePartitionsForMoneyOperationAction : IExecutionActionWithParams
    {
        /// <summary>
        /// Код действия
        /// </summary>
        public static string Code = "CreatePartitionsForMoneyOperationAction";

        string IExecutionAction.Code => CreatePartitionsForMoneyOperationAction.Code;

        /// <summary>
        /// Флаг обязательности выполнения
        /// </summary>
        public bool IsMandatory
        {
            get
            {
                var history = this.Container.ResolveDomain<ExecutionActionHistory>();
                var result =
                    history.GetAll() //если выполнился CreatePartitionsForTransferAction, то этот становится обязательным 
                        .Where(j => j.Code == CreatePartitionsForTransferAction.Code && j.Status == ExecutionActionStatus.Success)
                        .IsNotEmpty();
                return result;
            }
        }

        /// <summary>
        /// Параметры задачи
        /// </summary>
        public DynamicDictionary ExecutionParams { get; set; }

        /// <summary>
        /// Описание действия
        /// </summary>
        public string Description => this.Name;

        /// <summary>
        /// Название для отображения
        /// </summary>
        public string Name => "Рефакторинг - Создание партиций для regop_money_operation";

        /// <summary>
        /// Действие
        /// </summary>
        public Func<BaseDataResult> Action
        {
            get
            {
                return Execute;
            }
        }

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Таймаут на команду
        /// </summary>
        private int CommandTimeOut = 3600 * 3;

        private BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;
            var sessions = container.Resolve<ISessionProvider>();
            using (var connection = sessions.OpenStatelessSession().Connection)
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

                    //проставляем period_id в regop_money_operation
                    sql = @"  
                          --чистим regop_money_lock от блокировок операций, которые не имею трансферов
                          DELETE FROM public.regop_money_lock l
                          WHERE EXISTS (SELECT 1 FROM public.regop_money_operation m 
                                        WHERE NOT EXISTS (SELECT 1 FROM public.regop_transfer t WHERE t.op_id=m.id)
                                        AND (m.id=l.cancel_operation_id OR m.id=l.operation_id));";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                    sql = @"
                          DELETE FROM public.regop_bank_stmnt_op b 
                          WHERE EXISTS  (SELECT 1 FROM public.regop_money_operation m
                                         WHERE NOT EXISTS (SELECT 1 FROM public.regop_transfer t WHERE t.op_id=m.id)
                                         AND b.op_id=m.id);";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                    sql = @"--чистим regop_money_operation от левых записей
                          DELETE FROM public.regop_money_operation m WHERE NOT EXISTS (SELECT 1 FROM public.regop_transfer t WHERE t.op_id=m.id); ";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                    sql = @"
                          DROP INDEX IF EXISTS public.idx_regop_money_op_g;
                          DROP INDEX IF EXISTS public.idx_regop_money_op_o_g;
                          DROP INDEX IF EXISTS public.ind_regop_money_op_doc;
                          DROP INDEX IF EXISTS public.ind_regop_money_op_op_nnull;
                          DROP INDEX IF EXISTS public.ind_regop_money_op_op_null;

                          --устанавливаем date_operation из regop_transfer                           
                          UPDATE public.regop_money_operation m
                          SET operation_date=t.operation_date
                          FROM public.regop_transfer t 
                          WHERE t.op_id=m.id
                          AND NOT EXISTS (SELECT 1 FROM public.regop_period r WHERE  m.operation_date::DATE BETWEEN cstart 
                                                                        AND COALESCE(r.cend, 'infinity')::DATE);
                          --добавляем колонку period_id
                          ALTER TABLE public.regop_money_operation ADD COLUMN period_id BIGINT NOT NULL DEFAULT 0;
                          
                          --устанавливаем период
                          UPDATE public.regop_money_operation m 
                          SET period_id=r.id
                          FROM public.regop_period r WHERE m.operation_date::DATE BETWEEN cstart 
                                                      AND COALESCE(r.cend, 'infinity')::DATE;

                          CREATE UNIQUE INDEX idx_regop_money_op_g ON regop_money_operation USING btree (op_guid);
                          CREATE INDEX idx_regop_money_op_o_g ON regop_money_operation USING btree (originator_guid);
                          CREATE INDEX ind_regop_money_op_doc ON regop_money_operation USING btree (document_id);
                          CREATE INDEX ind_regop_money_op_op_nnull ON regop_money_operation USING btree (canceled_op_id) WHERE (canceled_op_id IS NOT NULL);
                          CREATE INDEX ind_regop_money_op_op_null ON regop_money_operation USING btree (canceled_op_id) WHERE (canceled_op_id IS NULL);
                          ANALYZE public.regop_money_operation;";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                    //снимаем FK с regop_money_lock
                    sql = @" ALTER TABLE public.regop_money_lock DROP CONSTRAINT fk_mlock_cancel_op;
                             ALTER TABLE public.regop_money_lock DROP CONSTRAINT fk_money_lock_operation;";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
                    //снимаем FK с regop_bank_stmnt_op
                    sql = @" ALTER TABLE public.regop_bank_stmnt_op DROP CONSTRAINT fk_money_operation;";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
                    //снимаем FK с regop_transfer
                    sql = @" ALTER TABLE public.regop_transfer DROP CONSTRAINT fk_regop_tr_op;";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                    //создаем таблицу с PK для таблицы regop_money_operation - на нее будут ссылаться другие таблицы
                    sql = @"CREATE TABLE public.regop_money_operation_pk (id BIGINT NOT NULL PRIMARY KEY);";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
                    //копируем все pk из regop_money_operation
                    sql = @"INSERT INTO public.regop_money_operation_pk (id) 
                            SELECT id FROM public.regop_money_operation";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                    //создаем FK на новую таблицу
                    sql = @"ALTER TABLE public.regop_money_lock ADD CONSTRAINT fk_mlock_cancel_op 
                            FOREIGN KEY (cancel_operation_id) REFERENCES public.regop_money_operation_pk (id);
                            ALTER TABLE public.regop_money_lock ADD CONSTRAINT fk_money_lock_operation 
                            FOREIGN KEY (operation_id) REFERENCES public.regop_money_operation_pk (id);";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

                    sql = @"ALTER TABLE public.regop_bank_stmnt_op ADD CONSTRAINT fk_regop_bank_stmnt_op 
                           FOREIGN KEY (op_id) REFERENCES public.regop_money_operation_pk (id);";
                    connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
                    
                    //разбиваем regop_money_operation на пар
                    this.CreatePartitionsWithClear(connection, tr, "regop_money_operation");
                    this.CreateRulesOnPartitions(connection,tr,"regop_money_operation","regop_money_operation_pk");
                    this.CreateFkOnPartitions(connection, tr, "regop_transfer", "regop_money_operation_pk", "op_id", "id");

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

                    this.CreateTriggers(connection, tr, "regop_money_operation");
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }

                tr.Commit();
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Создает FK на партициях
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tr"></param>
        /// <param name="tableName">таблица на которой создается FK</param>
        /// <param name="fTableName">таблица на которую ссылается FK</param>
        /// <param name="column">колонка которая будет ссылаться</param>
        /// <param name="fColumn">колонка на которую ссылается FK</param>
        private void CreateFkOnPartitions(IDbConnection connection, IDbTransaction tr, string tableName, string fTableName, string column, string fColumn)
        {
            var listCommands = new List<string>();
            using (
                var reader =
                    connection.ExecuteReader(
                        $@"SELECT CONCAT('ALTER TABLE ',schemaname,'.',tablename,
                           ' ADD CONSTRAINT fk_',tablename,' FOREIGN KEY ({column})
                           REFERENCES ',schemaname,'.','{fTableName}', '({fColumn})',';') as command
                           FROM pg_tables WHERE tablename LIKE '{tableName}_period_%' ",
                        transaction: tr))
            {
                while (reader.Read())
                {
                    listCommands.Add(reader["command"].ToString());
                }
            }

            foreach (var command in listCommands)
            {
                connection.Execute(command, transaction: tr, commandTimeout: this.CommandTimeOut);
            }
        }

        /// <summary> 
        /// Создает триггеры на вставку в мастер таблицу
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tr"></param>
        /// <param name="tableName"></param>
        private void CreateTriggers(IDbConnection connection, IDbTransaction tr, string tableName)
        {
            string sql;

            //создаем BEFORE триггер на вставку в мастер-таблицу
            sql = $@"CREATE TRIGGER partitioning_trigger_{tableName}
                            BEFORE INSERT ON public.{tableName}
                            FOR EACH ROW
                            EXECUTE PROCEDURE public.insert_into_partition_by_period_id(); ";
            connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

            //создаем AFTER триггер на вставку в мастер-таблицу - костыль для работы с NH (он требует ожидаемое значение affected_rows)
            sql = $@"CREATE TRIGGER partitioning_trigger_{tableName}_after
                            AFTER INSERT
                            ON public.{tableName}
                            FOR EACH STATEMENT
                            EXECUTE PROCEDURE public.delete_from_master_by_period_id();";
            connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);
        }

        /// <summary>
        /// Создает правила на вставку и удаления, которое перенаправляет работу c PK на спец.таблицу
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tr"></param>
        /// <param name="tableName"></param>
        /// <param name="tableNameSurrogate"></param>
        private void CreateRulesOnPartitions(IDbConnection connection, IDbTransaction tr, string tableName, string tableNameSurrogate)
        {
            var listCommands = new List<string>();
            using (
                var reader =
                    connection.ExecuteReader(
                        $@"SELECT CONCAT('CREATE RULE ',tablename,'_insert_id_rule',' AS ON INSERT TO ',schemaname,'.',tablename, ' DO
                           INSERT INTO public.{tableNameSurrogate} VALUES (NEW.id);')
                           FROM pg_tables WHERE tablename LIKE '{tableName}_period%';",
                        transaction: tr))
            {
                while (reader.Read())
                {
                    listCommands.Add(reader["command"].ToString());
                }
            }

            using (
                var reader =
                    connection.ExecuteReader(
                        $@"SELECT CONCAT('CREATE RULE ',tablename,'_delete_id_rule',' AS ON DELETE TO ',schemaname,'.',tablename, ' DO
                           DELETE FROM public.{tableNameSurrogate} WHERE id= OLD.id;')
                           FROM pg_tables WHERE tablename LIKE '{tableName}_period_%';;",
                        transaction: tr))
            {
                while (reader.Read())
                {
                    listCommands.Add(reader["command"].ToString());
                }
            }

            foreach (var command in listCommands)
            {
                connection.Execute(command, transaction: tr, commandTimeout: this.CommandTimeOut);
            }
        }

        /// <summary>
        /// Создает партиции, очищает мастер таблицу и сравнивает, что кол-во записей в мастере = кол-ву записей в партициях
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tr"></param>
        /// <param name="tableName"></param>
        private void CreatePartitionsWithClear(IDbConnection connection, IDbTransaction tr, string tableName)
        {
            string sql;

            //создаем партиции для regop_bank_stmnt_op
            sql = $@"SELECT create_partitions_by_name('{tableName}', 'period_id');";
            connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

            //получаем кол-во строк в мастер-таблице
            sql = $@"SELECT count(1) FROM ONLY public.{tableName}";
            var countInMaster = connection.ExecuteScalar<long>(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

            //очищаем мастер таблицу
            sql = $@"TRUNCATE ONLY public.{tableName} ";
            connection.Execute(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

            //получаем кол-во строк в партициях
            sql = $@"SELECT count(1) FROM public.{tableName}";
            var countInPartitions = connection.ExecuteScalar<long>(sql, transaction: tr, commandTimeout: this.CommandTimeOut);

            if (countInPartitions != countInMaster)
            {
                throw new Exception($"Кол-во строк в мастер-таблице {tableName} не совпадает с кол-вом строк в партициях");
            }
        }

        /// <summary>
        /// Проверка необходимости выполнения действия
        /// </summary>
        public bool IsNeedAction()
        {
            var container = ApplicationContext.Current.Container;
            var history = container.ResolveDomain<ExecutionActionHistory>();
            var result = history.GetAll().Where(j => j.Code == CreatePartitionsForMoneyOperationAction.Code && j.Status == ExecutionActionStatus.Success).IsEmpty();
            return result;
        }
    }
}
