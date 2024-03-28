namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.LoadFromOtherSystems
{
    using System;
    using System.Data;
    using Dapper;
    using Entities.ImportIncrementalData.LoadFromOtherSystems;
    using Intf;
    using System.Collections.Generic;
    using System.Linq;

    public class RegisterFormatForOtherSystems : IRegisterFormatForOtherSystems
    {
        public IDbConnection Connection { get; set; }
        public FileHeader FileHeader { get; set; }
        public LoadLog LoadLog { get; set; }
        public FormatTemplate FormatTemplate { get; set; }

        public UploadErrors UploadErrors { get; set; }

        public int AddFileToUpload()
        {
            int value;
            try
            {
                var date = new DateTime(Convert.ToInt32(FileHeader.SenderCalculationYear), Convert.ToInt32(FileHeader.SenderCalculationMonth), 1).ToShortDateString();
                var sql = string.Format(@"insert into upload.upload_file (mgf_file_id,file_name,file_path,file_status_id,progress,started_on,changed_by,changed_on,report_month) 
                                    values ({2},'{0}','',{1},0,now(),1,now(),'{3}') 
                                    returning file_id; ",
                                    FormatTemplate.FileName, (int)UploadStatuses.Upload, FormatTemplate.SupplierAndBank.MgfFileId, date);
                value = Connection.ExecuteScalar<int>(sql);
            }
            catch (Exception)
            {
                UploadErrors.GetErrorMessage("_info.csv", null, 999, "Ошибка добавления сведений о загрузке в таблицу upload.upload_file ");
                throw;
            }
            return value;
        }

        public void UpdateFileStatus(UploadStatuses statuses, int dataStatusId)
        {
            var sql = string.Format("update upload.upload_file set progress = 1,file_status_id = {0},data_status_id = {2},finished_on = now() where file_id = {1}", (int)statuses, FormatTemplate.UploadId, dataStatusId);
            Connection.Execute(sql);
        }

        public void UpdateBankAndSupplier()
        {
            var sql = string.Format("update upload.upload_file set data_supplier_id = {0},data_bank_id = {1}  where file_id = {2}", FormatTemplate.SupplierAndBank.DataSupplierId, FormatTemplate.SupplierAndBank.DataBankId, FormatTemplate.UploadId);
            Connection.Execute(sql);
        }

        public void UpdateFileProgress(decimal progress)
        {
            var sql = string.Format("update upload.upload_file set progress = {0} where file_id = {1}", progress, FormatTemplate.UploadId);
            Connection.Execute(sql);
        }

        public List<TemplateHeader> GetFormatHeaders()
        {
            List<TemplateHeader> list;
            try
            {
                list = Connection.
                Query<TemplateHeader>(string.Format("select section as \"SectionName\",table_name as \"SectionTable\"," +
                                               "file_name as \"SectionFile\",format_section_id as \"SectionNumber\",ordering as \"Ordering\" " +
                                              " from master.format f" +
                                              " join master.format_section fs on fs.format_id = f.format_id " +
                                              " where f.version = '{0}'", FileHeader.FormatVersion.Trim())).ToList();
            }
            catch (Exception)
            {
                UploadErrors.GetErrorMessage("_info.csv", null, 999, "Ошибка получения заголков секций, проверьте схему master");
                throw;
            }
            return list;
        }

        public void SetOrdering()
        {
            FormatTemplate.SectionList.ForEach(x =>
            {
                var template = FormatTemplate.HeaderList.FirstOrDefault(y => (y.SectionFile + ".csv").ToUpper() == x.SectionName.ToUpper());
                x.Ordering = template == null ? 1000 : template.Ordering;
            });
        }

        public Dictionary<string, List<Template>> GetFormat()
        {
            var sections = FormatTemplate.HeaderList;
            var dict = new Dictionary<string, List<Template>>();
            List<TemplateHeader> list = null;
            try
            {
                foreach (var section in sections)
                {
                    var sql =
                        string.Format(
                            " select column_name as \"ColumnName\",'{1}' as \"SectionName\",field_name as \"FieldName\"," +
                            " min_value as \"MinLength\", (case when data_type_id = 1 then char_length else max_value end) as \"MaxLength\", " +
                            " place as \"Place\",is_required::boolean as \"Necessarily\",data_type_id as \"Type\",numeric_precision as \"NumericPrecission\"," +
                            " numeric_scale as \"NumericScale\",is_unique::boolean as \"IsUnique\",is_ref as \"IsRef\",ref_schema as \"RefSchema\"," +
                            " ref_table as \"RefTable\",ref_column as \"RefColumn\"" +
                            " from master.format_field " +
                            " where format_section_id = {0} order by place", section.SectionNumber, section.SectionName);
                    dict.Add(section.SectionTable, Connection.Query<Template>(sql).ToList());
                }
            }
            catch (Exception)
            {
                UploadErrors.GetErrorMessage("_info.csv", null, 999, "Ошибка получения заголков секций, проверьте схему master");
                throw;
            }
            return dict;
        }

        public bool GetLoadResult()
        {
            bool result;
            try
            {
                var sql = string.Format(" select count(1) from upload.file_log where file_id = {0} and error_id < 3000 and error_id <> 1000 ", FormatTemplate.UploadId);
                var fileResult = Connection.ExecuteScalar<long>(sql) == 0;
                sql = string.Format(" select count(1) from upload.system_file_log where file_id = {0}", FormatTemplate.UploadId);
                var fileSystemResult = Connection.ExecuteScalar<long>(sql) == 0;
                result = fileResult && fileSystemResult;
            }
            catch
            {
                return false;
            }
            return result;
        }
    }
}
