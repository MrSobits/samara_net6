namespace Bars.Gkh.Regions.Msk.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using B4;
    using Castle.Windsor;
    using Domain;
    using Domain.CollectionExtensions;
    using Enums.Import;
    using Gkh.Import;
    using Gkh.Import.Impl;
    using GkhExcel;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Импорт состояний
    /// </summary>
     public class MskCeoStateImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key
        {
            get { return Id; }
        }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport
        {
            get { return "MskCeoStateImport"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        { 
            get { return "Импорт состояний ООИ (Москва)"; }
        }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "xlsx"; }
        }

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.MskCeoStatePanel.View"; }
        }

        public virtual IWindsorContainer Container { get; set; }

        private readonly Dictionary<string, int> _headersDict = new Dictionary<string, int>();

        private Dictionary<string, RealityObjectStructuralElement> dictRoStrEl = new Dictionary<string, RealityObjectStructuralElement>();

        private Dictionary<string, RealityObjectStructuralElementAttributeValue> dictRoAttribute = new Dictionary<string, RealityObjectStructuralElementAttributeValue>();

        private Dictionary<string, StructuralElementGroupAttribute> dictGroupAttribute = new Dictionary<string, StructuralElementGroupAttribute>();

        private List<ConditionStructElement> rangStates = new List<ConditionStructElement>()
        {
            ConditionStructElement.NotDetermined,
            ConditionStructElement.Unsatisfactory,
            ConditionStructElement.Satisfactory,
            ConditionStructElement.Emergency,
            ConditionStructElement.Normative,
            ConditionStructElement.LimitedUsable,
            ConditionStructElement.Workable
        };

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var data = Extract(baseParams.Files["FileImport"]);

            var uids = data.Select(x => x.Uid).ToArray();

            PrepareDictionaries(uids);

            ProcessData(data);

            return new ImportResult(StatusImport.CompletedWithoutError, string.Empty);
        }

        private void PrepareDictionaries(string[] uids)
        {
            // формируем словарь Сопоставления Id структурных элементов и кодов , тоест ьобрубаю цифровые значения с конца если они имеются 
            
            var strElDomain = Container.ResolveDomain<StructuralElement>();
            var roStrElDomain = Container.ResolveDomain<RealityObjectStructuralElement>();
            var roStrElAttributeDomain = Container.ResolveDomain<RealityObjectStructuralElementAttributeValue>();
            var groupAttributeDomain = Container.ResolveDomain<StructuralElementGroupAttribute>();
            
            try
            {
                var dictCeoCodes = new Dictionary<long, string>();

                var i = 0;
                var strList = strElDomain.GetAll()
                                .Select(x => new
                                {
                                    x.Id,
                                    x.Code
                                })
                                .ToList();

                foreach (var item in strList)
                {
                    if (string.IsNullOrEmpty(item.Code))
                    {
                        continue;
                    }

                    if (dictCeoCodes.ContainsKey(item.Id))
                    {
                        continue;
                    }

                    var code = item.Code;

                    // обрубаем цифры сконца, чтобы поулчить из кода вида "kan_m_40" значение такое "kan_m"
                    if (code.Contains('_'))
                    {
                        code = string.Empty;

                        foreach (var str in item.Code.Split('_'))
                        {

                            if (!int.TryParse(str, out i))
                            {
                                if (!string.IsNullOrEmpty(code))
                                {
                                    code += "_";
                                }

                                code += str;
                            }
                        }
                    }

                    dictCeoCodes.Add(item.Id, code);
                }
                
                // получаем словарь идентификаора группы, с кодом КЭ
                var dictStrByGroup = strElDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        groupId = x.Group.Id,
                        x.Code
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.groupId,
                        Code = dictCeoCodes.ContainsKey(x.Id) ? dictCeoCodes[x.Id] : x.Code
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.groupId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Code).Distinct().ToList());

                // поулчаем список атрибутов
                var listAtributes = groupAttributeDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        groupId = x.Group.Id,
                        attr = x
                    })
                    .AsEnumerable()
                    .ToList();

                //проходим по списку и формируем сопоставление для атрибута и кода КЭ
                foreach (var attr in listAtributes)
                {
                    if (dictStrByGroup.ContainsKey(attr.groupId))
                    {

                        foreach ( var code in dictStrByGroup[attr.groupId])
                        {
                            var key = code + "#" + attr.Name.ToLower().Replace(" ", string.Empty).Trim();

                            if (!dictGroupAttribute.ContainsKey(key))
                            {
                                dictGroupAttribute.Add(key, attr.attr);
                            }
                        }                        
                    }
                }
                
                // Формируем существующие записи КЭ по дому
                foreach (var section in uids.Section(1000))
                {
                    //ЗАПРОС НА кэ ПО ДОМАМ
                    var roStrElQuery = roStrElDomain.GetAll()
                        .Where(x => x.RealityObject.ExternalId != null && x.RealityObject.ExternalId != "")
                        .Where(x => section.Contains(x.RealityObject.ExternalId));

                    var portionRoStrEl = roStrElQuery
                        .Select(x => new
                        {
                            Uid = x.RealityObject.ExternalId,
                            x.Id,
                            StrElId = x.StructuralElement.Id,
                            StrElCode = x.StructuralElement.Code,
                            item = x
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.Uid,
                            x.Id,
                            x.StrElId,
                            StrElCode = dictCeoCodes.ContainsKey(x.StrElId) ? dictCeoCodes[x.StrElId] : x.StrElCode,
                            x.item
                        })
                        .GroupBy(x => x.Uid + "#" + x.StrElCode)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.item).FirstOrDefault() );

                    foreach (var kvp in portionRoStrEl)
                    {
                        if (dictRoStrEl.ContainsKey(kvp.Key))
                        {
                            continue;
                        }

                        dictRoStrEl.Add(kvp.Key, kvp.Value);
                    }

                    var portionAttributes = roStrElAttributeDomain.GetAll()
                        .Where(x => roStrElQuery.Any(y => y.Id == x.Object.Id))
                        .Select(x => new
                        {
                            Uid = x.Object.RealityObject.ExternalId,
                            StrElId = x.Object.StructuralElement.Id,
                            x.Attribute.Name,
                            StrElCode = x.Object.StructuralElement.Code,
                            item = x
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.Uid,
                            x.StrElId,
                            x.Name,
                            StrElCode = dictCeoCodes.ContainsKey(x.StrElId) ? dictCeoCodes[x.StrElId] : x.StrElCode,
                            x.item
                        })
                        .GroupBy(x => x.Uid + "#" + x.StrElCode + "#" + x.Name.ToLower().Replace(" ", string.Empty).Trim()) // Внимание код тут убраны пробелы и с маленько буквыы вот пример "455677777_45345435#fasad#состояние(стыки)"
                        .ToDictionary(x => x.Key, y => y.Select(z => z.item).FirstOrDefault());

                    foreach (var kvp in portionAttributes)
                    {
                        if (dictRoAttribute.ContainsKey(kvp.Key))
                        {
                            continue;
                        }

                        dictRoAttribute.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            finally 
            {
                Container.Release(strElDomain);
                Container.Release(roStrElDomain);
                Container.Release(roStrElAttributeDomain);
                Container.Release(groupAttributeDomain);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessData(List<Row> data)
        {
            var roStrToSave = new List<RealityObjectStructuralElement>();

            var roAttributeToSave = new List<RealityObjectStructuralElementAttributeValue>();

            foreach (var row in data)
            {
                //Сначала обновляем для Состояние в кэ дома

                foreach (var kvp in row.CeoStates)
            {

                    var existRecord = dictRoStrEl.Get(row.Uid+"#"+kvp.Key);

                    if (existRecord == null)
                {
                        // Добавить логирование если запись в доме несуществует по КЭ
                        continue;
                    }

                    if (existRecord.Condition != kvp.Value)
                    {
                        existRecord.Condition = kvp.Value;
                        roStrToSave.Add(existRecord);
                    }
                }

                foreach (var kvp in row.AttributeStates)
                {

                    var existRecord = dictRoAttribute.Get(row.Uid + "#" + kvp.Key);

                    if (existRecord == null)
                    {
                        // если не нашили данного атрибута по полному ключу то тогда пытаемся найти запись КЭ в котогрую нужно создать данный атрибут
                        var codeStrEl = kvp.Key.Substring(0, kvp.Key.IndexOf("#"));

                        var strEl = dictRoStrEl.Get(row.Uid + "#" + codeStrEl);

                        if (strEl == null)
                        {
                            // тут необходимо залогировать  то что для создания атрибута в доме ненайден конструктивный элемент 
                            continue;
            }

                        // сначала находим значение в справочнике
                        var attr = dictGroupAttribute.Get(kvp.Key);

                        if (attr == null)
                        {
                            // тут необходимо залогировать  то что ненайден атрибут 
                            continue;
        }

                        existRecord = new RealityObjectStructuralElementAttributeValue
        {
                            Attribute = attr,
                            Object = strEl
                        };

                    }

                    if ( existRecord == null )
                {
                        continue;
                    }

                    if (existRecord.Value != kvp.Value.GetEnumMeta().Display)
                    {
                        existRecord.Value = kvp.Value.GetEnumMeta().Display;
                        roAttributeToSave.Add(existRecord);
                    }
                }

            }

            TransactionHelper.InsertInManyTransactions(Container, roStrToSave, 1000, true, true);

            TransactionHelper.InsertInManyTransactions(Container, roAttributeToSave, 1000, true, true);
            
        }

        private List<Row> Extract(FileData file)
        {
            using (var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                if (file.Extention == "xlsx")
                {
                    excel.UseVersionXlsx();
                }

                var ceoStates = new List<Row>();

                using (var memoryStreamFile = new MemoryStream(file.Data))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    if (rows.Count < 3)
                    {
                        throw new Exception("Некорректный формат");
                    }

                    InitHeaders(rows[0]);

                    for (var i = 1; i < rows.Count; i++)
                    {
                        var tempRow = rows[i];

                        var row = Extract(tempRow);

                        if (row == null)
                            continue;

                        ceoStates.Add(row);
                    }
                }

                return ceoStates;
            }
        }

        /// <summary>
        /// Формирование заголовков импортируемого файла, для того чтобы проверять все ли колонки переданы
        /// </summary>
        /// <param name="cells"></param>
        private void InitHeaders(GkhExcelCell[] cells)
        {
            _headersDict["UID"] = -1;
            _headersDict["MKDOG1O01 MZI_ES_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O02 MZI_CO_cherdak_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O02 MZI_CO_tech_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O02 MZI_CO_etaj_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O03 MZI_GS_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O04 MZI_HVS_etaj_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O04 MZI_HVS_pojar_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O04 MZI_HVS_tech_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O05 MZI_GVS_cherdak_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O05 MZI_GVS_etaj_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O05 MZI_GVS_tech_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O06 MZI_Kanal_tech_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O06 MZI_Kanal_etaj_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O07 MZI_Mys_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG1O09 MZI_SB_PPADY_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG3O01 MZI_Fasad_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG3O01 MZI_Fasad_styki_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG3O02 MZI_Kon_el_Balk_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG3O04 MZI_Kon_el_Vodootvod_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG4O06 MZI_Podval_Ocenka".ToUpper()] = -1;
            _headersDict["MKDOG5O01 MZI_Krov_Ocenka_krov".ToUpper()] = -1;

            for (int i = 0; i < cells.Length; i++)
            {
                var key = cells[i].Value.ToStr().ToUpper();

                if (_headersDict.ContainsKey(key))
                {
                    _headersDict[key] = i;
                }
            }
        }

        /// <summary>
        /// Первоночальная валидация файла перед импортом
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            return true;
        }

        private Row Extract(GkhExcelCell[] row)
        {
            var uid = GetValue(row, "UID");

            if (uid.IsEmpty())
            {
                return null;
            }

            var result = new Row(uid);

            // Этот метод ля обработки всех одиночных столбцов в соторыми нет особой проблемы
            AddSingleStates(result, row);
            AddOtoplenieM(result, row);
            AddGvsM(result, row);

            return result;
        }

        /// <summary>
        /// Метод поулчения наименьшего из перданныйх статусов
        /// </summary>
        public ConditionStructElement GetMinState(ConditionStructElement[] values)
        {
            if (values.Length == 0)
                return ConditionStructElement.NotDetermined;

            var minValue = values[0];
            var minIndex = rangStates.IndexOf(values[0]);
            var idx = minIndex;

            foreach (var val in values)
            {
                if (val != minValue)
        {
                    idx = rangStates.IndexOf(val);

                    if (idx < minIndex)
            {
                        minIndex = idx;
                        minValue = val;
                    }
                }
            }
            
            return minValue;
        }

        private void AddSingleStates(Row row, GkhExcelCell[] cells)
        {
            row.CeoStates["electro"] = GetStateValue(cells, "MKDOG1O01 MZI_ES_Ocenka");
            row.CeoStates["otoplenie"] = GetStateValue(cells, "MKDOG1O02 MZI_CO_etaj_Ocenka");
            row.CeoStates["gas"] = GetStateValue(cells, "MKDOG1O03 MZI_GS_Ocenka");
            row.CeoStates["hvs"] = GetStateValue(cells, "MKDOG1O04 MZI_HVS_etaj_Ocenka");
            row.CeoStates["pv"] = GetStateValue(cells, "MKDOG1O04 MZI_HVS_pojar_Ocenka");
            row.CeoStates["hvs_m"] = GetStateValue(cells, "MKDOG1O04 MZI_HVS_tech_Ocenka");
            row.CeoStates["gvs"] = GetStateValue(cells, "MKDOG1O05 MZI_GVS_etaj_Ocenka");
            row.CeoStates["kan_m"] = GetStateValue(cells, "MKDOG1O06 MZI_Kanal_tech_Ocenka");
            row.CeoStates["kan"] = GetStateValue(cells, "MKDOG1O06 MZI_Kanal_etaj_Ocenka");
            row.CeoStates["mus"] = GetStateValue(cells, "MKDOG1O07 MZI_Mys_Ocenka");
            row.CeoStates["ppiadu"] = GetStateValue(cells, "MKDOG1O09 MZI_SB_PPADY_Ocenka");
            row.CeoStates["fasad"] = GetStateValue(cells, "MKDOG3O01 MZI_Fasad_Ocenka");
            row.AttributeStates["fasad#состояние(стыки)"] = GetStateValue(cells, "MKDOG3O01 MZI_Fasad_styki_Ocenka"); // Для аттрибутов
            row.CeoStates["balcony"] = GetStateValue(cells, "MKDOG3O02 MZI_Kon_el_Balk_Ocenka");
            row.CeoStates["vdsk"] = GetStateValue(cells, "MKDOG3O04 MZI_Kon_el_Vodootvod_Ocenka");
            row.CeoStates["podval"] = GetStateValue(cells, "MKDOG4O06 MZI_Podval_Ocenka");
            row.CeoStates["krov"] = GetStateValue(cells, "MKDOG5O01 MZI_Krov_Ocenka_krov");
        }

        /// <summary>
        ///  Тут для отопления поулчаем наименьшее значение и заполняем атрибуты
        /// </summary>
        private void AddOtoplenieM(Row row, GkhExcelCell[] cells)
            {
            
            var cherdak = GetStateValue(cells, "MKDOG1O02 MZI_CO_cherdak_Ocenka");
            var podval = GetStateValue(cells, "MKDOG1O02 MZI_CO_tech_Ocenka");

            row.CeoStates["otoplenie_m"] = GetMinState(new[] { cherdak, podval });
            row.AttributeStates["otoplenie_m#состояние(чердак)"] = cherdak;
            row.AttributeStates["otoplenie_m#состояние(подвал)"] = podval;
        }

        /// <summary>
        ///  Тут для ГВС поулчаем наименьшее значение и заполняем атрибуты
        /// </summary>
        private void AddGvsM(Row row, GkhExcelCell[] cells)
        {
            var cherdak = GetStateValue(cells, "MKDOG1O05 MZI_GVS_cherdak_Ocenka");
            var podval = GetStateValue(cells, "MKDOG1O05 MZI_GVS_tech_Ocenka");

            row.CeoStates["gvs_m"] = GetMinState(new[] { cherdak, podval });
            row.AttributeStates["gvs_m#состояние(чердак)"] = cherdak;
            row.AttributeStates["gvs_m#состояние(подвал)"] = podval;
        }

        /// <summary>
        /// Получаем строковое значение ячейки
        /// </summary>
        private string GetValue(GkhExcelCell[] row, string key)
        {
            key = key.ToUpper();

            var result = string.Empty;

            if (!_headersDict.ContainsKey(key))
                return result;

            var index = _headersDict[key];

            if (index < 0 && index >= row.Length)
                return result;

            return row[index].Value.ToStr();
        }

        /// <summary>
        /// Поулчаем Статус переводя стркоовое значение в значение Енума
        /// </summary>
        private ConditionStructElement GetStateValue(GkhExcelCell[] row, string key)
        {
            var value = GetValue(row, key);

            var result = ConditionStructElement.NotDetermined;

            if (string.IsNullOrEmpty(value))
                return result;

            switch (value)
            {
                case "не определялось": result = ConditionStructElement.NotDetermined; break;
                case "аварийное": result = ConditionStructElement.Emergency; break;
                case "неудовлетворительное": result = ConditionStructElement.Unsatisfactory; break;
                case "удовлетворительное": result = ConditionStructElement.Satisfactory; break;
                case "ограниченно-работоспособное": result = ConditionStructElement.LimitedUsable; break;
                case "нормативное": result = ConditionStructElement.Normative; break;
                case "работоспособное": result = ConditionStructElement.Workable; break;
            }

            return result;
        }

        /// <summary>
        ///  Запись по дому 
        /// </summary>
        private class Row
        {
            public Row(string uid)
            {
                Uid = uid;
                CeoStates = new Dictionary<string, ConditionStructElement>();
                AttributeStates = new Dictionary<string, ConditionStructElement>();
            }

            public string Uid { get; private set; }

            public Dictionary<string, ConditionStructElement> CeoStates { get; private set; }

            public Dictionary<string, ConditionStructElement> AttributeStates { get; private set; }
        }
    }
}
