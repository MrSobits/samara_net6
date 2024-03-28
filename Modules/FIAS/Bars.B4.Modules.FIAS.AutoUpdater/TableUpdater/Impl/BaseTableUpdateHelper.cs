namespace Bars.B4.Modules.FIAS.AutoUpdater.TableUpdater.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using Bars.B4.DataAccess;

    using Npgsql;

    /// <summary>
    /// Базовая реализация сервиса обновления данных в таблице
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    internal abstract class BaseTableUpdateHelper<T> : ITableUpdateHelper<T>
        where T : PersistentObject
    {
        /// <inheritdoc />
        public virtual string TableName { get; } = TableNameHelper.GetTableName<T>();

        /// <inheritdoc />
        public virtual string TempTableName => $"tmp_{this.TableName}";

        /// <inheritdoc />
        public virtual string CreateTempTableCommand => $"CREATE TEMP TABLE {this.TempTableName} AS SELECT * FROM {this.TableName} LIMIT 0;";

        /// <inheritdoc />
        public abstract string UpdateFromTempTableCommand { get; }

        /// <inheritdoc />
        public abstract string DropFiasTypeRecordsCommand { get; }

        /// <summary>
        /// Столбцы таблицы
        /// </summary>
        protected abstract string TableColumns { get; }

        /// <summary>
        /// Команда копирования данных
        /// </summary>
        /// <param name="toTempTable">Во временную таблицу</param>
        protected virtual string GetCopyCommand(bool toTempTable = false)
        {
            return $"COPY {(toTempTable ? this.TempTableName : this.TableName)} ({this.TableColumns}) FROM STDIN (FORMAT BINARY);";
        }

        /// <summary>
        /// Преобразование записи в бинарный вид
        /// </summary>
        /// <param name="serializer">Сериализатор <see cref="NpgsqlCopySerializer"/></param>
        /// <param name="record">Данные</param>
        protected abstract void WriteRecord(NpgsqlBinaryImporter writer, T record);

        /// <inheritdoc />
        public virtual void UpdateProcess(IDbConnection connection, IEnumerable<T> records, bool toTempTable = false)
        {
            var pgConnection = connection as NpgsqlConnection;
            if (pgConnection == null)
            {
                throw new NotImplementedException("Операция копирования реализована для PostgreSQL");
            }
            
            if (toTempTable)
            {
                using (var cmd = new NpgsqlCommand(this.CreateTempTableCommand, pgConnection))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var writer = pgConnection.BeginBinaryImport(this.GetCopyCommand(toTempTable)))
                {

                    foreach (var rec in records)
                    {
                        this.WriteRecord(writer, rec);
                    }

                    writer.Complete();
                }

                using (var cmd = new NpgsqlCommand(this.UpdateFromTempTableCommand, pgConnection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                using (var cmd = new NpgsqlCommand(this.DropFiasTypeRecordsCommand, pgConnection))
                {
                    cmd.ExecuteNonQuery();
                }
                
                using (var writer = pgConnection.BeginBinaryImport(this.GetCopyCommand(toTempTable)))
                {

                    foreach (var rec in records)
                    {
                        this.WriteRecord(writer, rec);
                    }

                    writer.Complete();
                }
            }
        }
    }
}