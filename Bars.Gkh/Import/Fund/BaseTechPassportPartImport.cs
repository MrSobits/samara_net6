namespace Bars.Gkh.Import.Fund
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;

    using Castle.Windsor;

    public abstract class BaseTechPassportPartImport
    {
        public IRepository<RealityObject> RealtyObjectRepository { get; set; }

        public IRepository<TehPassport> TehPassportRepository { get; set; }

        public IRepository<TehPassportValue> TehPassportValueRepository { get; set; }

        public IWindsorContainer Container { get; set; }

        protected sealed class TehPassportValueProxy
        {
            public string FormCode;

            public string CellCode;

            public string Value;
        }

        protected Dictionary<string, KeyValuePair<string, string>> dictCodes;

        protected abstract void InitDictCodes();

        private Dictionary<long, Dictionary<string, Dictionary<string, ExistingTechPassportValueProxy>>> realtyObjectTechPassportDataDict;

        private Dictionary<long, long> realtyObjectTechPassportDict;

        private Dictionary<string, long> robjectIdByFederalNumDict;

        protected bool ReplaceData;

        protected ILogImport LogImport;

        protected abstract string ImportCode { get; }
        
        public void Import(
            MemoryStream memoryStream, 
            ILogImport logImport,
            Dictionary<string, long> robjIdByFederalNumDict,
            Dictionary<long, long> tehPassportIdByRobjectIdDict,
            Dictionary<long, Dictionary<string, Dictionary<string, ExistingTechPassportValueProxy>>> existingDataDict, 
            bool replaceData)
        {
            ReplaceData = replaceData;
            LogImport = logImport;
            realtyObjectTechPassportDataDict = existingDataDict;
            realtyObjectTechPassportDict = tehPassportIdByRobjectIdDict;
            robjectIdByFederalNumDict = robjIdByFederalNumDict;

            InitDictCodes();

            DoImport(memoryStream);
        }

        private void DoImport(MemoryStream memoryStream)
        {
            if (dictCodes == null)
            {
                LogImport.Error(ImportCode, "Коды соответствия раздела реализованы неверно");
                return;
            }

            var dictHeaders = dictCodes.Keys.ToDictionary(x => x, y => 0);

            var robjectData = new Dictionary<long, List<TehPassportValueProxy>>();

            using (var sr = new StreamReader(memoryStream, Encoding.GetEncoding(1251)))
            {
                //считывание заголовков
                var headers = sr.ReadLine().Split(';').Select(x => x.Trim('"')).ToArray();

                for (int i = 0; i < headers.Length; i++)
                {
                    if (dictHeaders.ContainsKey(headers[i]))
                    {
                        dictHeaders[headers[i]] = i;
                    }
                }

                if (!dictHeaders.Values.Any(x => x > 0))
                {
                    LogImport.Warn(ImportCode, "Отсутствует соответствующий заголовок");
                    return;
                }

                var fieldInfoDict = dictCodes.ToDictionary(
                    x => x.Key, 
                    x => new
                        {
                            FormCode = x.Value.Key, 
                            CellCode = x.Value.Value,
                            index = dictHeaders[x.Key]
                        });

                int rowNumber = 1;

                //читаем данные
                while (!sr.EndOfStream)
                {
                    rowNumber++;
                    var line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var row = line.Split(';').Select(x => x ?? "").ToArray();

                    if (row.Length == 0)
                    {
                        LogImport.Warn(ImportCode, string.Format("Cтрока: {0}; Строка без данных", rowNumber));
                        continue;
                    }

                    var federalNumber = row[0];

                    if (string.IsNullOrEmpty(federalNumber))
                    {
                        LogImport.Warn(ImportCode, string.Format("Cтрока: {0};  Не указан федеральный номер дома", rowNumber));
                        continue;
                    }

                    if (!robjectIdByFederalNumDict.ContainsKey(federalNumber))
                    {
                        LogImport.Warn(ImportCode, string.Format("Cтрока: {0}; ID_MKD: {1}; Не удалось получить дом по федеральному номеру", rowNumber, federalNumber));
                        continue;
                    }

                    var rowFieldsNum = row.Length;

                    var data = fieldInfoDict
                        .Where(x => x.Value.index < rowFieldsNum) 
                        .Select(x => new { x.Value.FormCode, x.Value.CellCode, value = row[x.Value.index].Trim() })
                        .Where(x => ReplaceData || !string.IsNullOrWhiteSpace(x.value))
                        .Select(x => GetCorrectValue(x.FormCode, x.CellCode, x.value))
                        .Where(x => x != null)
                        .Where(x => !string.IsNullOrEmpty(x.FormCode))
                        .Where(x => !string.IsNullOrEmpty(x.CellCode))
                        .ToList();

                    var groupedData = data.GroupBy(x => new { x.FormCode, x.CellCode })
                                            .Select(x => new
                                                {
                                                    count = x.Count(),
                                                    valueList = x.ToList()
                                                })
                                            .ToList();

                    var uniqueData = groupedData.Where(x => x.count == 1).Select(x => x.valueList.First()).ToList();

                    var nonUniqueData = groupedData.Where(x => x.count != 1).Select(x => x.valueList).ToArray();

                    if (nonUniqueData.Any())
                    {
                        var allDataValid = true;
                        var conflictedFieldsGroups = new List<List<string>>();

                        foreach (var tehPassportValueProxyList in nonUniqueData)
                        {
                            var firstValue = tehPassportValueProxyList.First();
                            var value = firstValue.Value;

                            var valid = tehPassportValueProxyList.All(x => x.Value == value);

                            if (valid)
                            {
                                uniqueData.Add(firstValue);
                            }
                            else
                            {
                                var fields = dictCodes
                                    .Where(x => x.Value.Key == firstValue.FormCode)
                                    .Where(x => x.Value.Value == firstValue.CellCode)
                                    .Select(x => x.Key)
                                    .ToList();

                                if (fields.Any())
                                {
                                    conflictedFieldsGroups.Add(fields);
                                }
                            }

                            allDataValid &= valid;
                        }

                        if (allDataValid)
                        {
                            LogImport.Info(ImportCode, string.Format("Cтрока: {0}; ID_MKD: {1}; Успешно", rowNumber, federalNumber));
                        }
                        else
                        {
                            var groups = conflictedFieldsGroups.Select(x => string.Format("{{{0}}}", String.Join(", ", x))).ToArray();

                            LogImport.Warn(
                                    ImportCode,
                                    string.Format(
                                        "Cтрока: {0}; ID_MKD: {1}; Противоречивые данные слудеющих группах полей: {2} ", 
                                        rowNumber, 
                                        federalNumber,
                                        String.Join(", ", groups)));
                        }
                    }
                    else
                    {
                        LogImport.Info(ImportCode, string.Format("Cтрока: {0}; ID_MKD: {1}; Успешно", rowNumber, federalNumber));
                    }

                    robjectData[robjectIdByFederalNumDict[federalNumber]] = uniqueData;
                }
            }

            // запускаем транзакции по 100 домов
            const int PoolSize = 100;
            var current = 0;
            while (current < robjectData.Count)
            {
                var pool = robjectData.Skip(current).Take(PoolSize).ToArray();

                this.InTransaction(() => pool.ForEach(robjectdata => SaveRealtyObjectTpData(robjectdata.Key, robjectdata.Value)));

                current += PoolSize;
            }
        }

        protected virtual void SaveRealtyObjectTpData(long roId, List<TehPassportValueProxy> tpData)
        {
            if (!tpData.Any())
            {
                return;
            }

            // Список записей для создания
            var listToCreate = new List<TehPassportValueProxy>();

            // Список записей на обновление
            var listToUpdate = new List<ExistingTechPassportValueProxy>();

            if (realtyObjectTechPassportDataDict.ContainsKey(roId))
            {
                var existingTpData = realtyObjectTechPassportDataDict[roId];

                foreach (var data in tpData)
                {
                    if (existingTpData.ContainsKey(data.FormCode) && existingTpData[data.FormCode].ContainsKey(data.CellCode))
                    {
                        var tehPassportValue = existingTpData[data.FormCode][data.CellCode];

                        if (ReplaceData && tehPassportValue.Value != data.Value)
                        {
                            tehPassportValue.Value = data.Value;
                            listToUpdate.Add(tehPassportValue);
                        }
                    }
                    else if (!string.IsNullOrEmpty(data.Value))
                    {
                        listToCreate.Add(data);
                    }
                }
            }
            else
            {
                listToCreate = tpData.Where(x => !string.IsNullOrEmpty(x.Value)).ToList();
            }

            if (!listToCreate.Any() && !listToUpdate.Any())
            {
                return;
            }

            this.DoSave(roId, listToCreate, listToUpdate);
        }

        private void DoSave(long roId, List<TehPassportValueProxy> listToCreate, List<ExistingTechPassportValueProxy> listToUpdate)
        {
            foreach (var value in listToUpdate)
            {
                //var value = TehPassportValueRepository.Load(updateValuePair.Key);
                //value.Value = updateValuePair.Value;
                //TehPassportValueRepository.Update(value);
                
                // Закомментил вышеописанное, т.к. слишком большие затраты идут на каскадирование записей ФИАС
                
                var updateValue = new TehPassportValue
                    {
                        Id = value.Id,
                        ObjectCreateDate = value.ObjectCreateDate,
                        ObjectVersion = value.ObjectVersion + 1,
                        TehPassport = new TehPassport { Id = value.TechPassportId },
                        FormCode = value.FormCode,
                        CellCode = value.CellCode,
                        Value = value.Value
                    };

                TehPassportValueRepository.Update(updateValue);

                realtyObjectTechPassportDataDict[roId][updateValue.FormCode][updateValue.CellCode] = value;

                LogImport.CountChangedRows++;
            }

            TehPassport tehPassport;
            if (realtyObjectTechPassportDict.ContainsKey(roId))
            {
                tehPassport = TehPassportRepository.Load(realtyObjectTechPassportDict[roId]);
            }
            else
            {
                tehPassport = new TehPassport { RealityObject = RealtyObjectRepository.Load(roId) };

                TehPassportRepository.Save(tehPassport);

                realtyObjectTechPassportDict[roId] = tehPassport.Id;
            }

            if (listToCreate.Any())
            {
                Dictionary<string, Dictionary<string, ExistingTechPassportValueProxy>> realtyObjectTpDataDict;

                if (realtyObjectTechPassportDataDict.ContainsKey(roId))
                {
                    realtyObjectTpDataDict = realtyObjectTechPassportDataDict[roId];
                }
                else
                {
                    realtyObjectTpDataDict = new Dictionary<string, Dictionary<string, ExistingTechPassportValueProxy>>();
                    realtyObjectTechPassportDataDict[roId] = realtyObjectTpDataDict;
                }

                foreach (var valueToCreate in listToCreate)
                {
                    var newValue = new TehPassportValue
                    {
                        TehPassport = tehPassport,
                        FormCode = valueToCreate.FormCode,
                        CellCode = valueToCreate.CellCode,
                        Value = valueToCreate.Value
                    };

                    TehPassportValueRepository.Save(newValue);

                    LogImport.CountAddedRows++;

                    var newVal = new ExistingTechPassportValueProxy
                                    {
                                        Id = newValue.Id,
                                        ObjectCreateDate = newValue.ObjectCreateDate,
                                        ObjectVersion = newValue.ObjectVersion,
                                        TechPassportId = newValue.TehPassport.Id,
                                        FormCode = newValue.FormCode,
                                        CellCode = newValue.CellCode,
                                        Value = newValue.Value
                                    };

                    if (realtyObjectTpDataDict.ContainsKey(newValue.FormCode))
                    {
                        realtyObjectTpDataDict[newValue.FormCode][newValue.CellCode] = newVal;
                    }
                    else
                    {
                        realtyObjectTpDataDict[newValue.FormCode] = new Dictionary<string, ExistingTechPassportValueProxy> { { newValue.CellCode, newVal } };
                    }
                }
            }
        }

        protected abstract TehPassportValueProxy GetCorrectValue(string formCode, string cellCode, string value);

        protected void InTransaction(Action action)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }

        protected bool isInt(string value)
        {
            int intVal;

            return int.TryParse(value, out intVal);
        }

        protected bool isDecimal(string value)
        {
            decimal decimalVal;

            return decimal.TryParse(value, out decimalVal);
        }

        protected bool isBool(string value)
        {
            if (value == "1" || value == "0")
            {
                return true;
            }
            else
            {
                bool boolVal;

                return bool.TryParse(value, out boolVal);
            }
        }
    }
}