﻿namespace Bars.Gkh1468.DomainService.Impl
﻿{
﻿    using Bars.B4;
﻿    using Bars.B4.Utils;
﻿    using Bars.Gkh.Entities;
﻿    using Bars.Gkh.Enums;
﻿    using Bars.Gkh.Utils;
﻿    using Bars.Gkh1468.DomainService;
﻿    using Bars.Gkh1468.Entities;
﻿    using Bars.Gkh1468.Entities.Passport;
﻿    using Bars.Gkh1468.Enums;
﻿    using Castle.Windsor;
﻿    using System.Collections.Generic;
﻿    using System.Linq;
﻿    using ValueType = Bars.Gkh1468.Enums.ValueType;

﻿    /// <summary>
﻿    /// Сервис по формированию сводного паспорта
﻿    /// </summary>
﻿    public class HousePassportCombinedService : IHousePassportCombinedService
﻿    {
﻿        /// <summary>
﻿        /// Список идентификаторов атрибутов добавленных в сводый паспорт. Чтобы не добавлялись повторно.
﻿        /// </summary>
﻿        private readonly List<long> addedMetaAttributeIds = new List<long>();

﻿        /// <summary>
﻿        /// IoC контейнер
﻿        /// </summary>
﻿        public IWindsorContainer Container { get; set; }

﻿        /// <summary>
﻿        /// Список данных провайдеров паспортов по атрибутам
﻿        /// </summary>
﻿        private List<ProxyPassportProviderRow> PassportRowData { get; set; }

﻿        /// <summary>
﻿        /// Возвращает список строк в сводном паспорте
﻿        /// </summary>
﻿        /// <param name="baseParams">Базовые параметры запроса</param>
﻿        /// <returns>Результат выполнения запроса</returns>
﻿        public IDataResult GetList(BaseParams baseParams)
﻿        {
﻿            var loadParam = baseParams.GetLoadParam();
﻿            var passportId = baseParams.Params.GetAs<long>("passport");
﻿            var stateId = baseParams.Params.GetAs<long>("stateId");

﻿            var providerPassport = this.Container.Resolve<IDomainService<HouseProviderPassport>>().GetAll()
﻿                .Where(x => x.HousePassport.Id == passportId)
﻿                .WhereIf(stateId != 0, x => x.State.Id == stateId)
﻿                .Filter(loadParam, this.Container);

﻿            var data = providerPassport.Select(x => new {x.Id, paspStructId = (long?) x.PassportStruct.Id}).ToList();

﻿            var provPasppIds = data.Select(x => x.Id).ToList();

﻿            var list = new List<CombinedPassport>();
﻿            this.addedMetaAttributeIds.Clear();

﻿            var paspStructId = data.Select(x => x.paspStructId).FirstOrDefault();

﻿            if (paspStructId == null)
﻿            {
﻿                var housePassport = this.Container.Resolve<IDomainService<HousePassport>>()
﻿                    .GetAll()
﻿                    .FirstOrDefault(x => x.Id == passportId);

﻿                if (housePassport == null)
﻿                {
﻿                    list.Add(new CombinedPassport {Name = "Нет паспорта!"});
﻿                    return new ListDataResult(list, list.Count);
﻿                }

﻿                paspStructId = this.Container.Resolve<IDomainService<PassportStruct>>()
﻿                    .GetAll()
﻿                    .Where(
﻿                        x =>
﻿                            ((housePassport.ReportYear - x.ValidFromYear)*12) + housePassport.ReportMonth -
﻿                            x.ValidFromMonth >= 0)
﻿                    .Where(x => x.PassportType != PassportType.Nets)
﻿                    .WhereIf(housePassport.RealityObject.TypeHouse == TypeHouse.ManyApartments,
﻿                        x => x.PassportType == PassportType.Mkd)
﻿                    .WhereIf(housePassport.RealityObject.TypeHouse != TypeHouse.ManyApartments,
﻿                        x => x.PassportType == PassportType.House)
﻿                    .OrderByDescending(x => x.ValidFromYear)
﻿                    .ThenByDescending(x => x.ValidFromMonth)
﻿                    .Select(x => (long?) x.Id)
﻿                    .FirstOrDefault();
﻿            }

﻿            if (paspStructId == null)
﻿            {
﻿                list.Add(new CombinedPassport {Name = "Нет структуры паспорта на отчетный период!"});
﻿                return new ListDataResult(list, list.Count);
﻿            }

﻿            var partsService = this.Container.Resolve<IDomainService<Part>>();

﻿            var partIdsQuery = partsService.GetAll()
﻿                .Where(x => x.Struct.Id == paspStructId)
﻿                .Where(x => x.Parent == null)
﻿                .Select(x => x.Id);

﻿            var subPartIdsQuery = partsService.GetAll()
﻿                .Where(x => partIdsQuery.Contains(x.Parent.Id))
﻿                .Select(x => x.Id);

﻿            var partAttributesDict = this.GetMetaAttributesDict(partIdsQuery);
﻿            var subPartAttributesDict = this.GetMetaAttributesDict(subPartIdsQuery);

﻿            var subPartsDict = partsService.GetAll()
﻿                .Where(x => partIdsQuery.Contains(x.Parent.Id))
﻿                .Select(x => new
﻿                {
﻿                    x.Id,
﻿                    x.Name,
﻿                    x.Code,
﻿                    ParentId = x.Parent.Id
﻿                })
﻿                .OrderBy(x => x.Code)
﻿                .AsEnumerable()
﻿                .GroupBy(x => x.ParentId)
﻿                .ToDictionary(
﻿                    x => x.Key,
﻿                    x => x.Select(y => new
﻿                    {
﻿                        Code = y.Code.ToStr(),
﻿                        y.Name,
﻿                        MetaAttributes =
﻿                            subPartAttributesDict.ContainsKey(y.Id)
﻿                                ? subPartAttributesDict[y.Id]
﻿                                : new List<ProxyMetaAttribute>(),
﻿                    })
﻿                        .ToList());

﻿            var parts = partsService.GetAll()
﻿                .Where(x => x.Struct.Id == paspStructId)
﻿                .Where(x => x.Parent == null)
﻿                .OrderBy(x => x.Code)
﻿                .Select(x => new
﻿                {
﻿                    x.Id,
﻿                    x.Code,
﻿                    x.Name
﻿                })
﻿                .AsEnumerable()
﻿                .Select(x => new
﻿                {
﻿                    Code = x.Code.ToStr(),
﻿                    x.Name,
﻿                    MetaAttributes =
﻿                        partAttributesDict.ContainsKey(x.Id)
﻿                            ? partAttributesDict[x.Id]
﻿                            : new List<ProxyMetaAttribute>(),
﻿                    SubParts = subPartsDict.ContainsKey(x.Id) ? subPartsDict[x.Id] : null
﻿                });

﻿            foreach (var part in parts)
﻿            {
﻿                // Получаем данные для раздела
﻿                this.InitDict(part.MetaAttributes, provPasppIds);

﻿                list.Add(new CombinedPassport {PpNumber = part.Code, Name = part.Name});

﻿                var partRows = AddMetaAttributes(part.MetaAttributes);

﻿                list.AddRange(partRows.OrderBy(x => x.PpNumber, new CodeComparer()));

﻿                if (part.SubParts != null)
﻿                {
﻿                    foreach (var subPart in part.SubParts)
﻿                    {
﻿                        // Получаем данные для подраздела
﻿                        this.InitDict(subPart.MetaAttributes, provPasppIds);

﻿                        list.Add(new CombinedPassport {PpNumber = subPart.Code, Name = subPart.Name});
﻿                        list.AddRange(this.AddMetaAttributes(subPart.MetaAttributes));
﻿                    }
﻿                }
﻿            }

﻿            return new ListDataResult(list, list.Count);
﻿        }

﻿        /// <summary>
﻿        /// Получает словарь метаатрибутов сгруппированных по разделам
﻿        /// </summary>
﻿        /// <param name="partIdsQuery">Идентификаторы разделов</param>
﻿        /// <returns>Словарь метаатрибутов</returns>
﻿        private Dictionary<long, List<ProxyMetaAttribute>> GetMetaAttributesDict(IQueryable<long> partIdsQuery)
﻿        {
﻿            var metaAttributeService = this.Container.Resolve<IDomainService<MetaAttribute>>();

﻿            try
﻿            {
﻿                var mt = metaAttributeService.GetAll()
﻿                    .Where(x => partIdsQuery.Contains(x.ParentPart.Id))
﻿                    .OrderBy(x => x.Code)
﻿                    .Select(x => new ProxyMetaAttribute
﻿                    {
﻿                        Id = x.Id,
﻿                        Code = x.Code,
﻿                        Name = x.Name,
﻿                        ParentPartId = x.ParentPart.Id,
﻿                        ParentId = x.Parent.Id,
﻿                        Type = x.Type,
﻿                        OrderNum = x.OrderNum
﻿                    })
﻿                    .AsEnumerable()
﻿                    .GroupBy(x => x.ParentPartId)
﻿                    .ToDictionary(
﻿                        x => x.Key,
﻿                        x => x.ToList());

﻿                return mt;
﻿            }
﻿            finally
﻿            {
﻿                this.Container.Release(metaAttributeService);
﻿            }
﻿        }

﻿        /// <summary>
﻿        /// Добавить данные по метаатрибутам в список данных по сводному паспорту
﻿        /// </summary>
﻿        /// <param name="attributes">Метаатрибуты</param>
﻿        /// <returns>Список данных, которые добавляются в сводный паспорт</returns>
﻿        private IEnumerable<CombinedPassport> AddMetaAttributes(List<ProxyMetaAttribute> attributes)
﻿        {
﻿            var list = new List<CombinedPassport>();

﻿            var sortedAttributes = attributes
﻿                .Where(x =>
﻿                    x.Code != null /* проверяем Code на null, чтобы в OrderBy не возник ArgumentNullException */
﻿                    && x.ParentId == null)
﻿                // добавить только корневые элементы (дочерние будут добавлены вместе со своими групповыми)
﻿                .OrderBy(x => x.Code, new CodeComparer()).ThenBy(x => x.OrderNum);

﻿            foreach (var attr in sortedAttributes)
﻿            {
﻿                if (!this.addedMetaAttributeIds.Contains(attr.Id))
﻿                {
﻿                    this.AddMetaAttribute(attributes, attr, list);
﻿                }
﻿            }

﻿            return list;
﻿        }

﻿        /// <summary>
﻿        /// Добавить данные по текущему метаатрибуту в список данных по сводному паспорту
﻿        /// </summary>
﻿        /// <param name="attributes">Все метаатрибуты</param>
﻿        /// <param name="attr">Атрибут для добавления</param>
﻿        /// <param name="list">Итоговый плоский список упорядоченных атрибутов, который добавляются в сводный паспорт</param>
﻿        /// <param name="passportProviderComplexRows">Групповые-множественные родительские строки (по одной на каждый паспорт)</param>
﻿        private void AddMetaAttribute(
﻿            List<ProxyMetaAttribute> attributes,
﻿            ProxyMetaAttribute attr,
﻿            List<CombinedPassport> list,
﻿            List<ProxyPassportProviderRow> passportProviderComplexRows = null)
﻿        {
﻿            passportProviderComplexRows = passportProviderComplexRows ?? new List<ProxyPassportProviderRow>();

﻿            // Если атрибут групповой-множественный
﻿            if (attr.Type == MetaAttributeType.GroupedComplex)
﻿            {
﻿                // Добавляем данные по нему обычным способом
﻿                var passportRows = PassportRowData
﻿                    .Where(x => x.MetaAttributeId == attr.Id)
﻿                    .WhereIf(passportProviderComplexRows.Count > 0,
﻿                        x => passportProviderComplexRows.Select(y => y.Id).Contains(x.ParentValue))
﻿                    .GroupBy(x => x.Value ?? string.Empty)
﻿                    .ToDictionary(x => x.Key, x => x.ToList());

﻿                foreach (var complexRow in passportRows.OrderBy(x => x.Key))
﻿                {
﻿                    // Получаем дочерние метаатрибуты и формируем список строк для сводного паспорта
﻿                    var childrenAttributes =
﻿                        attributes.Where(x => x.ParentId == attr.Id).OrderBy(x => x.OrderNum).ToList();

﻿                    var childrenList = new List<CombinedPassport>();
﻿                    foreach (var childAttr in childrenAttributes)
﻿                    {
﻿                        this.AddMetaAttribute(attributes, childAttr, childrenList, complexRow.Value);
﻿                    }

﻿                    list.AddRange(childrenList.OrderBy(x => x.ParentValue));
﻿                }
﻿            }
﻿            else if (attr.Type == MetaAttributeType.GroupedWithValue || attr.Type == MetaAttributeType.Grouped)
﻿            {
﻿                //// Если атрибут Групповой - добавляем рекурсивно вслед за ним все его дочерние атрибуты

﻿                this.AddCombinedRowsList(attr, list, passportProviderComplexRows);

﻿                var childrenAttributes = attributes.Where(x => x.ParentId == attr.Id).OrderBy(x => x.OrderNum).ToList();

﻿                foreach (var childAttr in childrenAttributes)
﻿                {
﻿                    this.AddMetaAttribute(attributes, childAttr, list, passportProviderComplexRows);
﻿                }
﻿            }
﻿            else
﻿            {
﻿                //// добавить простой элемент

﻿                this.AddCombinedRowsList(attr, list, passportProviderComplexRows);
﻿            }
﻿        }

﻿        /// <summary>
﻿        /// Добавить строки для сводного паспорта по атрибуту attr
﻿        /// </summary>
﻿        /// <param name="attr">Атрибут для добавления</param>
﻿        /// <param name="list">Итоговый плоский список упорядоченных атрибутов</param>
﻿        /// <param name="passportProviderComplexRows">Групповые-множественные родительские строки (по одной на каждый паспорт)</param>
﻿        private void AddCombinedRowsList(ProxyMetaAttribute attr, List<CombinedPassport> list,
﻿            List<ProxyPassportProviderRow> passportProviderComplexRows)
﻿        {
﻿            passportProviderComplexRows = passportProviderComplexRows ?? new List<ProxyPassportProviderRow>();

﻿            var passportRows = this.PassportRowData
﻿                .Where(x => x.MetaAttributeId == attr.Id)
﻿                .WhereIf(passportProviderComplexRows.Count > 0,
﻿                    x => passportProviderComplexRows.Select(y => y.Id).Contains(x.ParentValue))
﻿                .ToList();

﻿            list.AddRange(this.AddCombinedRows(attr, passportRows));
﻿        }

﻿        /// <summary>
﻿        /// Сформировать строки для сводного паспорта
﻿        /// </summary>
﻿        /// <param name="attr">Атрибут</param>
﻿        /// <param name="passportProviderRows">Данные по провайдерам паспортов</param>
﻿        /// <returns>Список строк для сводного паспорта</returns>
﻿        private IEnumerable<CombinedPassport> AddCombinedRows(ProxyMetaAttribute attr,
﻿            List<ProxyPassportProviderRow> passportProviderRows)
﻿        {
﻿            var list = new List<CombinedPassport>();

﻿            var count = passportProviderRows.Count();
﻿            if (count == 0)
﻿            {
﻿                list.Add(new CombinedPassport {PpNumber = attr.Code, Name = attr.Name});
﻿            }
﻿            else if (count == 1)
﻿            {
﻿                var val = passportProviderRows.FirstOrDefault();
﻿                list.Add(new CombinedPassport
﻿                {
﻿                    PpNumber = attr.Code,
﻿                    Name = attr.Name,
﻿                    Value = val.Return(r => r.Value),
﻿                    ParentValue = val.Return(r => r.ParentValue)
﻿                });
﻿            }
﻿            else
﻿            {
﻿                // Группируем по значению. 
﻿                var valGroups = passportProviderRows.GroupBy(x => x.Value).ToList();

﻿                // Достаем список всех контрагентов для атрибута
﻿                var contragents = passportProviderRows.Select(x => x.ContragentName).Distinct().ToList();

﻿                foreach (var valgroup in valGroups)
﻿                {
﻿                    // Контрагенты установившее конкретное значение для атрибута
﻿                    var valContragents = valgroup.Select(x => x.ContragentName).Distinct().ToList();

﻿                    // Если все конрагенты установили это значение, то не выделяем его
﻿                    if (contragents.All(valContragents.Contains))
﻿                    {
﻿                        list.Add(new CombinedPassport
﻿                        {
﻿                            PpNumber = attr.Code,
﻿                            Name = attr.Name,
﻿                            Value = valgroup.First().Value,
﻿                            InfoSupplier = contragents.Any()
﻿                                ? contragents.Count() > 1
﻿                                    ? string.Format("Значение из паспортов контрагентов {0}",
﻿                                        contragents.AggregateWithSeparator(", "))
﻿                                    : string.Format("Значение из паспорта контрагента {0}",
﻿                                        contragents.FirstOrDefault())
﻿                                : string.Empty,
﻿                            IsMultiple = false,
﻿                            ParentValue = valgroup.First().ParentValue
﻿                        });
﻿                    }
﻿                    else
﻿                    {
﻿                        foreach (var value in valgroup)
﻿                        {
﻿                            list.Add(new CombinedPassport
﻿                            {
﻿                                PpNumber = attr.Code,
﻿                                Name = attr.Name,
﻿                                Value = value.Value,
﻿                                InfoSupplier =
﻿                                    string.Format("Значение из паспорта контрагента {0}", value.ContragentName),
﻿                                IsMultiple = true,
﻿                                ParentValue = value.ParentValue
﻿                            });
﻿                        }
﻿                    }
﻿                }
﻿            }

﻿            this.addedMetaAttributeIds.Add(attr.Id);

﻿            return list;
﻿        }

﻿        /// <summary>
﻿        /// Инициализирует данные для списка атрибутов.
﻿        /// </summary>
﻿        /// <param name="attributes">Список метаатрибутов</param>
﻿        /// <param name="provPasppIds">Идентификаторы провайдеров паспортов</param>
﻿        private void InitDict(IEnumerable<ProxyMetaAttribute> attributes, List<long> provPasppIds)
﻿        {
﻿            var houseProviderPassportRowService = this.Container.Resolve<IDomainService<HouseProviderPassportRow>>();

﻿            this.PassportRowData = new List<ProxyPassportProviderRow>();

﻿            var metaattributeIdsQuery = attributes.Select(x => x.Id);

﻿            this.PassportRowData = houseProviderPassportRowService.GetAll()
﻿                .Where(x => metaattributeIdsQuery.Contains(x.MetaAttribute.Id))
﻿                .Where(x => provPasppIds.Contains(x.ProviderPassport.Id))
﻿                .Select(x => new ProxyPassportProviderRow
﻿                {
﻿                    Id = x.Id,
﻿                    MetaAttributeId = x.MetaAttribute.Id,
﻿                    Code = x.MetaAttribute.Code,
﻿                    Value = x.Value,
﻿                    Exp = x.MetaAttribute.Exp,
﻿                    ContragentId = (long?) x.ProviderPassport.Contragent.Id,
﻿                    ContragentName = x.ProviderPassport.Contragent.Name,
﻿                    ParentValue = x.ParentValue ?? 0
﻿                })
﻿                .ToList();

﻿            this.Container.Release(houseProviderPassportRowService);
﻿        }

﻿        /// <summary>
﻿        /// Сводный паспорт
﻿        /// </summary>
﻿        public class CombinedPassport
﻿        {
﻿            /// <summary>
﻿            /// Номер п/п
﻿            /// </summary>
﻿            public string PpNumber { get; set; }

﻿            /// <summary>
﻿            /// Наименование
﻿            /// </summary>
﻿            public string Name { get; set; }

﻿            /// <summary>
﻿            /// Значение
﻿            /// </summary>
﻿            public string Value { get; set; }

﻿            /// <summary>
﻿            /// Примечание
﻿            /// </summary>
﻿            public string InfoSupplier { get; set; }

﻿            /// <summary>
﻿            /// Признак того, что значения отличаются для разных контрагентов
﻿            /// </summary>
﻿            public bool IsMultiple { get; set; }

﻿            /// <summary>
﻿            /// Значение родителя
﻿            /// </summary>
﻿            public long ParentValue { get; set; }
﻿        }

﻿        /// <summary>
﻿        /// Атрибут
﻿        /// </summary>
﻿        public class Attribute
﻿        {
﻿            /// <summary>
﻿            /// Код
﻿            /// </summary>
﻿            public string Code { get; set; }

﻿            /// <summary>
﻿            /// Наименование
﻿            /// </summary>
﻿            public string Name { get; set; }

﻿            /// <summary>
﻿            /// Значения
﻿            /// </summary>
﻿            public IEnumerable<AttrValue> Values { get; set; }
﻿        }

﻿        /// <summary>
﻿        /// Значение атрибута
﻿        /// </summary>
﻿        public class AttrValue
﻿        {
﻿            /// <summary>
﻿            /// Значение
﻿            /// </summary>
﻿            public string Value { get; set; }

﻿            /// <summary>
﻿            /// Тип значения
﻿            /// </summary>
﻿            public ValueType ValueType { get; set; }

﻿            /// <summary>
﻿            /// Exp
﻿            /// </summary>
﻿            public int Exp { get; set; }

﻿            /// <summary>
﻿            /// Контрагент
﻿            /// </summary>
﻿            public Contragent Contragent { get; set; }
﻿        }

﻿        /// <summary>
﻿        /// Прокси класс для метаатрибутов
﻿        /// </summary>
﻿        public class ProxyMetaAttribute
﻿        {
﻿            /// <summary>
﻿            /// ИД
﻿            /// </summary>
﻿            public long Id { get; set; }

﻿            /// <summary>
﻿            /// Код
﻿            /// </summary>
﻿            public string Code { get; set; }

﻿            /// <summary>
﻿            /// Наименование
﻿            /// </summary>
﻿            public string Name { get; set; }

﻿            /// <summary>
﻿            /// ИД родительского раздела
﻿            /// </summary>
﻿            public long ParentPartId { get; set; }

﻿            /// <summary>
﻿            /// ИД родителя
﻿            /// </summary>
﻿            public long? ParentId { get; set; }

﻿            /// <summary>
﻿            /// Тип метаатрибута
﻿            /// </summary>
﻿            public MetaAttributeType Type { get; set; }

﻿            /// <summary>
﻿            /// Порядок атрибута
﻿            /// </summary>
﻿            public virtual int OrderNum { get; set; }
﻿        }

﻿        /// <summary>
﻿        /// Прокси класс для строк провайдера паспорта
﻿        /// </summary>
﻿        public class ProxyPassportProviderRow
﻿        {
﻿            /// <summary>
﻿            /// ИД
﻿            /// </summary>
﻿            public long Id { get; set; }

﻿            /// <summary>
﻿            /// ИД метаатрибута
﻿            /// </summary>
﻿            public long MetaAttributeId { get; set; }

﻿            /// <summary>
﻿            /// Код
﻿            /// </summary>
﻿            public string Code { get; set; }

﻿            /// <summary>
﻿            /// Значение
﻿            /// </summary>
﻿            public string Value { get; set; }

﻿            /// <summary>
﻿            /// Exp
﻿            /// </summary>
﻿            public int Exp { get; set; }

﻿            /// <summary>
﻿            /// ИД контрагента
﻿            /// </summary>
﻿            public long? ContragentId { get; set; }

﻿            /// <summary>
﻿            /// Наименование контрагента
﻿            /// </summary>
﻿            public string ContragentName { get; set; }

﻿            /// <summary>
﻿            /// Значение родителя
﻿            /// </summary>
﻿            public long ParentValue { get; set; }
﻿        }

﻿        /// <summary>
﻿        /// Данный класс сравнивает коды атрибутов структуры паспорта.
﻿        /// </summary>
﻿        private class CodeComparer : IComparer<string>
﻿        {
﻿            /// <summary>
﻿            /// The compare.
﻿            /// </summary>
﻿            /// <param name="x">The x.</param>
﻿            /// <param name="y">The y.</param>
﻿            /// <returns>The <see cref="int"/>.</returns>
﻿            public int Compare(string x, string y)
﻿            {
﻿                // удаляем последнюю точку в строке, чтобы не возникало пустых значений
﻿                var firstCodeSplit = x.TrimEnd('.').Split('.');
﻿                var secondCodeSplit = y.TrimEnd('.').Split('.');

﻿                // находим длину более короткого массива, чтоб не выйти в потом цикле за его индексы
﻿                var minLengthCode = System.Math.Min(firstCodeSplit.Length, secondCodeSplit.Length);

﻿                for (int i = 0; i < minLengthCode; i++)
﻿                {
﻿                    // вытаскиваем соответствующие значения разделенные точками из каждого массива
﻿                    var firstCodeString = firstCodeSplit[i];
﻿                    var secondCodeString = secondCodeSplit[i];

﻿                    int firstCode;
﻿                    int secondCode;

﻿                    // пытаемся их превратить в целые числа
﻿                    int.TryParse(firstCodeString, out firstCode);
﻿                    int.TryParse(secondCodeString, out secondCode);

﻿                    // если какой-то код равен нулю это значит, что превратить его в число не удалось и коды надо сравнить как строки
﻿                    if (firstCode == 0 || secondCode == 0)
﻿                    {
﻿                        var comp = string.CompareOrdinal(firstCodeString, secondCodeString);

﻿                        // не выходим, если значения равны
﻿                        if (comp != 0)
﻿                        {
﻿                            return comp;
﻿                        }
﻿                    }
﻿                    else
﻿                    {
﻿                        var comp = firstCode.CompareTo(secondCode);

﻿                        // не выходим, если значения равны
﻿                        if (comp != 0)
﻿                        {
﻿                            return comp;
﻿                        }
﻿                    }
﻿                }

﻿                // Если попали сюда, то значит по минимальной длинне коды совпадают. Сравниваем их тогда просто по длинне.
﻿                return firstCodeSplit.Length.CompareTo(secondCodeSplit.Length);
﻿            }
﻿        }
﻿    }
﻿}