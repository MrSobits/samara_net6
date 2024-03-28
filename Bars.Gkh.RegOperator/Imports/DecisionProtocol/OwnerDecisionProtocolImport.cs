namespace Bars.Gkh.RegOperator.Imports.DecisionProtocol
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Proxies;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.GkhExcel;

    using Castle.Windsor;
    using Import.Impl;

    public sealed class OwnerDecisionProtocolImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        
        /// <summary>
        ///     Логгер импорта
        /// </summary>
        public ILogImport LogImport { get; set; }

        /// <summary>
        ///     Менеджер логгеров импорта
        /// </summary>
        public ILogImportManager LogImportManager { get; set; }

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key
        {
            get
            {
                return Id;
            }
        }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport
        {
            get
            {
                return "OwnerDecisionProtocolImport";
            }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get
            {
                return "Импорт протоколов решений собственников";
            }
        }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions
        {
            get
            {
                return "xlsx";
            }
        }

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName
        {
            get
            {
                return "Import.OwnerDecisionProtocolImport";
            }
        }

        public class ParsedRow
        {
            public int RowNumber { get; set; }

            public string Index { get; set; }

            public string Kladr { get; set; }

            public string MR { get; set; }

            public string MO { get; set; }

            public string City { get; set; }

            public string Street { get; set; }

            public string House { get; set; }

            public string MkdService { get; set; }

            public string IsInRegProgram { get; set; }

            public string MeetingDate { get; set; }

            public string FIO { get; set; }

            public string Phone { get; set; }

            public string KrFondFormation { get; set; }

            public string Payment { get; set; }

            public string CreditOrg { get; set; }

            public string MinFond { get; set; }

            public string Income { get; set; }

            public string Status { get; set; }

            public string Additional { get; set; }

            public string BankName { get; set; }

            public string INN { get; set; }

            public string KPP { get; set; }

            public string KS { get; set; }

            public string BIK { get; set; }

            public string Person { get; set; }
        }

        private RealityObject TryGetRealityObject(ParsedRow row)
        {
            //если в файлике указан индекс, и в системе существует единственная запись с таким кодом - берем его
            if (!string.IsNullOrWhiteSpace(row.House)
                && _robjectByCodeCache.ContainsKey(row.House)
                && _robjectByCodeCache[row.House].Count == 1)
            {
                return _robjectByCodeCache[row.House].Return(x => x.Robject);
            }

            if(_robjectByAddressCache.ContainsKey(row.Kladr + "#" + row.House))
            {
                return _robjectByAddressCache[row.Kladr + "#" + row.House].Robject;
            }

            return null;
        }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];

            var parsedRows = new List<ParsedRow>();

            // Парсинг сущностей
            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                if(excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                excel.UseVersionXlsx();
                using(var xlsMemoryStream = new MemoryStream(file.Data))
                {
                    excel.Open(xlsMemoryStream);
                    var dict = new Dictionary<string, string>();
                    var data = excel.GetRows(0, 0);

                     var startIndex = 0;
                    String[] header = null;

                    var rowNum = 0;
                    do
                    {
                        var row = data[rowNum].Select(x => x.Value.ToLower()).ToArray();

                        if (row.Contains("index"))
                        {
                            startIndex = rowNum + 1;
                            header = row.Distinct().ToArray();
                        }

                        rowNum++;

                    } while (startIndex == 0 && rowNum + 1 != data.Count);

                    if (startIndex == 0)
                    {
                        throw new ArgumentException("Формат шаблона не соответсвует импорту!");
                    }

                    for(var i = startIndex; i < data.Count; i++)
                    {
                        var row = data[i];
                        dict.Clear();
                        for(var j = 0; j < row.Length && j < header.Length; j++)
                        {
                            if(dict.ContainsKey(header[j]))
                            {
                                continue;
                            }

                            dict.Add(header[j], row[j].Value);
                        }

                        var entity = new ParsedRow
                        {
                            RowNumber = i,
                            Index = dict.Get("index"),
                            Kladr = dict.Get("kladr"),
                            MR = dict.Get("mr"),
                            MO = dict.Get("mo"),
                            City = dict.Get("city"),
                            Street = dict.Get("street"),
                            House = dict.Get("house"),
                            MkdService = dict.Get("mkd_service"),
                            IsInRegProgram = dict.Get("in_reg_program"),
                            MeetingDate = dict.Get("meeting_date"),
                            FIO = dict.Get("fio"),
                            Phone = dict.Get("phone"),
                            KrFondFormation = dict.Get("kr_fond_formation"),
                            Payment = dict.Get("payment"),
                            CreditOrg = dict.Get("credit_org"),
                            MinFond = dict.Get("min_fond"),
                            Income = dict.Get("income"),
                            Status = dict.Get("status"),
                            Additional = dict.Get("additional"),
                            BankName = dict.Get("bank_name"),
                            INN = dict.Get("inn"),
                            KPP = dict.Get("kpp"),
                            KS = dict.Get("ks"),
                            BIK = dict.Get("bik"),
                            Person = dict.Get("person")
                        };

                        if(string.IsNullOrWhiteSpace(entity.Index))
                        {
                            LogImport.Error(
                                "Протокол решения не добавлен",
                                string.Format("Для протокола решения в строке {0} не найден индекс", i + 1));
                            continue;
                        }

                        if(string.IsNullOrWhiteSpace(entity.Kladr))
                        {
                            LogImport.Error(
                                "Протокол решения не добавлен",
                                string.Format("Для протокола решения в строке {0} не указан ни кладр код ни полный адрес", i + 1));
                            continue;
                        }

                        parsedRows.Add(entity);
                    }
                }
            }

            var stateDomain = Container.ResolveDomain<State>();
            var finalState = stateDomain.GetAll().FirstOrDefault(item => item.TypeId == "gkh_real_obj_dec_protocol" && item.FinalState);

            InitCache();

            var indexGroups = parsedRows.GroupBy(item => item.Index).ToList();

            foreach(var partition in Partitioner.Create(0, indexGroups.Count).GetDynamicPartitions())
            {
                var creditOrg = Container.ResolveDomain<CreditOrg>();
                var creditOrgs = creditOrg.GetAll().ToList();

                var tr = Container.Resolve<IDataTransaction>();
                try
                {
                    for(var index = partition.Item1; index < partition.Item2; index++)
                    {
                        var groupItem = indexGroups[index].ToArray();

                        for(var groupIndex = 0; groupIndex < groupItem.Length; groupIndex++)
                        {
                            var entry = parsedRows[index];
                            var robject = TryGetRealityObject(entry);

                            if (robject == null)
                            {
                                LogImport.Error(
                                "Протокол решения не добавлен",
                                string.Format("Для протокола решения в строке {0} не найден дом", entry.RowNumber + 1));
                                continue;
                            }

                            var entity = new UltimateDecisionProxy
                            {
                                Protocol = new RealityObjectDecisionProtocol
                                {
                                    DocumentNum = (groupIndex + 1).ToString(),
                                    ProtocolDate = entry.MeetingDate.ToDateTime(),
                                    AuthorizedPerson = entry.FIO,
                                    PhoneAuthorizedPerson = entry.Phone,
                                    State = finalState,
                                    RealityObject = robject
                                },
                                CrFundFormationDecision = new CrFundFormationDecision
                                {
                                    Decision =  entry.KrFondFormation.Trim().ToLower() == "специальный счет" ? CrFundFormationDecisionType.SpecialAccount : CrFundFormationDecisionType.RegOpAccount,
                                    IsChecked = true
                                }
                            };

                            if(!string.IsNullOrWhiteSpace(entry.Payment))
                            {
                                entity.MonthlyFeeAmountDecision = new MonthlyFeeAmountDecision
                                {
                                    Decision = new List<PeriodMonthlyFee>
                                    {
                                        new PeriodMonthlyFee
                                        {
                                            Value = entry.Payment.ToDecimal()
                                        }
                                    }
                                };
                            }

                            if(!string.IsNullOrWhiteSpace(entry.MinFond))
                            {
                                entity.MinFundAmountDecision = new MinFundAmountDecision
                                {
                                    Decision = entry.MinFond.ToDecimal()
                                };
                            }

                            if(!string.IsNullOrWhiteSpace(entry.Income))
                            {
                                entity.AccumulationTransferDecision = new AccumulationTransferDecision
                                {
                                    Decision = entry.Income.ToDecimal()
                                };
                            }

                            if(!string.IsNullOrWhiteSpace(entry.INN) &&
                               !string.IsNullOrWhiteSpace(entry.BIK))
                            {
                                var org = creditOrgs.FirstOrDefault(item => item.Inn == entry.INN && item.Bik == entry.BIK);
                                if(org == null)
                                {
                                    if(string.IsNullOrWhiteSpace(entry.BankName))
                                    {
                                        LogImport.Error(
                                            "Протокол решения не добавлен",
                                            string.Format("Для протокола решения в строке {0} не указано название банка ", entry.RowNumber + 1));
                                        continue;
                                    }

                                    org = new CreditOrg
                                    {
                                        Address = entry.Person,
                                        Bik = entry.BIK,
                                        CorrAccount = entry.KS,
                                        Inn = entry.INN,
                                        Kpp = entry.KPP,
                                        Name = entry.BankName
                                    };

                                    creditOrg.Save(org);
                                    creditOrgs.Add(org);
                                }

                                entity.CreditOrgDecision = new CreditOrgDecision
                                {
                                    Decision = org
                                };
                            }

                            SaveOwnersDecision(entity);
                            LogImport.CountAddedRows++;
                            LogImport.Info("Добавление", string.Format("Строка {0}. Добавлен протокол решений собственников", index));
                        }
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    LogImport.Error("Ошибка сохранения. ", ex.Message);
                    throw;
                }
                finally
                {
                    Container.Release(tr);
                }
            }

            LogImport.ImportKey = Key;
            LogImportManager.Add(file, LogImport);
            LogImportManager.FileNameWithoutExtention = file.FileName;
            LogImportManager.Save();

            var statusImport = LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : LogImport.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError;

            return new ImportResult(
                statusImport,
                string.Format("Импортировано {0} записей", LogImport.CountAddedRows),
                string.Empty,
                LogImportManager.LogFileId);
        }

        private void SaveOwnersDecision(UltimateDecisionProxy proxy)
        {
            var properties = proxy.GetType().GetProperties();

            dynamic protocol = null;
            var protocolProperty = properties.FirstOrDefault(x => typeof(RealityObjectDecisionProtocol) == x.PropertyType);

            if (protocolProperty != null)
            {
                properties = properties.Except(new[] { protocolProperty }).ToArray();
                protocol = protocolProperty.GetValue(proxy, new object[0]);

                if (protocol != null)
                {
                    var domain = Container.ResolveRepository<RealityObjectDecisionProtocol>();

                    using (Container.Using(domain))
                    {
                        domain.Save(protocol);
                    }
                }
            }

            foreach (var property in properties)
            {
                var value = property.GetValue(proxy, new object[0]);

                if (value == null)
                {
                    continue;
                }

                var protocolProp =
                    value.GetType()
                        .GetProperties()
                        .FirstOrDefault(x => typeof(RealityObjectDecisionProtocol) == x.PropertyType);

                if (protocolProp != null)
                {
                    protocolProp.SetValue(value, protocol, new object[0]);
                }

                if (!typeof(IEntity).IsAssignableFrom(property.PropertyType))
                {
                    continue;
                }

                var domainType = typeof(IDomainService<>).MakeGenericType(property.PropertyType);
                var domain = Container.Resolve(domainType);

                using (Container.Using(domain))
                {
                    var save = domain.GetType().GetMethod("Save", new[] { property.PropertyType });
                    save.Invoke(domain, new[] { value });
                }
            }
        }

        #region cache

        private void InitCache()
        {
            var roRep = Container.ResolveRepository<RealityObject>();

            _robjectByCodeCache = roRep.GetAll()
                .Where(x => x.GkhCode != null)
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.GkhCode))
                .GroupBy(x => x.GkhCode)
                .ToDictionary(x => x.Key, y => new RobjectProxy
                {
                    Count = y.Count(),
                    Robject = y.First()
                });

            var fiasCache = Container.ResolveRepository<Fias>().GetAll()
                .Select(x => new
                {
                    x.AOGuid,
                    x.ShortName,
                    x.FormalName,
                    x.ActStatus,
                    x.KladrCode
                })
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .ToList()
                .GroupBy(x => x.AOGuid)
                .ToDictionary(x => x.Key, y => y.First());

            _robjectByAddressCache = roRep.GetAll()
                .Where(x => x.FiasAddress != null)
                .Select(
                    x => new
                {
                    RoId = x.Id,
                    Mr = x.Municipality.Name.ToLower().Trim(),
                    Mo = x.MoSettlement.Name.ToLower().Trim(),
                    x.FiasAddress.PlaceGuidId,
                    x.FiasAddress.StreetGuidId,
                    House = x.FiasAddress.House.ToLower().Trim()
                })
                .AsEnumerable()
                .Select(
                    x => new
                {
                        Key = (x.StreetGuidId.IsEmpty()
                            ? x.PlaceGuidId.IsEmpty()
                                ? string.Empty
                                : fiasCache.Get(x.PlaceGuidId).Return(y => y.KladrCode)
                            : fiasCache.Get(x.StreetGuidId).Return(y => y.KladrCode)) + "#" + x.House,
                    x.RoId
                })
                .GroupBy(x => x.Key)
                .ToDictionary(
                    x => x.Key,
                    y => new RobjectProxy
                {
                    Count = y.Count(),
                    Robject = new RealityObject { Id = y.First().RoId }
                });
        }

        private Dictionary<string, RobjectProxy> _robjectByCodeCache;

        private Dictionary<string, RobjectProxy> _robjectByAddressCache;

        private string ToLowerTrim(string value)
        {
            return (value ?? "").ToLower().Trim(' ', '.');
        }

        #endregion cache

        /// <summary>
        /// Первоночальная валидация файла перед импортом
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            var fileData = baseParams.Files["FileImport"];
            var extention = fileData.Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        private class RobjectProxy
        {
            public int Count { get; set; }

            public RealityObject Robject { get; set; }
        }
    }
}