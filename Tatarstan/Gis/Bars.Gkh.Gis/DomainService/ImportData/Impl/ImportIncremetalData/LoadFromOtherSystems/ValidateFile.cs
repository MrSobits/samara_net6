namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.LoadFromOtherSystems
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4;
    using Castle.Windsor;
    using Dapper;
    using Entities.ImportIncrementalData.LoadFromOtherSystems;
    using Intf;
    using Ionic.Zip;

    public class ValidateFile : IValidateFile
    {
        public IWindsorContainer Container { get; set; }
        public LoadLog Log { get; set; }
        public FileHeader FileHeader { get; set; }
        public FormatTemplate FormatTemplate { get; set; }
        public IncrementalParams IncrementalParams { get; set; }
        public IDbConnection Connection { get; set; }
        public UploadErrors UploadErrors { get; set; }
        public IRegisterFormatForOtherSystems RegisterFormatForOtherSystems { get; set; }
        public IDataResult ValidateHeader(ZipFile mainArchive, string fileName, bool checkDataSupplier)
        {
            //Считать заголовок файла
            var verificationInfo =
                mainArchive.FirstOrDefault(x => x.FileName.ToLower().Contains("_info.csv"));

            if (verificationInfo == null)
            {
                UploadErrors.GetErrorMessage("_info.csv", null, 1500, "Архив не прошел валидацию! Не найден заголовочный файл!");
                return new BaseDataResult(false, "Архив не прошел валидацию! Не найден заголовочный файл!");
            }

            using (var ms = new MemoryStream())
            {
                verificationInfo.Extract(ms);
                ms.Seek(0, SeekOrigin.Begin);

                var streamReader = new StreamReader(ms, Encoding.GetEncoding(1251));
                //пропускаем заколовки
                streamReader.ReadLine();

                //считываем инфомрацию об отправителе (для дальнейшей сверки)
                var senderInfo = streamReader.ReadLine().Split(';');

                //Версия формата
                FileHeader.FormatVersion = senderInfo[0];
                //ИНН
                FileHeader.SenderInn = senderInfo[1];
                //КПП
                FileHeader.SenderKpp = senderInfo[2];
                //ОГРН (ОГРНИП)
                FileHeader.SenderOgrn = senderInfo[3];
                //Наименование организации (ФИО ИП)
                FileHeader.SenderOrgName = senderInfo[4];
                //Ключ банка данных (Наименование банка данных)
                FileHeader.SenderDataBankKey = senderInfo[5];
                //Год, за который предоставлены сведения
                FileHeader.SenderCalculationYear = senderInfo[6];
                //Месяц, за который предоставлены сведения
                FileHeader.SenderCalculationMonth = senderInfo[7];
                //Дата и время формирования файла
                FileHeader.SenderFileDateTime = senderInfo[8];
                //ФИО отправителя
                FileHeader.SenderFullName = senderInfo[9];
                //Телефон отправителя
                FileHeader.SenderPhone = senderInfo[10];
            }
            //Добавить запись о загрузке]
            FormatTemplate.FileName = fileName;
            FormatTemplate.UploadId = Container.Resolve<IRegisterFormatForOtherSystems>().AddFileToUpload();

            FormatTemplate.SectionList = LoadSections(mainArchive);

            if (checkDataSupplier)
            {
                //Обновление данных поставщика
                foreach (var supplier in IncrementalParams.Suppliers)
                {
                    UpdateSupplier(supplier);

                    UpdateDataBanks(supplier.DataBankList, supplier.DataSupplierId);
                }
                var suppliersIds = string.Join(",", IncrementalParams.Suppliers.Select(x => x.DataSupplierId));
                //Проверить существование поставщика и банка данных
                var check = CheckSupplierAndBank(suppliersIds);
                if (!check.Success) return check;
                FormatTemplate.SupplierAndBank = (SupplierAndBank)check.Data;
                if (FormatTemplate.SupplierAndBank == null)
                {
                    return new BaseDataResult(false, "Ошибка при получении банка данных и поставщика");
                }
                var count = FormatTemplate.SectionList.FirstOrDefault(x => x.SectionName.Contains("kvar")).RowCount;
                var sql = string.Format("update master.data_bank set kvar_count = {2} where data_supplier_id = {0} and data_bank_key = '{1}'", FormatTemplate.SupplierAndBank.DataSupplierId, FormatTemplate.SupplierAndBank.DataBankKey, count);
                Connection.Execute(sql);
                Container.Resolve<IRegisterFormatForOtherSystems>().UpdateBankAndSupplier();
            }
            else FormatTemplate.SupplierAndBank = new SupplierAndBank();

            return new BaseDataResult();
        }

        private List<Section> LoadSections(IEnumerable<ZipEntry> mainArchive)
        {
            var verificationInfo =
                mainArchive.FirstOrDefault(x => x.FileName.ToLower().Contains("_filelist.csv"));
            var list = new List<Section>();
            if (verificationInfo == null)
            {
                UploadErrors.GetErrorMessage("_info.csv", null, 999, "Архив не прошел валидацию! Не найден заголовочный файл!");
                throw new Exception("Архив не прошел валидацию! Не найден заголовочный файл!");
            }
            using (var ms = new MemoryStream())
            {
                verificationInfo.Extract(ms);
                ms.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(ms, Encoding.GetEncoding(1251)))
                {
                    streamReader.ReadLine();
                    while (!streamReader.EndOfStream)
                    {
                        var readLine = streamReader.ReadLine();
                        if (readLine != null)
                        {
                            var split = readLine.Split(';');
                            list.Add(new Section { SectionName = split[0], RowCount = Convert.ToInt32(split[1]) });
                        }
                    }
                }
            }
            return list;
        }

        private IDataResult CheckSupplierAndBank(string suppliers)
        {
            var sql = string.Format("select count(*) from master.data_supplier where upper(ogrn) = upper(trim('{0}')) and data_supplier_id in ({1})", FileHeader.SenderOgrn, suppliers);
            var countOgrn = Connection.ExecuteScalar<long>(sql);
            sql = string.Format("select count(*) from master.data_supplier where upper(inn) = upper(trim('{0}')) and upper(kpp) = upper(trim('{1}')) and data_supplier_id in ({2})",
               FileHeader.SenderInn, FileHeader.SenderKpp, suppliers);
            var countInnKpp = Connection.ExecuteScalar<long>(sql);
            if (countOgrn == 0)
            {
                UploadErrors.GetErrorMessage("_INFO.csv", null, 1000, "");
                if (countInnKpp == 0)
                {
                    UploadErrors.GetErrorMessage("_INFO.csv", null, 1001, "");
                    return new BaseDataResult
                    {
                        Message = string.Format("Ошибка {0}! {1}", 1001, UploadErrors.Types[1001]),
                        Success = false
                    };
                }
            }
            sql = string.Format("select count(*) from master.data_bank b,master.data_supplier s " +
                                "where upper(trim(b.data_bank_key)) = upper(trim('{0}')) and b.is_actual <> 100 and s.data_supplier_id = b.data_supplier_id and s.data_supplier_id in ({1})",
                                FileHeader.SenderDataBankKey, suppliers);
            var countDataBank = Connection.ExecuteScalar<long>(sql);
            if (countDataBank == 0)
            {
                UploadErrors.GetErrorMessage("_info.csv", null, 1002, "Ключ банка данных " + FileHeader.SenderDataBankKey + " не зарегистрирован в МЖФ");
                return new BaseDataResult { Message = string.Format("Ошибка {0}!Ключ банка данных {1} не зарегистрирован в МЖФ", 1002, FileHeader.SenderDataBankKey), Success = false };
            }
            sql = string.Format(@"select data_bank_id as ""DataBankId"",b.data_supplier_id as ""DataSupplierId"",data_bank_key as ""DataBankKey"",
                    data_storage_id as ""DataStorageId"",kvar_count as ""PersonalAccountsCount"",data_supplier as ""DataSupplier"",
                    inn as ""Inn"",kpp as ""Kpp"",ogrn as ""Ogrn""
                    from master.data_bank b  
                    join master.data_supplier s on s.data_supplier_id = b.data_supplier_id and 
                        (upper(inn) = upper(trim('{1}')) and upper(kpp) = upper(trim('{2}')) or (upper(ogrn) = upper(trim('{3}'))))
                    where trim(upper(b.data_bank_key)) = upper(trim('{0}')) and s.data_supplier_id in ({4})"
                , FileHeader.SenderDataBankKey, FileHeader.SenderInn, FileHeader.SenderKpp, FileHeader.SenderOgrn, suppliers);
            return new BaseDataResult { Data = Connection.Query<SupplierAndBank>(sql).FirstOrDefault() };
        }

        private void UpdateSupplier(Supplier supplier)
        {
            var sql = string.Format("select count(*) from master.data_supplier where data_supplier_id = {0}", supplier.DataSupplierId);
            if (Connection.ExecuteScalar<long>(sql) == 0)
            {
                sql = string.Format("insert into master.data_supplier(data_supplier_id,data_supplier,inn,kpp,ogrn) values ({0},'{1}','{2}','{3}','{4}')",
                    supplier.DataSupplierId, supplier.DataSupplier, supplier.Inn, supplier.Kpp, supplier.Ogrn);
            }
            else
            {
                sql =
                  string.Format(
                      " update master.data_supplier set inn = '{0}',kpp = '{1}',ogrn = '{2}' where data_supplier_id = {3}"
                      , supplier.Inn, supplier.Kpp, supplier.Ogrn, supplier.DataSupplierId);
            }
            Connection.Execute(sql);
        }

        private void UpdateDataBanks(IEnumerable<DataBank> dataBankKeys, long dataSupplierId)
        {
            var sql = string.Format("update master.data_bank set is_actual = 100 where data_supplier_id = {0}", dataSupplierId);
            Connection.Execute(sql);
            foreach (var key in dataBankKeys)
            {
                sql = string.Format("select count(*) from master.data_bank where data_supplier_id = {0} and data_bank_key = '{1}'", dataSupplierId, key.DataBankKey);
                if (Connection.ExecuteScalar<long>(sql) == 0)
                {
                    sql = string.Format(
                        " insert into master.data_bank(data_supplier_id,data_bank_key,data_bank_name,data_storage_id,kvar_count,is_actual,rajon_list) " +
                        " values ({0},'{1}','{2}',null,{3},1,'{4}')"
                        , dataSupplierId, key.DataBankKey, key.DataBankName, 0, key.RajonList);
                }
                else
                {
                    sql = string.Format("update master.data_bank set is_actual = 1 where data_supplier_id = {0} and data_bank_key = '{1}'", dataSupplierId, key.DataBankKey);
                }
                Connection.Execute(sql);
            }
        }

        public IDataResult Validate()
        {
            //Проверка уникальности значений
            //Справочник услуг
            string sql;
            var headers = FormatTemplate.HeaderList;
            var fields = FormatTemplate.Format;
            var uploadId = FormatTemplate.UploadId;
            var listJoinFields = "";
            //Общие проверки
            foreach (var section in headers)
            {
                try
                {
                    List<string> listValues;
                    var listFields = fields[section.SectionTable].Where(x => x.Necessarily);
                    listJoinFields = GetAllSectionField(section.SectionTable);

                    //Проверка на заполненность обязательных полей
                    foreach (var field in listFields)
                    {
                        sql =
                            string.Format(
                                "select ' строка из файла - '||replace(" + listJoinFields +
                                ",'\"\"','') from upload.{1} where file_id = {2} and ({0} is null or {0}::varchar = '')",
                                field.ColumnName, section.SectionTable, uploadId);
                        listValues = Connection.Query<string>(sql).ToList();
                        foreach (var value in listValues)
                        {
                            UploadErrors.GetErrorMessage(section.SectionFile, null, 2002,
                                string.Format("Секция {0},поле: {1}, {2}", section.SectionName, field.FieldName, value));
                        }
                    }
                    //Одно на секцию
                    var uniqueField = fields[section.SectionTable].FirstOrDefault(x => x.IsUnique);
                    if (uniqueField != null)
                    {
                        //Проверка на уникальность
                        sql =
                            string.Format(
                                "select {0} from upload.{1} where file_id = {2} group by 1 having (count({0})>1)",
                                uniqueField.ColumnName, section.SectionTable, uploadId);
                        listValues = Connection.Query<string>(sql).ToList();
                        foreach (var value in listValues)
                        {
                            sql =
                                string.Format(
                                    "select replace(" + listJoinFields +
                                    ",'\"\"','') from upload.{0} where file_id = {1} and {2}::varchar = {3}::varchar",
                                    section.SectionTable, uploadId, uniqueField.ColumnName, value);
                            var printValue = Connection.ExecuteScalar<string>(sql);
                            UploadErrors.GetErrorMessage(section.SectionFile, null, 2500,
                                string.Format("Секция {0},поле: {1}, строка из файла - {2}", section.SectionName,
                                    uniqueField.FieldName, printValue));
                        }

                    }
                    //ссылки
                    listFields = fields[section.SectionTable].Where(x => x.IsRef);
                    foreach (var field in listFields)
                    {
                        var table = FormatTemplate.HeaderList.FirstOrDefault(x => x.SectionTable == field.RefTable);
                        //sql = string.Format("select count(1) from upload.{0} where file_id = {1} and {2} <> ''", section.SectionTable, uploadId, field.ColumnName);
                        sql = string.Format("select count(1) from upload.{0} where file_id = {1} and {2} is not null ",
                            section.SectionTable, uploadId, field.ColumnName);
                        if (table != null && !table.IsUpload)
                        {
                            if (Connection.ExecuteScalar<long>(sql) > 0)
                                UploadErrors.GetErrorMessage(section.SectionFile, null, 2503,
                                    string.Format("Секция {0},поле: {1}, не загружена секция - {2}", section.SectionName,
                                        field.FieldName, table.SectionName));
                            continue;
                        }
                        //sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{1} where file_id = {2} and {0} <> '' and not exists (select {3} from {4}.{5} where file_id = {2} and upload.{1}.{0} = {3})", field.ColumnName, section.SectionTable, uploadId, field.RefColumn, field.RefSchema, field.RefTable);
                        sql =
                            string.Format(
                                "select 'значение - '||{0}||', строка из файла - '||replace(" + listJoinFields +
                                ",'\"\"','') from upload.{1} a where a.file_id = {2} and a.{0} is not null and not exists (select 1 from {4}.{5} b where b.file_id = {2} and a.{0} = b.{3})",
                                field.ColumnName, section.SectionTable, uploadId, field.RefColumn, field.RefSchema,
                                field.RefTable);
                        listValues = Connection.Query<string>(sql).ToList();
                        foreach (var value in listValues)
                        {
                            UploadErrors.GetErrorMessage(section.SectionFile, null, 2502,
                                string.Format("Секция {0},поле: {1}, {2}", section.SectionName, field.FieldName, value));
                        }
                    }
                }
                catch (Exception ex)
                {
                    UploadErrors.GetErrorMessage("_info.csv", null, 999, string.Format("Ошибка:{0}", ex.Message));
                }
            }
            {
                //Проверки по конкретным таблицам
                //Общая информация	КПП	Длина ИНН < 12	Поле заполнено
                var section = headers.Find(x => x.SectionNumber == 1);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format("select 'значение - '||inn||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and kpp is null and char_length(inn)<12 ", section.SectionTable, uploadId);
                var listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле: ИНН - Длина ИНН < 12, {1}", section.SectionName, value));
                }

                //Справочные строка из файла параметров	Уникальный код параметра + Код значения		Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 7);
                sql = string.Format(" select max('Уникальный код параметра - '||coalesce(prm_id::varchar,'')||', Код значения - '||coalesce(prm_dict_id::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by prm_dict_id,prm_id having (count(prm_dict_id||prm_id)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2500, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Контрагента	КПП	Длина ИНН < 12	Поле заполнено	2002
                section = headers.Find(x => x.SectionNumber == 12);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format("select 'значение - '||inn||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and kpp is null and char_length(inn)<12 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле: ИНН - Длина ИНН < 12, {1}", section.SectionName, value));
                }

                //Лицевые счета	Номер квартиры	Признак объединенного ЛС = 0(is_union_ls = 0)	Поле заполнено
                section = headers.Find(x => x.SectionNumber == 16);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and is_union_ls = 0 and trim(coalesce(nkvar, '')) = '' ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Номер квартиры, строка из файла - {1}", section.SectionName, value));
                }

                //Дополнительные параметры домов	Код дома + Код параметра + Дата начала действия		Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 15);
                sql = string.Format(" select max('Код дома - '||coalesce(dom_id::varchar,'')||'Код параметра - '||coalesce(prm_id::varchar,'')||'Дата начала действия - '||coalesce(act_from::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by dom_id,prm_id,act_from having (count(dom_id||prm_id||act_from)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Дополнительные параметры лицевых счетов	Код ЛС + Код параметра + Дата начала действия		Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 17);
                sql = string.Format(" select max('Код ЛС - '||coalesce(ls_id::varchar,'')||'Код параметра - '||coalesce(prm_id::varchar,'')||'Дата начала действия - '||coalesce(act_from::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by ls_id,prm_id,act_from having (count(ls_id||prm_id||act_from)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Дополнительные параметры договоров на оказание ЖКУ Код договора + Код параметра + Дата начала действия		Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 19);
                sql = string.Format(" select max('Код договора - '||coalesce(contract_id::varchar,'')||', Код параметра - '||coalesce(prm_id::varchar,'')||', Дата начала действия - '||coalesce(act_from::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by contract_id,prm_id,act_from having (count(contract_id||prm_id||act_from)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Начисления и сальдо лицевых счетов	Код ЛС + Код услуги + Код договора 		Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 20);
                sql = string.Format(" select max('Код ЛС - '||coalesce(ls_id::varchar,'')||', Код услуги - '||coalesce(serv_id::varchar,'')||', Код договора  - '||coalesce(contract_id::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by ls_id,serv_id,contract_id having (count(ls_id||serv_id||contract_id)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Изменения сальдо	Код ЛС + Код услуги + Код договора + Код перекидки		Проверка на уникальность
                //section = headers.Find(x => x.SectionNumber == 21);
                //sql = string.Format(" select max(replace(" + listJoinFields + ",'\"\"','')) from upload.{0} where file_id = {1} group by ls_id,serv_id,contract_id,saldo_change_type having (count(ls_id||serv_id||contract_id||saldo_change_type)>1) ", section.SectionTable, uploadId);
                //listValues = Connection.Query<string>(sql).ToList();
                //foreach (var value in listValues)
                //{
                //    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: Код ЛС + Код услуги + Код договора + Код перекидки , строка из файла - {1}", section.SectionName, value));
                //}

                //Изменения сальдо Тариф	Код типа перекидки > 100	Поле заполнено
                section = headers.Find(x => x.SectionNumber == 21);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and tarif is null and saldo_change_type > 100 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Тариф, при типе перекидки > 100, строка из файла - {1}", section.SectionName, value));
                }

                //Изменения сальдо Расход	Код типа перекидки > 100	Поле заполнено
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and rashod is null and saldo_change_type > 100 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Расход, при типе перекидки > 100, строка из файла - {1}", section.SectionName, value));
                }

                //Недопоставки услуг	Код ЛС + Код услуги + Тип недопоставки + Дата начала		Проверка на уникальность
                //section = headers.Find(x => x.SectionNumber == 22);
                //sql = string.Format(" select max('Код ЛС - '||coalesce(ls_id::varchar,'')||', Код услуги - '||coalesce(serv_id::varchar,'')||', Тип недопоставки - '||coalesce(nedo_dict_id::varchar,'')||', Дата начала - '||coalesce(nedo_from::varchar,''))" +
                //                    " from upload.{0} where file_id = {1} group by ls_id,serv_id,nedo_dict_id,nedo_from having (count(ls_id||serv_id||nedo_dict_id||nedo_from)>1) ", section.SectionTable, uploadId);
                //listValues = Connection.Query<string>(sql).ToList();
                //foreach (var value in listValues)
                //{
                //    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                //}

                //Параметры лицевых счетов в месяце перерасчета	Месяц перерасчета + Код ЛС Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 23);
                sql = string.Format(" select max('Месяц перерасчета - '||coalesce(recalc_month::varchar,'')||', Код ЛС - '||coalesce(ls_id::varchar,''))" +
                                    " from upload.{0} where file_id = {1} group by recalc_month,ls_id having (count(recalc_month||ls_id)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Дополнительные параметры лицевых счетов в месяце перерасчета	Месяц перерасчета + Код ЛС + Код параметра + Дата начала действия		Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 24);
                sql = string.Format(" select max('Месяц перерасчета - '||coalesce(recalc_month::varchar,'')||', Код ЛС - '||coalesce(ls_id::varchar,'')||', Код параметра - '||coalesce(prm_id::varchar,'')||', Дата начала действия - '||coalesce(act_from::varchar,''))" +
                                    " from upload.{0} where file_id = {1} group by recalc_month,ls_id,prm_id,act_from having (count(recalc_month||ls_id||prm_id||act_from)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Перерасчеты начислений лицевых счетов	Месяц перерасчета + Код ЛС + Код договора + Код услуги		Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 25);
                sql = string.Format(" select max('Месяц перерасчета - '||coalesce(recalc_month::varchar,'')||', Код ЛС - '||coalesce(ls_id::varchar,'')||', Код договора - '||coalesce(contract_id::varchar,'')||', Код услуги - '||coalesce(serv_id::varchar,''))" +
                                    " from upload.{0} where file_id = {1} group by recalc_month,ls_id,contract_id,serv_id having (count(recalc_month||ls_id||contract_id||serv_id)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Приборы учета	Код дома	Вид прибора учета = Общедомовой	Поле заполнено
                section = headers.Find(x => x.SectionNumber == 26);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and dom_id is null and pu_type_id = 1 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Код дома, при Вид прибора учета = Общедомовой, строка из файла - {1}", section.SectionName, value));
                }

                //Приборы учета	Код дома	Вид прибора учета = Индивидуальный	Поле заполнено
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and ls_id is null and pu_type_id = 3 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Код лс, при Вид прибора учета = Индивидуальный, строка из файла - {1}", section.SectionName, value));
                }

                //Лицевые счета групповых/общеквартирных приборов учета	Код прибора + Код ЛС Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 27);
                sql = string.Format(" select max('Код прибора - '||coalesce(pu_id::varchar,'')||', Код ЛС - '||coalesce(ls_id::varchar,''))" +
                                    " from upload.{0} where file_id = {1} group by pu_id,ls_id having (count(pu_id||ls_id)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Дополнительные параметры приборов учета	Код ПУ + Код параметра + Дата начала действия		Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 28);
                sql = string.Format(" select max('Код прибора - '||coalesce(pu_id::varchar,'')||', Код параметра - '||coalesce(prm_id::varchar,'')||', Дата начала действия - '||coalesce(act_from::varchar,''))" +
                                    " from upload.{0} where file_id = {1} group by pu_id,prm_id,act_from having (count(pu_id||prm_id||act_from)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Показания ПУ	Код прибора + Дата показания		Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 29);
                sql = string.Format(" select max('Код прибора - '||coalesce(pu_id::varchar,'')||', Дата показания - '||coalesce(val_date::varchar,''))" +
                                    " from upload.{0} where file_id = {1} group by pu_id,val_date having (count(pu_id||val_date)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Информация по проживающим	Страна куда убыл 	Тип адресного листка = убытие	Поле заполнено
                section = headers.Find(x => x.SectionNumber == 30);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and leave_country is null and kart_type_id = 2 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Страна куда убыл, при Тип адресного листка = убытие, строка из файла - {1}", section.SectionName, value));
                }

                //Информация по проживающим	Регион куда убыл 	Тип адресного листка = убытие	Поле заполнено
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and leave_region is null and kart_type_id = 2 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Регион куда убыл, при Тип адресного листка = убытие, строка из файла - {1}", section.SectionName, value));
                }

                //Информация по проживающим	Район куда убыл	Тип адресного листка = убытие	Поле заполнено
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and leave_district is null and kart_type_id = 2 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Район куда убыл, при Тип адресного листка = убытие, строка из файла - {1}", section.SectionName, value));
                }

                //Информация по проживающим	Город куда убыл	Тип адресного листка = убытие	Поле заполнено
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and leave_town is null and kart_type_id = 2 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Город куда убыл, при Тип адресного листка = убытие, строка из файла - {1}", section.SectionName, value));
                }

                //Информация по проживающим	Нас. Пункт, куда убыл	Тип адресного листка = убытие	Поле заполнено
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and leave_locality is null and kart_type_id = 2 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Нас. Пункт, куда убыл, при Тип адресного листка = убытие, строка из файла - {1}", section.SectionName, value));
                }

                //Информация по проживающим	Улица, дом, корпус, квартира куда убыл	Тип адресного листка = убытие	Поле заполнено
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and leave_place is null and kart_type_id = 2 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Улица, дом, корпус, квартира куда убыл, при Тип адресного листка = убытие, строка из файла - {1}", section.SectionName, value));
                }

                //Информация по проживающим	Дата окончания регистрации по месту пребывания	Тип регистрации = по месту пребывания	Поле заполнено
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and registered_off is null and register_type = 'В' ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Дата окончания регистрации по месту пребывания, при Тип регистрации = по месту пребывания, строка из файла - {1}", section.SectionName, value));
                }

                //Информация по проживающим	Код цели прибытия	Тип адресного листка = прибытиея	Поле заполнено
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and arrive_purpose_id is null and kart_type_id = 1 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Код цели прибытия, при Тип адресного листка = прибытия, строка из файла - {1}", section.SectionName, value));
                }

                //Информация по проживающим	Код цели убытия	Тип адресного листка = убытие	Поле заполнено
                sql = string.Format("select replace(" + listJoinFields + ",'\"\"','') from upload.{0} where file_id = {1} and leave_purpose_id is null and kart_type_id = 2 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2002, string.Format("Секция {0},поле не заполнено: Код цели убытия, при Тип адресного листка = убытие, строка из файла - {1}", section.SectionName, value));
                }

                //Реестр временно выбывших	Код ЛС + код жильца + Дата начала выбытия		Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 31);
                sql = string.Format(" select max('Код ЛС - '||coalesce(ls_id::varchar,'')||', код жильца - '||coalesce(gil_id::varchar,'')||', Дата начала выбытия - '||coalesce(leave_from::varchar,''))" +
                                    " from upload.{0} where file_id = {1} group by ls_id,gil_id,leave_from having (count(ls_id||gil_id||leave_from)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Расщепление оплат Код оплаты + Код услуги + Код договора		Проверка на уникальность

                section = headers.Find(x => x.SectionNumber == 34);
                sql = string.Format(" select max('Код оплаты - '||coalesce(pay_id::varchar,'')||', Код услуги - '||coalesce(serv_id::varchar,'')||', Код договора - '||coalesce(contract_id::varchar,''))" +
                                    " from upload.{0} where file_id = {1} group by pay_id,serv_id,contract_id having (count(pay_id||serv_id||contract_id)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetErrorMessage(section.SectionFile, null, 2501, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Предупреждения
                //Общая информация	ИНН	ИНН состоит из 10 или 12 цифр
                section = headers.Find(x => x.SectionNumber == 1);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'значение - '||coalesce(inn::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and (char_length(inn)>12 or char_length(inn)<10) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0},некорректное значение: ИНН должен состоять из 10 или 12 цифр = Индивидуальный, {1}", section.SectionName, value));
                }

                //Общая информация	КПП	КПП состоит из 9 цифр
                sql = string.Format(" select 'значение - '||coalesce(kpp::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and char_length(kpp)<>9 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0},некорректное значение: КПП не состоит из 9 цифр, строка из файла - {1}", section.SectionName, value));
                }

                //Общая информация	ОГРН	ОГРН состоит из 13,15 цифр
                sql = string.Format(" select 'значение - '||coalesce(ogrn::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and (char_length(ogrn)<>15 and char_length(ogrn)<>13) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0} ,некорректное значение: ОГРН не состоит из 13 или 15 цифр, строка из файла - {1}", section.SectionName, value));
                }

                //Справочник услуг	Наименование	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 3);
                sql = string.Format(" select max('Наименование - '||coalesce(service::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by service having (count(service)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Справочник услуг	Краткое наименование	Проверка на уникальность
                sql = string.Format(" select max('Краткое наименование - '||coalesce(service_short::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by service_short having (count(service_short)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Справочник единиц измерений	Наименование	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 4);
                sql = string.Format(" select max('Наименование - '||coalesce(measure::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by measure having (count(measure)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Справочник единиц измерений	Краткое наименование	Проверка на уникальность
                sql = string.Format(" select max('Краткое наименование - '||coalesce(measure_short::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by measure_short having (count(measure_short)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Справочник единиц измерений	Код ОКЕИ	Код ОКЕИ состоит из 3 цифр
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'значение - '||coalesce(okei::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and char_length(okei)<>3 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0} ,некорректное значение: Код ОКЕИ не состоит из 3 цифр, строка из файла - {1}", section.SectionName, value));
                }

                //Справочник методик расчета	Наименование	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 5);
                sql = string.Format(" select max('Наименование - '||coalesce(frm_name::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by frm_name having (count(frm_name)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Справочник методик расчета	Краткое наименование	Проверка на уникальность
                sql = string.Format(" select max('Краткое наименование - '||coalesce(frm_short::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by frm_short having (count(frm_short)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Справочник параметров	Наименование "Принадлежность к уровню"	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 6);
                sql = string.Format(" select max('Наименование - '||coalesce(prm_name::varchar,'')||', Принадлежность к уровню - '||coalesce(prm_level_id::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by prm_name,prm_level_id having (count(prm_name||prm_level_id)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Справочные значения параметров	Уникальный код параметра + Наименование значения	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 7);
                sql = string.Format(" select max('Уникальный код параметра - '||coalesce(prm_id::varchar,'')||', Наименование значения - '||coalesce(prm_dict_val::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by prm_id,prm_dict_val having (count(prm_id||prm_dict_val)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Справочник типов недопоставок	Наименование	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 11);
                sql = string.Format(" select max('Наименование - '||coalesce(nedo_name::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by nedo_name having (count(nedo_name)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Контрагенты	ИНН + КПП	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 12);
                sql = string.Format(" select max('ИНН - '||coalesce(inn::varchar,'')||', КПП - '||coalesce(kpp::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by inn,kpp having (count(inn||kpp)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Контрагенты	ОГРН	Проверка на уникальность
                sql = string.Format(" select max('ОГРН - '||coalesce(ogrn::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by ogrn having (count(ogrn)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Контрагенты	Полное + краткое наименование	Проверка на уникальность
                sql = string.Format(" select max('Полное наименование - '||coalesce(full_name::varchar,'')||', краткое наименование - '||coalesce(short_name::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by bik having (count(bik)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Контрагенты	ИНН	ИНН состоит из 10 или 12 цифр
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'значение - '||coalesce(inn,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and (char_length(inn)>12 or char_length(inn)<10) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0}, некорректное значение: ИНН должен состоять из 10 или 12 цифр, {1}", section.SectionName, value));
                }

                //Контрагенты	КПП КПП состоит из 9 цифр
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'значение - '||coalesce(kpp,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and char_length(kpp)<>9 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0}, некорректное значение: КПП должен состоять из 9 цифр, {1}", section.SectionName, value));
                }

                //Контрагенты ОГРН	ОГРН состоит из 15 цифр
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'значение - '||coalesce(ogrn,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and char_length(ogrn)<>15 and char_length(ogrn)<>13", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0}, некорректное значение: ОГРН должен состоять из 13 или 15 цифр, {1}", section.SectionName, value));
                }

                //Контрагенты БИК	БИК состоит из 9 цифр
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'значение - '||coalesce(bik,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and char_length(bik)<>9 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0}, некорректное значение: БИК должен состоять из 9 цифр, {1}", section.SectionName, value));
                }

                //Контрагенты Кор. счет	Кор.счет состоит из 20 цифр
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'значение - '||coalesce(corr_account,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and char_length(corr_account)<>20 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0}, некорректное значение: Кор.счет должен состоять из 20 цифр, {1}", section.SectionName, value));
                }

                //Расчетные счета	Код банка + Код контрагента + Расчетный счет	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 13);
                sql = string.Format(" select max('Код банка - '||coalesce(bank_id::varchar,'')||', Код контрагента - '||coalesce(contractor_id::varchar,'')||', Расчетный счет - '||coalesce(bank_account::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by bank_id,contractor_id,bank_account having (count(bank_id||contractor_id||bank_account)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Расчетные счета Расчетный счет	Расчетный счет состоит из 20 цифр
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'значение - '||coalesce(bank_account,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and char_length(bank_account)<>20 ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0}, некорректное значение: Расчетный счет должен состоять из 20 цифр, {1}", section.SectionName, value));
                }

                //Дома	Город/район + Населенный пункт + Улица + Номер дома + Корпус + Литера	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 14);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select max('Город/район - '||coalesce(town::varchar,'')||', Населенный пункт - '||coalesce(rajon::varchar,'')||', Улица - '||coalesce(ulica::varchar,'')||', Номер дома - '||coalesce(ndom::varchar,'')||', Корпус - '||coalesce(nkor::varchar,'')||', Литера - '||coalesce(letter::varchar,'')) " +
                                    " from upload.{0} where file_id = {1} group by town,rajon,ulica,ndom,nkor,letter having (count(town||rajon||ulica||ndom||nkor||letter)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Дома	Количество строк - лицевой счет	Количество ЛС = количество строк из «Лицевые счета»
                sql = string.Format(" select 'Уникальный код дома в системе отправителя - '||coalesce(dom_id,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','')" +
                                    " from upload.{0} where file_id = {1} and ls_count <> (select count(1) from upload.file_kvar where file_id = {1} and dom_id = upload.{0}.dom_id) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3004, string.Format("Секция {0}, Некорретное количество строк, {1}", section.SectionName, value));
                }

                //Дома Код улицы КЛАДР	Код улицы КЛАДР состоит из 15 цифр
                sql = string.Format(" select 'значение - '||coalesce(ulica_kladr,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and char_length(ulica_kladr)<15 and ulica_kladr is not null ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0}, некорректное значение: Код улицы КЛАДР должен быть >= 15 цифр, {1}", section.SectionName, value));
                }

                //Код улицы ФИАС	Код ФИАС состоит из символов нижнего регистра и соответствует регулярному выражению
                //[0-9a-f]{8}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{12}
                //Пример значения:
                //9ed11393-e4ac-4df4-894c-487231464151
                sql = " select 'значение - '||coalesce(ulica_fias,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload." + section.SectionTable + " where file_id = " + uploadId +
                                    @" and not(ulica_fias SIMILAR TO '[0-9a-f]{8}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{12}') ";
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0}, некорректное значение: Код улицы ФИАС должен соответсвовать регулярному выражению, {1}", section.SectionName, value));
                }

                //Код улицы ФИАС	Код дома ФИАС состоит из символов нижнего регистра и соответствует регулярному выражению
                //[0-9a-f]{8}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{12}
                //Пример значения:
                //9ed11393-e4ac-4df4-894c-487231464151
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = " select 'значение - '||coalesce(dom_fias,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload." + section.SectionTable + " where file_id = " + uploadId +
                                    @" and not(dom_fias SIMILAR TO '[0-9a-f]{8}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{12}') ";
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3007, string.Format("Секция {0}, некорректное значение: Код дома ФИАС должен соответсвовать регулярному выражению, {1}", section.SectionName, value));
                }

                //Дополнительные параметры домов	Дата начала, дата окончания	Дата начала <= дата окончания
                section = headers.Find(x => x.SectionNumber == 15);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Дата начала действия значения - '||coalesce(act_from::varchar,'')||', Дата окончания действия значения - '||coalesce(act_to::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and act_from > act_to", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата начала действия значения > Даты окончания действия значения, {1}", section.SectionName, value));
                }

                //Лицевые счета	Код дома + Номер квартиры + Номер комнаты	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 16);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select max('Код дома - '||coalesce(dom_id::varchar,'')||', Номер квартиры - '||coalesce(nkvar::varchar,'')||', Номер комнаты - '||coalesce(nkom::varchar,'')) " +
                                " from upload.{0} where file_id = {1} group by dom_id,nkvar,nkom having (count(dom_id||nkvar||nkom)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Лицевые счета	Дата открытия ЛС, дата закрытия ЛС	Дата открытия <= дата закрытия ЛС
                sql = string.Format(" select 'Дата открытия ЛС - '||coalesce(opened_on::varchar,'')||', Дата закрытия ЛС - '||coalesce(closed_on::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and opened_on > closed_on", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата открытия ЛС > Даты закрытия ЛС, {1}", section.SectionName, value));
                }

                //Дополнительные параметры лицевых счетов	Дата начала, дата окончания	Дата начала <= дата окончания
                section = headers.Find(x => x.SectionNumber == 17);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Дата начала действия значения - '||coalesce(act_from::varchar,'')||', Дата окончания действия значения - '||coalesce(act_to::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and act_from > act_to", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата начала действия значения > Дата окончания действия значения, {1}", section.SectionName, value));
                }

                //Договоры на оказание ЖКУ	Код принципала + код поставщика + код расчетного счета + дата договора + номер договора + наименование договора	Проверка на уникальность
                section = headers.Find(x => x.SectionNumber == 18);
                sql = string.Format(" select max('Код принципала - '||coalesce(principal_id::varchar,'')||', код поставщика - '||coalesce(supplier_id::varchar,'')||', код расчетного счета - '||coalesce(bank_account_id::varchar,'')||', Дата договора - '||coalesce(contract_date::varchar,'')||', Номер договора - '||coalesce(contract_number::varchar,'')||', Наименование договора на оказание ЖКУ - '||coalesce(contract_name::varchar,'')) " +
                                " from upload.{0} where file_id = {1} group by principal_id,supplier_id,bank_account_id,contract_date,contract_number,contract_name having (count(principal_id||supplier_id||bank_account_id||contract_date||contract_number||contract_name)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Дополнительные параметры договоров на оказание ЖКУ	Дата начала, дата окончания	Дата начала <= дата окончания
                section = headers.Find(x => x.SectionNumber == 19);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Дата начала действия - '||coalesce(act_from::varchar,'')||', Дата окончания действия - '||coalesce(act_to::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and act_from > act_to", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата начала действия > Дата окончания действия , {1}", section.SectionName, value));
                }

                //Начисления и сальдо лицевых счетов	Поля для проверки исходящего сальдо	Расчет исходящего сальдо = Исходящее сальдо 
                //Расчет исходящего сальдо
                //Исходящее сальдо = Входящее сальдо 
                //+ Сумма начисления 
                //+ Сумма перерасчета начисления 
                //+ Сумма изменения сальдо 
                //– Сумма учтенной недопоставки 
                //– Сумма дотации 
                //– Сумма перерасчета дотации
                //– Сумма льготы
                //– Сумма перерасчета льготы 
                //– Сумма СМО
                //– Сумма перерасчета СМО
                //– Сумма оплаты
                section = headers.Find(x => x.SectionNumber == 20);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Расчет исходящего сальдо - '||(coalesce(sum_insaldo,0) + coalesce(sum_nach,0) + coalesce(sum_nach_p,0) + coalesce(sum_saldo_change,0) - coalesce(sum_nedo,0) - coalesce(sum_subs,0) - coalesce(sum_subs_p,0) - coalesce(sum_lgot,0) - coalesce(sum_lgot_p,0) - coalesce(sum_smo,0) - coalesce(sum_smo_p,0) - coalesce(sum_pay,0))||', Исходящее сальдо - '||coalesce(sum_outsaldo,0)||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} " +
                                    " and coalesce(sum_outsaldo,0) <> coalesce(sum_insaldo,0) + coalesce(sum_nach,0) + coalesce(sum_nach_p,0) + coalesce(sum_saldo_change,0) - coalesce(sum_nedo,0) - coalesce(sum_subs,0) - coalesce(sum_subs_p,0) - coalesce(sum_lgot,0) - coalesce(sum_lgot_p,0) - coalesce(sum_smo,0) - coalesce(sum_smo_p,0) - coalesce(sum_pay,0)", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3003, string.Format("Секция {0}, некорректное значение: Расчет исходящего сальдо <> Исходящему сальдо , {1}", section.SectionName, value));
                }

                //Начисления и сальдо лицевых счетов Сумма изменения сальдо	Сумма изменения сальдо = сумме сумм перекидок из «Изменения сальдо»
                if (FormatTemplate.HeaderList.First(x => x.SectionNumber == 21).IsUpload)
                {
                    sql =
                        string.Format(
                            " select 'Сумма изменения сальдо - '||coalesce(sum_saldo_change,0)||', строка из файла - '||replace(" +
                            listJoinFields + ",'\"\"','') " +
                            " from upload.{0} where file_id = {1} and coalesce(sum_saldo_change,0) <> (select sum(sum_saldo_change) from upload.file_saldo_change where file_id = {1} and upload.{0}.ls_id = ls_id and upload.{0}.serv_id = serv_id and upload.{0}.contract_id = contract_id )",
                            section.SectionTable, uploadId);
                    listValues = Connection.Query<string>(sql).ToList();
                    foreach (var value in listValues)
                    {
                        UploadErrors.GetWarningMessage(section.SectionFile, null, 3003,
                            string.Format(
                                "Секция {0}, некорректное значение: Сумма изменения сальдо <> сумме сумм перекидок из «Изменения сальдо», {1}",
                                section.SectionName, value));
                    }
                }
                //Начисления и сальдо лицевых счетов Сумма начисления	Сумма начисления = Расход фактический * Регулируемый тариф
                sql = string.Format(" select 'Сумма начисления - '||coalesce(sum_nach,0)||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                  " from upload.{0} where file_id = {1} and coalesce(sum_nach,0) <> round(coalesce(fact_rashod,0) * coalesce(nas_tarif,0),2)", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3003, string.Format("Секция {0}, некорректное значение: Сумма начисления <> Расход фактический * Регулируемый тариф, {1}", section.SectionName, value));
                }


                if (FormatTemplate.HeaderList.First(x => x.SectionNumber == 25).IsUpload)
                {
                    //Сумма перерасчета дотации 	Сумма перерасчета дотации  = сумма сумм перерасчета дотации из «Перерасчеты»
                    sql = string.Format(" select 'Сумма перерасчета дотации - '||coalesce(sum_subs_p,0)||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                      " from upload.{0} where file_id = {1} and coalesce(sum_subs_p,0) <> (select sum(sum_subs_p) from upload.file_recalc where file_id = {1} and upload.{0}.ls_id = ls_id and upload.{0}.serv_id = serv_id and upload.{0}.contract_id = contract_id)", section.SectionTable, uploadId);
                    listValues = Connection.Query<string>(sql).ToList();
                    foreach (var value in listValues)
                    {
                        UploadErrors.GetWarningMessage(section.SectionFile, null, 3003, string.Format("Секция {0}, некорректное значение: Сумма перерасчета дотации <> сумме сумм перерасчета дотации из «Перерасчеты», {1}", section.SectionName, value));
                    }

                    //Сумма перерасчета льготы 	Сумма перерасчета льготы = сумма сумм перерасчета льготы из «Перерасчеты»
                    sql = string.Format(" select 'Сумма перерасчета льготы - '||coalesce(sum_lgot_p,0)||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                      " from upload.{0} where file_id = {1} and coalesce(sum_lgot_p,0) <> (select sum(sum_lgot_p) from upload.file_recalc where file_id = {1} and upload.{0}.ls_id = ls_id and upload.{0}.serv_id = serv_id and upload.{0}.contract_id = contract_id)", section.SectionTable, uploadId);
                    listValues = Connection.Query<string>(sql).ToList();
                    foreach (var value in listValues)
                    {
                        UploadErrors.GetWarningMessage(section.SectionFile, null, 3003, string.Format("Секция {0}, некорректное значение: Сумма перерасчета льготы <> сумме сумм перерасчета льготы из «Перерасчеты», {1}", section.SectionName, value));
                    }

                    //Сумма перерасчета СМО 	Сумма перерасчета СМО = сумма сумм перерасчета СМО из «Перерасчеты»
                    sql = string.Format(" select 'Сумма перерасчета СМО - '||coalesce(sum_smo_p,0)||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                      " from upload.{0} where file_id = {1} and coalesce(sum_smo_p,0) <> (select sum(sum_smo_p) from upload.file_recalc where file_id = {1} and upload.{0}.ls_id = ls_id and upload.{0}.serv_id = serv_id and upload.{0}.contract_id = contract_id)", section.SectionTable, uploadId);
                    listValues = Connection.Query<string>(sql).ToList();
                    foreach (var value in listValues)
                    {
                        UploadErrors.GetWarningMessage(section.SectionFile, null, 3003, string.Format("Секция {0}, некорректное значение: Сумма перерасчета СМО <> сумме сумм перерасчета СМО из «Перерасчеты», {1}", section.SectionName, value));
                    }

                    //Количество строк перерасчета	Количество строк перерасчета = Количество строки из «Перерасчеты»
                    sql = string.Format(" select 'Количество строк перерасчета - '||coalesce(recalc_count,0)||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                      " from upload.{0} where file_id = {1} and coalesce(recalc_count,0) <> (select count(1) from upload.file_recalc where file_id = {1} and upload.{0}.ls_id = ls_id and upload.{0}.serv_id = serv_id and upload.{0}.contract_id = contract_id)", section.SectionTable, uploadId);
                    listValues = Connection.Query<string>(sql).ToList();
                    foreach (var value in listValues)
                    {
                        UploadErrors.GetWarningMessage(section.SectionFile, null, 3004, string.Format("Секция {0}, некорректное значение: Количество строк перерасчета <> Количество строки из «Перерасчеты», {1}", section.SectionName, value));
                    }
                }

                //Недопоставки услуг	Дата начала недопоставки, Дата окончания недопоставки	Дата окончания недопоставки <= Дата окончания недопоставки
                section = headers.Find(x => x.SectionNumber == 22);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Дата начала недопоставки - '||coalesce(nedo_from::varchar,'')||', Дата окончания недопоставки - '||coalesce(nedo_to::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and nedo_to < nedo_from", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата начала недопоставки > Дата окончания недопоставки, {1}", section.SectionName, value));
                }

                //Дополнительные параметры лицевых счетов в месяце перерасчета	Дата начала, дата окончания	Дата начала <= дата окончания
                section = headers.Find(x => x.SectionNumber == 24);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Дата начала - '||coalesce(act_from::varchar,'')||', Дата окончания - '||coalesce(act_to::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and act_to < act_from", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата начала > Даты окончания, {1}", section.SectionName, value));
                }

                //Приборы учета	Дата поверки, дата следующей поверки	Дата поверки <= дата следующей поверки
                section = headers.Find(x => x.SectionNumber == 26);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Дата поверки - '||coalesce(check_date::varchar,'')||', дата следующей поверки - '||coalesce(next_check_date::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and next_check_date < check_date", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата поверки > даты следующей поверки, {1}", section.SectionName, value));
                }

                //Приборы учета Вид  + Код дома + код ЛС + код услуги + Тип (модель) + заводской номер	Проверка на уникальность
                sql = string.Format(" select max('Вид - '||coalesce(pu_type_id::varchar,'')||', код дома - '||coalesce(dom_id::varchar,'')||', код ЛС- '||coalesce(ls_id::varchar,'')||', код услуги - '||coalesce(serv_id::varchar,'')||', Тип (модель) - '||coalesce(pu_model::varchar,'')||', заводской номер - '||coalesce(pu_number::varchar,'')) " +
                                " from upload.{0} where file_id = {1} group by pu_type_id,dom_id,ls_id,serv_id,pu_model,pu_number having (count(pu_type_id||dom_id||ls_id||serv_id||pu_model||pu_number)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Приборы учета Вид ПУ = групповой или общеквартирный	Проверка наличия ЛС в «Лицевые счета групповых/ общеквартирных приборов учета»
                sql = string.Format(" select 'Уникальный код прибора учета в системе поставщика - '||coalesce(pu_id::varchar,'') " +
                                " from upload.{0} where file_id = {1} and pu_type_id in (2,4) and not exists (select pu_id from upload.file_pu_kvar where upload.{0}.pu_id = pu_id) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3006, string.Format("Секция {0},некорректные значения: {1}", section.SectionName, value));
                }

                //Дополнительные параметры приборов учета	Дата начала, дата окончания	Дата начала <= дата окончания
                section = headers.Find(x => x.SectionNumber == 28);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Дата начала - '||coalesce(act_from::varchar,'')||', Дата окончания - '||coalesce(act_to::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and act_to < act_from", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата начала > Даты окончания, {1}", section.SectionName, value));
                }

                //Информация по проживающим	Дата рождения, дата выдачи удостоверения	Дата рождения <= дата выдачи удостоверения
                section = headers.Find(x => x.SectionNumber == 30);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Дата рождения - '||coalesce(born_on::varchar,'')||', Дата выдачи удостоверения - '||coalesce(issued_on::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and issued_on < born_on and issued_on is not null", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата рождения > даты выдачи удостоверения, {1}", section.SectionName, value));
                }

                //Информация по проживающим	Дата рождения, Дата регистрации	Дата рождения <= Дата регистрации
                sql = string.Format(" select 'Дата рождения - '||coalesce(born_on::varchar,'')||', Дата регистрации - '||coalesce(registered_on::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and registered_on < born_on and registered_on is not null", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата рождения > Даты регистрации, {1}", section.SectionName, value));
                }

                //Дата рождения, Дата постановки на воинский учет	Дата рождения <= Дата постановки на воинский учет
                sql = string.Format(" select 'Дата рождения - '||coalesce(born_on::varchar,'')||', Дата постановки на воинский учет - '||coalesce(army_registered_on::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and army_registered_on < born_on and army_registered_on is not null", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата рождения > Даты постановки на воинский учет, {1}", section.SectionName, value));
                }

                //Дата рождения, Дата составления	Дата рождения <= Дата составления
                sql = string.Format(" select 'Дата рождения - '||coalesce(born_on::varchar,'')||', Дата составления - '||coalesce(created_on::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and created_on < born_on and created_on is not null", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата рождения > Даты составления, {1}", section.SectionName, value));
                }

                //Дата рождения, Дата рождения, Дата оформления	Дата рождения <= Дата оформления
                sql = string.Format(" select 'Дата рождения - '||coalesce(born_on::varchar,'')||', Дата оформления - '||coalesce(filled_on::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and filled_on < born_on and filled_on is not null", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата рождения > Даты оформления, {1}", section.SectionName, value));
                }

                //Дата регистрации, дата окончания регистрации	Дата регистрации <= дата окончания регистрации
                sql = string.Format(" select 'Дата рождения - '||coalesce(born_on::varchar,'')||', Дата окончания регистрации - '||coalesce(registered_off::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and registered_off < born_on and registered_off is not null", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата рождения > Даты окончания регистрации, {1}", section.SectionName, value));
                }

                //Дата постановки на воинский учет, дата снятия с воинского учета	Дата постановки на воинский учет <= дата снятия с воинского учета
                sql = string.Format(" select 'Дата постановки на воинский учет - '||coalesce(army_registered_on::varchar,'')||', Дата снятия с воинского учета - '||coalesce(army_registered_off::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and army_registered_off < army_registered_on and army_registered_on is not null and army_registered_off is not null", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата постановки на воинский учет > Даты снятия с воинского учета, {1}", section.SectionName, value));
                }

                //Код гражданина + Код ЛС + Тип + Наименование + серия + номер удостоверения личности	Проверка на уникальность
                sql = string.Format(" select max('Код гражданина - '||coalesce(gil_id::varchar,'')||', Код ЛС - '||coalesce(ls_id::varchar,'')||', Тип - '||coalesce(kart_type_id::varchar,'')||', Наименование - '||coalesce(passport::varchar,'')||', серия - '||coalesce(pass_ser::varchar,'')||', заводской номер - '||coalesce(pass_number::varchar,'')) " +
                                 " from upload.{0} where file_id = {1} group by gil_id,ls_id,kart_type_id,passport,pass_ser,pass_number having (count(gil_id||ls_id||kart_type_id||passport||pass_ser||pass_number)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Код гражданика + Код ЛС + ФИО + дата рождения	Проверка на уникальность
                sql = string.Format(" select max('Код гражданина - '||coalesce(gil_id::varchar,'')||', Код ЛС - '||coalesce(ls_id::varchar,'')||', ФИО - '||coalesce(fam,'')||coalesce(ima,'')||coalesce(otch,'')||', Дата рождения - '||coalesce(passport::varchar,'')) " +
                                 " from upload.{0} where file_id = {1} group by gil_id,ls_id,fam,ima,otch,born_on having (count(gil_id||ls_id||coalesce(fam,'')||coalesce(ima,'')||coalesce(otch,'')||born_on)>1) ", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3001, string.Format("Секция {0}, не обеспечена уникальность: {1}", section.SectionName, value));
                }

                //Реестр временно выбывших	Дата начала, дата окончания	Дата начала <= дата окончания
                section = headers.Find(x => x.SectionNumber == 31);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Дата начала убытия - '||coalesce(leave_from::varchar,'')||', Дата окончания убытия - '||coalesce(leave_to::varchar,'')||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                    " from upload.{0} where file_id = {1} and leave_to < leave_from", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3002, string.Format("Секция {0}, некорректное значение: Дата начала убытия > Даты окончания убытия, {1}", section.SectionName, value));
                }

                //Пачки оплат ЖКУ	Количество платежей, вошедших в пачку	Количество платежей должно быть равно количеству оплат из "Оплаты ЖКУ"
                section = headers.Find(x => x.SectionNumber == 32);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Количество платежей вошедших в пачку - '||coalesce(pay_count,0)||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                  " from upload_part.{0}_{1} where file_id = {1} and coalesce(pay_count,0) <> (select count(1) from upload_part.file_pay_{1} where upload_part.{0}_{1}.pack_id = pack_id)", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3004, string.Format("Секция {0}, некорректное значение: Количество платежей <> Количеству оплат из Оплаты ЖКУ, {1}", section.SectionName, value));
                }

                //Пачки оплат ЖКУ	Сумма платежей	Сумма платежей = Сумма сумм оплаты из "Оплаты ЖКУ"
                sql = string.Format(" select 'Сумма платежей - '||coalesce(sum_pay,0)||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                  " from upload.{0} b where b.file_id = {1} and coalesce(b.sum_pay,0) <> coalesce((select sum(a.sum_pay) from upload.file_pay a where a.file_id = {1} and b.pack_id = a.pack_id),0)", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3003, string.Format("Секция {0}, некорректное значение: Сумма платежей <> Сумме сумм оплаты из Оплаты ЖКУ, {1}", section.SectionName, value));
                }

                //Пачки оплат ЖКУ	Сумма оплаты 	Сумма оплаты = Сумма из "Расщепление оплат"
                section = headers.Find(x => x.SectionNumber == 33);
                listJoinFields = GetAllSectionField(section.SectionTable);
                sql = string.Format(" select 'Сумма оплаты - '||coalesce(sum_pay,0)||', строка из файла - '||replace(" + listJoinFields + ",'\"\"','') " +
                                  " from upload.{0} where file_id = {1} and coalesce(sum_pay,0) <> (select sum(sum_distr) from upload.file_pay_distr where file_id = {1} and upload.{0}.pay_id = pay_id)", section.SectionTable, uploadId);
                listValues = Connection.Query<string>(sql).ToList();
                foreach (var value in listValues)
                {
                    UploadErrors.GetWarningMessage(section.SectionFile, null, 3003, string.Format("Секция {0}, некорректное значение: Сумма оплаты <> Сумме из Расщепление оплат, {1}", section.SectionName, value));
                }
            }
            RegisterFormatForOtherSystems.UpdateFileProgress(0.3m);
            return new BaseDataResult();
        }

        private string GetAllSectionField(string sectionTable)
        {
            return string.Join("||';'||", FormatTemplate.Format[sectionTable].Select(x => "coalesce(" + x.ColumnName + "::varchar,'')"));
        }
    }
}
