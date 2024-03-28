namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.LoadFromOtherSystems
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using B4;
    using Dapper;
    using Entities.ImportIncrementalData.LoadFromOtherSystems;
    using Intf;
    using NHibernate.Util;
    using Npgsql;

    public class DeterminateStorageService : IDeterminateStorage
    {
        public IDbConnection CurrentConnection { get; set; }
        public FormatTemplate FormatTemplate { get; set; }
        public FileHeader FileHeader { get; set; }
        public UploadErrors UploadErrors { get; set; }

        public IDataResult DeterminateStorage()
        {
            var dataSupplierId = FormatTemplate.SupplierAndBank.DataSupplierId;
            var dataBankKey = FormatTemplate.SupplierAndBank.DataBankKey;
            var sql = string.Format("select data_storage_id from master.data_bank where data_supplier_id = {0} and data_bank_key = '{1}'", dataSupplierId, dataBankKey);
            var dataStorageId = CurrentConnection.ExecuteScalar<int>(sql);
            DataSchema dataSchema = null;
            DataStorage dataStorage = null;
            var year = 0;
            var bufferPersonalAccountCount = FormatTemplate.SectionList.FirstOrDefault(x => x.SectionName.Contains("kvar")).RowCount;
            sql = string.Format(" select ds.data_storage_id as \"Id\", ds.data_storage as \"DataStorageName\", ds.pref as \"Pref\", " +
                           "   (select sum(kvar_count) from master.data_bank where ds.data_storage_id = data_storage_id) as \"PersonalAccountCount\"," +
                           "   db.db_name as \"DatabaseName\",db.db_connect as \"DatabaseConnectionString\"," +
                           "   (select setting_value from master.sys_setting where setting_id = 1) as \"DefaultVolume\"," +
                           "   (select setting_value from master.sys_setting where setting_id = 2) as \"MaxVolume\"" +
                           " from master.data_storage ds" +
                           " join master.data_base db on ds.db_id = db.db_id and db.is_active <> 100");

            var dataStorageList = CurrentConnection.Query<DataStorage>(sql).ToList();
            //Хранилище не определилось
            if (dataStorageId == 0)
            {
                var list = dataStorageList.Where(
                    x =>
                        x.PersonalAccountCount < x.DefaultVolume &&
                        x.PersonalAccountCount + bufferPersonalAccountCount <= x.MaxVolume);
                if (!list.Any())
                {
                    dataStorageId = CreateStorage();
                    dataStorageList = CurrentConnection.Query<DataStorage>(sql).ToList();
                }
                else
                {
                    var min = dataStorageList.Min(x => x.PersonalAccountCount);
                    foreach (var storage in dataStorageList)
                    {
                        if (storage.PersonalAccountCount == min)
                        {
                            dataStorageId = storage.Id;
                            break;
                        }
                    }
                }
                //Обновление data_storage_id
                sql = string.Format("update master.data_bank set data_storage_id = {0} where data_supplier_id = {1} and data_bank_key = '{2}'", dataStorageId, dataSupplierId, dataBankKey);
                CurrentConnection.Execute(sql);
            }
            dataStorage = dataStorageList.First(x => x.Id == dataStorageId);

            sql = string.Format("select count(*) from master.data_schema where year_ is null and data_storage_id = {1}", year, dataStorageId);
            var dataSchemaId = 0;
            if (CurrentConnection.ExecuteScalar<long>(sql) == 0)
            {
                var connection = new NpgsqlConnection(dataStorage.DatabaseConnectionString);
                try
                {
                    connection.Open();
                    dataSchemaId = CreateSchema(connection, dataStorage.Pref, dataStorageId);
                }
                catch (Exception ex)
                {
                    UploadErrors.GetErrorMessage("INFO.csv", null, 999, "Ошибка, не удалось создать схему:" + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            else dataSchemaId = CurrentConnection.ExecuteScalar<int>(string.Format("select data_schema_id from master.data_schema where year_ is null and data_storage_id = {1}", year, dataStorageId));
            sql = string.Format("select data_schema_Id as \"Id\",schema_name as \"SchemaName\",year_ as \"Year\" from master.data_schema where data_schema_id = {0}", dataSchemaId);
            dataSchema = EnumerableExtensions.First(CurrentConnection.Query<DataSchema>(sql)) as DataSchema;
            dataSchema.DataStorage = dataStorage;
            return new BaseDataResult { Success = true, Data = dataSchema };
        }

        private int CreateStorage()
        {
            var sql =
                string.Format(
                    " insert into master.data_storage(data_storage_id,data_storage,pref,db_id) " +
                    " select (select coalesce(max(data_storage_id),0)+1 from master.data_storage)," +
                    " 'ds_'||(select coalesce(max(data_storage_id),0)+1 from master.data_storage)," +
                    " 'ds_'||(select coalesce(max(data_storage_id),0)+1 from master.data_storage)," +
                    " (select db_id from master.data_base where is_active <> 100) returning data_storage_id");
            return CurrentConnection.ExecuteScalar<int>(sql);
        }

        private int CreateSchema(IDbConnection connection, string schemaName, int dataStorageId)
        {
            var sql = string.Format("drop schema if exists {0} cascade;", schemaName);
            connection.Execute(sql);
            sql = string.Format("create schema {0}", schemaName);
            connection.Execute(sql);
            var tablesList = GetChdTablesList();
            foreach (var table in tablesList)
            {
                sql = string.Format("create table {0}.{1} ( LIKE chd_pattern.{1} INCLUDING ALL)", schemaName, table.TableName);
                connection.Execute(sql);
                var seqName = string.Format("{0}.{1}", schemaName, table.SequenceName);
                sql = string.Format("create sequence {0};", seqName);
                connection.Execute(sql);
                sql = string.Format("alter table {0}.{1} alter column {2} drop default;", schemaName, table.TableName, table.SequenceColumnName);
                connection.Execute(sql);
                sql = string.Format("alter table {0}.{1} alter column {2} set default nextval('{3}');", schemaName, table.TableName, table.SequenceColumnName, seqName);
                connection.Execute(sql);
            }
            sql = string.Format("insert into master.data_schema(data_storage_id,schema_name) values ({0},'{1}') returning data_schema_id", dataStorageId, schemaName);
            return CurrentConnection.ExecuteScalar<int>(sql);
        }

        public List<ChdTable> GetChdTablesList()
        {
            var sql = string.Format(" select table_id as \"Id\",table_name as \"TableName\",sequence_name as \"SequenceName\",seq_column_name as \"SequenceColumnName\" " +
                                    " from master.chd_table");
            return CurrentConnection.Query<ChdTable>(sql).ToList();
        }
    }
}
