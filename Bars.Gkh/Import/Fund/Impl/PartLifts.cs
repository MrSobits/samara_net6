namespace Bars.Gkh.Import.Fund.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;
    using Castle.Windsor;
    using System.Collections.Generic;

    public class PartLifts : ITechPassportPartImport
    {
        public IRepository<RealityObject> RealtyObjectRepository { get; set; }

        public IRepository<TehPassport> TehPassportRepository { get; set; }

        public IRepository<TehPassportValue> TehPassportValueRepository { get; set; }

        public IWindsorContainer Container { get; set; }

        private ILogImport LogImport;

        protected Dictionary<string, KeyValuePair<string, string>> dictCodes;

        private Dictionary<long, Dictionary<string, Dictionary<string, ExistingTechPassportValueProxy>>> realtyObjectTechPassportDataDict;

        private Dictionary<long, long> realtyObjectTechPassportDict;

        private Dictionary<string, long> robjectIdByFederalNumDict;

        protected bool ReplaceData;

        public void Import(MemoryStream memoryStream, ILogImport logImport, Dictionary<string, long> robjIdByFederalNumDict,
                           Dictionary<long, long> tehPassportIdByRobjectIdDict, Dictionary<long, Dictionary<string, Dictionary<string, ExistingTechPassportValueProxy>>> existingDataDict, bool replaceData)
        {
            ReplaceData = replaceData;
            LogImport = logImport;
            realtyObjectTechPassportDataDict = existingDataDict;
            realtyObjectTechPassportDict = tehPassportIdByRobjectIdDict;
            robjectIdByFederalNumDict = robjIdByFederalNumDict;

            InitDictCodes();

            DoImport(memoryStream);
        }

        public static string Code
        {
            get
            {
                return "RASP_PART_LIFT";
            }
        }

        string ITechPassportPartImport.Code
        {
            get
            {
                return Code;
            }
        }

        protected string ImportCode = "RASP_PART_LIFTS-Техпаспорт";

        private string FormLiftCode = "Form_4_2_1";

        protected void InitDictCodes()
        {
            dictCodes = new Dictionary<string, KeyValuePair<string, string>>
                {
                    {"CAPACITY", new KeyValuePair<string, string>(this.FormLiftCode, "5")},
                    {"COUNT_STOP", new KeyValuePair<string, string>(this.FormLiftCode, "7")},
                    {"LIFT_TONNEL", new KeyValuePair<string, string>(this.FormLiftCode, "8")},
                    {"YEAR_START", new KeyValuePair<string, string>(this.FormLiftCode, "9")},
                    {"YEAR_UP", new KeyValuePair<string, string>(this.FormLiftCode, "10")}
                };
        }

        private void DoImport(MemoryStream memoryStream)
        {
            if (dictCodes == null)
            {
                LogImport.Error(ImportCode, "Импорт данного раздела не реализован");
                return;
            }

            var dictHeaders = dictCodes.Keys.ToDictionary(x => x, y => 0);

            var allFields = new List<LiftProxy>();
            using (var sr = new StreamReader(memoryStream, Encoding.GetEncoding(1251)))
            {
                // считывание заголовков
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

                int rowNumber = 1;

                // читаем данные
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

                    var lifts = new LiftProxy()
                        {
                            idMkd = federalNumber,
                            Capacity = row[dictHeaders["CAPACITY"]],
                            Count_Stop = row[dictHeaders["COUNT_STOP"]],
                            Lift_Tonnel = row[dictHeaders["LIFT_TONNEL"]],
                            Year_Start = row[dictHeaders["YEAR_START"]],
                            Year_Up = row[dictHeaders["YEAR_UP"]]
                        };

                    this.GetCorrectLiftFields(allFields, lifts);

                    LogImport.Info(ImportCode, string.Format("Cтрока: {0}; ID_MKD: {1}; Успешно", rowNumber, federalNumber));
                }
            }

            var robjectLifts = allFields
                .GroupBy(x => robjectIdByFederalNumDict.ContainsKey(x.idMkd) ? robjectIdByFederalNumDict[x.idMkd] : -1)
                .ToDictionary(x => x.Key, x => x.ToList());

            var liftsToCreate = new Dictionary<long, List<LiftProxy>>();
            var listTodelete = new List<ExistingTechPassportValueProxy>();
            foreach (var robjectLift in robjectLifts)
            {
                if (realtyObjectTechPassportDataDict.ContainsKey(robjectLift.Key))
                {
                    if (realtyObjectTechPassportDataDict[robjectLift.Key].ContainsKey(this.FormLiftCode))
                    {
                        var existFields = realtyObjectTechPassportDataDict[robjectLift.Key][FormLiftCode];

                        existFields.ForEach(x => listTodelete.Add(x.Value));

                        realtyObjectTechPassportDataDict[robjectLift.Key].Remove(FormLiftCode);
                    }

                    liftsToCreate[robjectLift.Key] = robjectLift.Value;
                }
                else
                {
                    liftsToCreate[robjectLift.Key] = robjectLift.Value;
                }
            }

            //this.InTransaction(() => listTodelete.ForEach(x => this.TehPassportValueRepository.Delete(x)));
            var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();

            this.InTransaction(() => listTodelete.ForEach(value =>
                {
                     var deleteValue = new TehPassportValue
                    {
                        Id = value.Id,
                        TehPassport = new TehPassport { Id = value.TechPassportId }
                    };

                    session.Delete(deleteValue);
                }));

            // запускаем транзакции по 100 домов
            const int PoolSize = 100;
            var current = 0;
            while (current < liftsToCreate.Count)
            {
                var pool = liftsToCreate.Skip(current).Take(PoolSize).ToArray();

                this.InTransaction(() => pool.ForEach(robjectdata => this.SaveLifts(robjectdata.Key, robjectdata.Value)));

                current += PoolSize;
            }

        }


        private void GetCorrectLiftFields(List<LiftProxy> allFields, LiftProxy lift)
        {
            var correctField = new LiftProxy
                {
                    idMkd = lift.idMkd,
                    Capacity = this.isDecimal(lift.Capacity) ? lift.Capacity : null,
                    Count_Stop = this.isInt(lift.Count_Stop) ? lift.Count_Stop : null,
                    Lift_Tonnel = this.isTypeLiftShaft(ref lift.Lift_Tonnel) ? lift.Lift_Tonnel : null,
                    Year_Start = this.isInt(lift.Year_Start) ? lift.Year_Start : null,
                    Year_Up = this.isInt(lift.Year_Up) ? lift.Year_Up : null
                };

            if (correctField.Capacity != null
                || correctField.Count_Stop != null
                || correctField.Lift_Tonnel != null
                || correctField.Year_Start != null
                || correctField.Year_Up != null)
            {
                allFields.Add(correctField);
            }
        }

        private void SaveLifts(long roId, List<LiftProxy> listToCreate)
        {
            TehPassport tehPassport;
            if (realtyObjectTechPassportDict.ContainsKey(roId))
            {
                tehPassport = TehPassportRepository.Load(realtyObjectTechPassportDict[roId]);
            }
            else
            {
                tehPassport = new TehPassport {RealityObject = RealtyObjectRepository.Load(roId)};

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
                    realtyObjectTpDataDict =
                        new Dictionary<string, Dictionary<string, ExistingTechPassportValueProxy>>();
                    realtyObjectTechPassportDataDict[roId] = realtyObjectTpDataDict;
                }

                int liftIndex = 0;

                foreach (var valueToCreate in listToCreate)
                {
                    var list = new List<TehPassportValue>
                        {
                            new TehPassportValue
                                {
                                    FormCode = this.FormLiftCode,
                                    CellCode = string.Format("{0}:5", liftIndex),
                                    Value = valueToCreate.Capacity
                                },
                            new TehPassportValue
                                {
                                    FormCode = this.FormLiftCode,
                                    CellCode = string.Format("{0}:7", liftIndex),
                                    Value = valueToCreate.Count_Stop
                                },
                            new TehPassportValue
                                {
                                    FormCode = this.FormLiftCode,
                                    CellCode = string.Format("{0}:8", liftIndex),
                                    Value = valueToCreate.Lift_Tonnel
                                },
                            new TehPassportValue
                                {
                                    FormCode = this.FormLiftCode,
                                    CellCode = string.Format("{0}:9", liftIndex),
                                    Value = valueToCreate.Year_Start
                                },
                            new TehPassportValue
                                {
                                    FormCode = this.FormLiftCode,
                                    CellCode = string.Format("{0}:10", liftIndex),
                                    Value = valueToCreate.Year_Up
                                }
                        };

                    foreach (var tehPassportValue in list.Where(x => !string.IsNullOrWhiteSpace(x.Value)))
                    {
                        var newValue = new TehPassportValue
                            {
                                TehPassport = tehPassport,
                                FormCode = tehPassportValue.FormCode,
                                CellCode = tehPassportValue.CellCode,
                                Value = tehPassportValue.Value
                            };

                        this.TehPassportValueRepository.Save(newValue);

                        this.LogImport.CountAddedRows++;

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
                            realtyObjectTpDataDict[newValue.FormCode] =
                                new Dictionary<string, ExistingTechPassportValueProxy> {{newValue.CellCode, newVal}};
                        }

                        ++liftIndex;
                    }
                }
            }
        }

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

        private bool isTypeLiftShaft(ref string value)
        {
            var code = "-1";
            switch (value.ToUpper())
            {
                case "НЕ ЗАДАНО":
                    code = "0";
                    break;
                case "ВСТРОЕННАЯ":
                    code = "1";
                    break;
                case "ПРИСТАВНАЯ":
                    code = "2";
                    break;      
            }

            value = code;
            return code != "-1";
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

        private class LiftProxy
        {
            public string idMkd;
            public string Capacity;
            public string Count_Stop;
            public string Lift_Tonnel;
            public string Year_Start;
            public string Year_Up;
        }

        protected sealed class TehPassportValueProxy
        {
            public string FormCode;

            public string CellCode;

            public string Value;
        }
    }
}