namespace Bars.Gkh1468.Domain.PassportImport.Impl.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.Gkh1468.Domain.PassportImport.Interfaces;
    using Bars.Gkh1468.Domain.PassportImport.ProxyEntity;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Entities.Passport;
    using Bars.Gkh1468.Enums;

    using Castle.Windsor;

    using NHibernate;
    using NHibernate.Linq;

    internal class MetaAtrMatch
    {
        public string Code { get; set; }

        public MetaAttribute Meta { get; set; }
    }

    public class ArchiveDataProcessor : IDataProcessor
    {
        private readonly IDynamicDataProvider _provider;
        private readonly IWindsorContainer _container;

        private readonly IDomainService<Contragent> _contragentDomain;
        private readonly IDomainService<RealityObject> _realityObjectDomain;
        private readonly IDomainService<HouseProviderPassport> _passportDomain;
        private readonly IDomainService<PassportStruct> _structDomain;
        private readonly IDomainService<MetaAttribute> _metaDomain;
        private readonly IDomainService<HouseProviderPassportRow> _rowDomain;
        private readonly IStateProvider _stateProvider;
        private readonly IDomainService<Part> _partDomain;

        private readonly ISessionProvider _sessionProvider;
        private IStatelessSession _statelessSession;
        private readonly IUnProxy _unProxy;

        private readonly IHousePassportService _passportService;
        private readonly ILogImport _logger;

        private readonly Dictionary<long, List<MetaAtrMatch>> integrationCompareRegex = new Dictionary<long, List<MetaAtrMatch>>();

        private readonly Dictionary<long, Dictionary<string, MetaAtrMatch>> integrationComparePlain = new Dictionary<long, Dictionary<string, MetaAtrMatch>>();

        private readonly List<long> knownPartIds = new List<long>();

        private long passportStructureId;

        private List<HouseProviderPassportRow> _rows;

        private readonly Regex housePattern = new Regex("(?<guid>.{36})(?<house>he.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private readonly Regex housePartsPattern = new Regex("he(?<house>.*)hg(?<housing>.*)bg(?<building>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private State _defaultPassportState;

        public ArchiveDataProcessor(BaseParams baseParams, IDynamicDataProvider provider, IWindsorContainer container, ILogImport logger)
        {
            ArgumentChecker.NotNull(provider, "[1468] Провайдер данных паспорта пуст!");
            ArgumentChecker.NotNull(container, "[1468] СервисЛокатор пуст!");
            ArgumentChecker.NotNull(logger, "[1468] Логгер пуст!");

            _container = container;
            _provider = provider;
            _contragentDomain = container.Resolve<IDomainService<Contragent>>();
            _realityObjectDomain = container.Resolve<IDomainService<RealityObject>>();
            _passportDomain = container.Resolve<IDomainService<HouseProviderPassport>>();
            _structDomain = container.Resolve<IDomainService<PassportStruct>>();
            _metaDomain = container.Resolve<IDomainService<MetaAttribute>>();
            _rowDomain = container.Resolve<IDomainService<HouseProviderPassportRow>>();
            _stateProvider = container.Resolve<IStateProvider>();
            _partDomain = container.Resolve<IDomainService<Part>>();

            _passportService = container.Resolve<IHousePassportService>();
            _logger = logger;

            _sessionProvider = container.Resolve<ISessionProvider>();
            _unProxy = container.Resolve<IUnProxy>();
        }

        public void ProcessData()
        {
            try
            {
                var userName = _container.Resolve<IGkhUserManager>().GetActiveOperator().Return(x => x.User).Return(x => x.Name ?? x.Login);
                var data = _provider.GetData() as IEnumerable<XmlDocument>;
                var state = _container.ResolveDomain<State>().GetAll().Where(x => x.TypeId == "houseproviderpassport").AsEnumerable().FirstOrDefault(x => x.Code.Trim().ToUpper() == "ПОДПИСАНО");
                if (state == null)
                {
                    var tempPassport = new HouseProviderPassport();
                    _stateProvider.SetDefaultState(tempPassport);
                    state = tempPassport.State;
                }

                _defaultPassportState = UnProxy(state);

                if (data == null)
                {
                    return;
                }

                var addr = string.Empty;
                foreach (var xmlDocument in data)
                {
                    if (xmlDocument == null)
                    {
                        continue;
                    }

                    using (_statelessSession = _sessionProvider.OpenStatelessSession())
                    {
                        using (var tr = _statelessSession.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            try
                            {
                                var passports = ProcessData(xmlDocument);
                                foreach (var passport in passports)
                                {
                                    addr = passport.Passport.RealityObject.Address;
                                    passport.Passport.State = _defaultPassportState;
                                    passport.Passport.UserName = userName;
                                    passport.Passport.SignDate = DateTime.Now;

                                    SaveEntity(passport.Passport);

                                    foreach (var row in passport.Rows)
                                    {
                                        SaveEntity(row);
                                    }

                                    _logger.Info("Загрузка паспорта", "Паспорт {0} загружен.".FormatUsing(addr));
                                    addr = string.Empty;
                                }

                                tr.Commit();
                            }
                            catch (Exception ex)
                            {
                                _logger.Error("Ошибка загрузки паспорта {0}".FormatUsing(addr), ex.Message);
                                tr.Rollback();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(string.Empty, e.Message);
                throw;
            }
        }

        /// <summary>
        /// Здесь идет сохранение паспортов по следующему алгоритму:
        /// 1) Получаем данные из xml
        /// Струтура xml имеет вид:
        /// 
        /// data
        /// - meta_data
        /// -- inn
        /// -- kpp
        /// -- month
        /// -- year
        /// -- adr_code
        /// - &gt;Тип дома как узел с данными&lt;
        /// -- &gt;Код атрибута 1&lt;
        /// -- &gt;Код атрибута 2&lt;
        /// --- &gt;Код атрибута 2.1&lt;
        /// 
        /// 2) По мета информации мы можем получить контрагента, дом, паспорт дома поставщика.
        /// 3) Если паспорта поставщика нет - создаем.
        /// 4) По типу дома получаем струтуру паспорта
        /// 5) Получаем атрибуты структуры и возможно уже существующие значения в паспорте дома
        /// 6) В атрибуте содержатся коды интеграции, разделенные "|". Для них строим xpath вида "//код_интеграции"
        /// 7) Для каждого xpath атрибута ищем соответсвующий узел и полученное значение сохраняем 
        /// </summary>
        private IEnumerable<PassportWithAttributes> ProcessData(XmlDocument doc)
        {
            var meta = GetMetaTag(doc);
            var data = GetValuableDataTag(doc);

            if (meta == null)
            {
                _logger.Error(string.Empty, "Не обнаружен тег с метаданными (meta_data)");
                yield break;
            }

            if (data == null)
            {
                _logger.Error(string.Empty, "Не обнаружен тег паспортными данными (C_ЭП_ОКИ|C_ЭП_ЖД|C_ЭП_МКД)");
                yield break;
            }

            var year = meta.SelectSingleNode("year").Return(x => x.InnerText).ToInt();
            var month = meta.SelectSingleNode("month").Return(x => x.InnerText).ToInt();

            Contragent contragent = GetContragent(meta);

            if (contragent == null)
            {
                var inn = meta.SelectSingleNode("inn").Return(x => x.InnerText);
                var kpp = meta.SelectSingleNode("kpp").Return(x => x.InnerText);
                _logger.Error(string.Empty, string.Format("Не найден контрагент по ИНН: {0} и КПП: {1}", inn, kpp));
                yield break;
            }

            var ros = GetRealityObjectList(meta);

            if (ros.Length == 0)
            {
                var addrCode = meta.SelectSingleNode("adr_code").Return(y => y.InnerText);
                _logger.Error(string.Empty, string.Format("Не найден объект недвижимости по коду: {0}", addrCode));
                yield break;
            }

            foreach (var ro in ros)
            {
                var passport = GetPassport(contragent, ro, year, month);

                passportStructureId = passport != null ? passport.PassportStruct.Id : 0;

                if (passport == null)
                {
                    var isMkd = ro.TypeHouse.To1468RealObjType() == TypeRealObj.Mkd;

                    var str = isMkd
                                  ? _structDomain.GetAll()
                                                 .OrderByDescending(x => x.ValidFromYear)
                                                 .ThenByDescending(x => x.ValidFromMonth)
                                                 .Where(
                                                     x =>
                                                     (x.ValidFromYear < year) ||
                                                     (x.ValidFromYear == year && x.ValidFromMonth <= month))
                                                 .FirstOrDefault(x => x.PassportType == PassportType.Mkd)
                                  : _structDomain.GetAll()
                                                 .OrderByDescending(x => x.ValidFromYear)
                                                 .ThenByDescending(x => x.ValidFromMonth)
                                                 .Where(
                                                     x =>
                                                     (x.ValidFromYear < year) ||
                                                     (x.ValidFromYear == year && x.ValidFromMonth <= month))
                                                 .FirstOrDefault(x => x.PassportType == PassportType.House);

                    if (str == null)
                    {
                        _logger.Error(string.Empty,
                            string.Format("Не удалось найти структуру паспорта для дома типа: {0}", isMkd ? "МКД" : "ЖД"));
                        continue;
                    }

                    passportStructureId = str.Id;

                    passport = new HouseProviderPassport
                    {
                        HouseType = isMkd ? HouseType.Mkd : HouseType.House,
                        RealityObject = ro,
                        ReportMonth = month,
                        ReportYear = year,
                        ContragentType = GetProviderType(data),
                        Contragent = contragent,
                        PassportStruct = str,
                        HousePassport = UnProxy(_passportService.GetPassport(ro, year, month).Data as HousePassport),
                        State = _defaultPassportState
                    };

                    SaveEntity(passport);
                }
                else
                {
                    _rowDomain.GetAll()
                              .Where(x => x.ProviderPassport.Id == passport.Id)
                              .ForEach(x => _statelessSession.Delete(x));
                }

                yield return CollectPassportAttributes(passport, data, _logger);
            }
        }

        #region helpers

        private void SaveEntity<T>(T entity) where T : BaseEntity
        {
            if (entity == null)
            {
                return;
            }

            var obj = UnProxy(entity);
            obj.ObjectEditDate = DateTime.Now;

            if (obj.Id == 0)
            {
                obj.ObjectCreateDate = DateTime.Now;
                entity.Id = (long)_statelessSession.Insert(obj);
            }
            else
            {
                obj.ObjectVersion += 1;
                _statelessSession.Update(obj);
            }
        }

        private T UnProxy<T>(T entity) where T : BaseEntity
        {
            if (entity == null)
            {
                return null;
            }

            return (T)_unProxy.GetUnProxyObject(entity);
        }

        private PassportWithAttributes CollectPassportAttributes(
            HouseProviderPassport passport,
            XmlNode data,
            ILogImport logger)
        {
            var result = new PassportWithAttributes();

            _rows = new List<HouseProviderPassportRow>();

            var parts = _partDomain.GetAll().Where(x => x.Struct.Id == passportStructureId || x.Parent.Struct.Id == passportStructureId).ToArray();

            FillMetaCompareDict(parts);

            foreach (XmlNode node in data.ChildNodes)
            {
                var part = parts.FirstOrDefault(x => x.IntegrationCode == node.Name);
                if (part == null)
                {
                    logger.Error("Обработка дочерних элементов XML документа по тегу {0}".FormatUsing(data.Name),
                            string.Format("Для тега {0} не удалось найти ни одно раздела соответсвуюещго коду интеграции.", node.Name));

                    continue;
                }

                HandlePartNode(part, passport, node, logger);
            }

            result.Rows = _rows;
            result.Passport = passport;

            return result;
        }

        private void FillMetaCompareDict(Part[] parts)
        {
            var newParts = parts.Where(x => !knownPartIds.Contains(x.Id)).ToArray();
            if (newParts.Length == 0)
            {
                return;
            }

            if (!integrationComparePlain.ContainsKey(passportStructureId))
            {
                integrationComparePlain[passportStructureId] = new Dictionary<string, MetaAtrMatch>();
            }

            if (!integrationCompareRegex.ContainsKey(passportStructureId))
            {
                integrationCompareRegex[passportStructureId] = new List<MetaAtrMatch>();
            }

            var metaList = _metaDomain.GetAll().ToArray();

            foreach (var part in newParts)
            {
                foreach (var meta in GetChildrenMeta(part, null, metaList))
                {
                    var match = new MetaAtrMatch { Code = string.Format("{0}$", meta.IntegrationCode.Replace(".", "\\.").Replace("X", "[0-9]+")), Meta = meta };

                    // Данное разделение сделано для оптимизации:
                    // Regex.IsMatch (см. строку присваивания matchedAtr ниже) занимает очень много времени,
                    // но в большинстве случаев это не нужно. Элементы с простыми кодами будут сравниваться напрямую.
                    if (match.Meta.IntegrationCode.Contains("X"))
                    {
                        integrationCompareRegex[passportStructureId].Add(match);
                    }
                    else
                    {
                        if (!integrationComparePlain[passportStructureId].ContainsKey(match.Meta.IntegrationCode))
                        {
                            integrationComparePlain[passportStructureId][match.Meta.IntegrationCode] = match;
                        }
                    }
                }

                knownPartIds.Add(part.Id);
            }
        }

        private IEnumerable<MetaAttribute> GetChildrenMeta(Part part, MetaAttribute root, MetaAttribute[] listAttributes)
        {
            var result = new List<MetaAttribute>();

            var query = listAttributes
                .WhereIf(part != null, x => x.ParentPart != null && x.ParentPart.Id == part.Id)
                .WhereIf(root != null, x => x.Parent != null && x.Parent.Id == root.Id)
                .ToArray();
            result.AddRange(query.Select(UnProxy));

            foreach (var meta in query)
            {
                result.AddRange(GetChildrenMeta(null, meta, listAttributes));
            }

            return result;
        }

        private void HandlePartNode(Part part, HouseProviderPassport passport, XmlNode parentNode, ILogImport logger)
        {
            int rowNum = 0;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                XmlNode node1 = node;
                var matchedAtr = integrationComparePlain[passportStructureId].Get(node1.Name) ?? integrationCompareRegex[passportStructureId].FirstOrDefault(x => Regex.IsMatch(node1.Name, x.Code));

                if (matchedAtr == null)
                {
                    logger.Error("Обработка дочерних элементов XML документа по тегу {0}".FormatUsing(parentNode.Name),
                        String.Format("Для тега {0} не удалось найти ни одно соответсвующего элемента из раздела {1}",
                        node.Name, part.Name));

                    continue;
                }

                rowNum++;
                switch (matchedAtr.Meta.Type)
                {
                    case MetaAttributeType.Simple:
                        HandleSimple(passport, matchedAtr.Meta, node, logger, null);
                        break;
                    case MetaAttributeType.Grouped:
                        HandleGrouped(passport, matchedAtr.Meta, node, logger, null);
                        break;

                    case MetaAttributeType.GroupedComplex:
                        HandleMulty(passport, matchedAtr.Meta, node, logger, null);
                        break;
                    case MetaAttributeType.GroupedWithValue:
                        HandleGroupedWhithValue(passport, matchedAtr.Meta, node, logger, null);
                        HandleGrouped(passport, matchedAtr.Meta, node, logger, null);
                        break;
                }
            }
        }

        private void HandleMulty(HouseProviderPassport passport, MetaAttribute parentMeta, XmlNode parentNode, ILogImport logger, HouseProviderPassportRow parent)
        {
            var rowNum = 0;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                rowNum++;
                var rowNum2 = 0;
                var parentForNode = new HouseProviderPassportRow
                {
                    Passport = passport,
                    MetaAttribute = parentMeta,
                    ParentValue = parent.Return(x => x.Id),
                    Value = rowNum.ToStr()
                };

                SaveEntity(parentForNode);

                foreach (XmlNode rowNode in node.ChildNodes)
                {
                    var matchedAtr = integrationComparePlain[passportStructureId].Get(rowNode.Name) ??
                                              integrationCompareRegex[passportStructureId].FirstOrDefault(x => Regex.IsMatch(rowNode.Name, x.Code));

                    if (matchedAtr == null)
                    {
                        if (rowNode.Name.ToLower().Contains("text"))
                        {
                            logger.Error(
                                "Тег {0} является групповым-множественным, но структура не соответсвует типу аттрибута. Значение тега '{1}'."
                                    .FormatUsing(node.Name, rowNode.Value),
                                parentMeta.Name);
                        }
                        else
                        {
                            logger.Error("Обработка дочерних элементов XML документа по тегу {0}".FormatUsing(rowNode.Name),
                            String.Format("Для тега {0} не удалось найти ни одно соответсвующего элемента по коду интеграции из аттрибутов группы {1}.\"{2}\"",
                            rowNode.Name, parentMeta.Code, parentMeta.Name));
                        }
                        continue;
                    }
                    //var newGroupKey = string.Format("{0}.{1}", groupKey, rowNum);
                    rowNum2++;

                    switch (matchedAtr.Meta.Type)
                    {
                        case MetaAttributeType.Simple:
                            HandleSimple(passport, matchedAtr.Meta, rowNode, logger, parentForNode);
                            break;

                        case MetaAttributeType.Grouped:
                            HandleGrouped(passport, matchedAtr.Meta, rowNode, logger, parentForNode);
                            break;

                        case MetaAttributeType.GroupedComplex:
                            HandleMulty(passport, matchedAtr.Meta, rowNode, logger, parentForNode);
                            break;

                        case MetaAttributeType.GroupedWithValue:
                            HandleGroupedWhithValue(passport, matchedAtr.Meta, rowNode, logger, parentForNode);
                            HandleGrouped(passport, matchedAtr.Meta, rowNode, logger, parentForNode);
                            break;
                    }
                }
            }
        }

        private void HandleGrouped(HouseProviderPassport passport, MetaAttribute parentMeta, XmlNode parentNode, ILogImport logger, HouseProviderPassportRow parent)
        {
            foreach (XmlNode rowNode in parentNode.ChildNodes)
            {
                var matchedAtr = integrationComparePlain[passportStructureId].Get(rowNode.Name) ?? integrationCompareRegex[passportStructureId].FirstOrDefault(x => Regex.IsMatch(rowNode.Name, x.Code));

                if (matchedAtr == null)
                {
                    logger.Error("Обработка дочерних элементов XML документа по тегу {0}".FormatUsing(rowNode.Name),
                        String.Format("Для тега {0} не удалось найти ни одно соответсвующего элемента по коду интеграции из аттрибутов группы {1}.\"{2}\"",
                        rowNode.Name, parentMeta.Code, parentMeta.Name));

                    continue;
                }

                switch (matchedAtr.Meta.Type)
                {
                    case MetaAttributeType.Simple:
                        HandleSimple(passport, matchedAtr.Meta, rowNode, logger, parent);
                        break;

                    case MetaAttributeType.Grouped:
                        HandleGrouped(passport, matchedAtr.Meta, rowNode, logger, parent);
                        break;
                    case MetaAttributeType.GroupedComplex:
                        HandleMulty(passport, matchedAtr.Meta, rowNode, logger, parent);
                        break;

                    case MetaAttributeType.GroupedWithValue:
                        HandleGroupedWhithValue(passport, matchedAtr.Meta, rowNode, logger, parent);
                        HandleGrouped(passport, matchedAtr.Meta, rowNode, logger, parent);
                        break;
                }
            }
        }

        private void HandleGroupedWhithValue(HouseProviderPassport passport, MetaAttribute meta, XmlNode node, ILogImport logger, HouseProviderPassportRow parent, int i = 0)
        {
            var valueToSave = node.Return(x => x.Attributes).Return(x => x["Value"]).Return(x => x.Value).Return(x => x, string.Empty);
            AddRowToSave(passport, meta, valueToSave, parent, i);
        }

        private void HandleSimple(HouseProviderPassport passport, MetaAttribute meta, XmlNode node, ILogImport logger, HouseProviderPassportRow parent, int i = 0)
        {
            var valueToSave = node.Return(x => x.InnerText);
            AddRowToSave(passport, meta, valueToSave, parent, i);
        }

        private void AddRowToSave(HouseProviderPassport passport, MetaAttribute metaAttribute, string valueToSave,
                                  HouseProviderPassportRow parent, int i = 0)
        {
            _rows.Add(new HouseProviderPassportRow
            {
                MetaAttribute = metaAttribute,
                Value = valueToSave,
                Passport = passport,
                ParentValue = parent.Return(x => x.Id),
                GroupKey = i
            });
        }

        private ContragentType GetProviderType(XmlNode node)
        {
            return ContragentType.NotSet;
        }

        private HouseProviderPassport GetPassport(Contragent contragent, RealityObject ro, int year, int month)
        {
            return
                UnProxy(
                    _passportDomain.GetAll()
                        .Fetch(x => x.RealityObject)
                        .Fetch(x => x.State)
                        .FirstOrDefault(x => x.Contragent.Id == contragent.Id && x.RealityObject.Id == ro.Id && x.ReportMonth == month && x.ReportYear == year));
        }

        private RealityObject[] GetRealityObjectList(XmlNode node)
        {
            var guidWithParams = node.SelectSingleNode("adr_code").Return(y => y.InnerText);

            if (guidWithParams.IsEmpty())
            {
                return new RealityObject[0];
            }

            var match = housePattern.Match(guidWithParams);
            if (!match.Success)
            {
                return new RealityObject[0];
            }

            var fiasGuid = match.Groups["guid"].Value;
            var houseParts = match.Groups["house"].Value;

            string house = string.Empty, housing = string.Empty, building = string.Empty;

            var houseMatch = housePartsPattern.Match(houseParts);
            if (houseMatch.Success)
            {
                house = houseMatch.Groups["house"].Value;
                housing = houseMatch.Groups["housing"].Value;
                building = houseMatch.Groups["building"].Value;
            }

            return _realityObjectDomain.GetAll()
                .Where(x => x.FiasAddress.StreetGuidId == fiasGuid)
                .WhereIf(!house.IsEmpty(), x => x.FiasAddress.House == house)
                .WhereIf(!housing.IsEmpty(), x => x.FiasAddress.Housing == housing)
                .WhereIf(!building.IsEmpty(), x => x.FiasAddress.Building == building)
                .ToArray();
        }

        private Contragent GetContragent(XmlNode meta)
        {
            var inn = meta.SelectSingleNode("inn").Return(x => x.InnerText);
            var kpp = meta.SelectSingleNode("kpp").Return(x => x.InnerText);

            if (inn.IsEmpty() && kpp.IsEmpty())
            {
                return null;
            }

            return _contragentDomain.GetAll().FirstOrDefault(x => x.Inn == inn && x.Kpp == kpp);
        }

        private XmlNode GetValuableDataTag(XmlDocument doc)
        {
            var xpaths = new[]
            {
                "//C_ЭП_ОКИ",
                "//C_ЭП_ЖД",
                "//C_ЭП_МКД"
            };

            foreach (var xpath in xpaths)
            {
                var data = GetTagByXpath(doc, xpath);
                if (data != null)
                {
                    return data;
                }
            }

            return null;
        }

        private XmlNode GetMetaTag(XmlDocument data)
        {
            return GetTagByXpath(data, "//meta_data");
        }

        private XmlNode GetTagByXpath(XmlDocument doc, string xpath)
        {
            return doc.SelectSingleNode(xpath);
        }

        #endregion helpers
    }
}