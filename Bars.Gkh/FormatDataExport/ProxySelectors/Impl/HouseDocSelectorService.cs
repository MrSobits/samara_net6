namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.TechPassport;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.BasePassport;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.PassportProvider;
    using Bars.Gkh.Serialization;

    /// <summary>
    /// Селектор для Паспорт дома
    /// </summary>
    public class HouseDocSelectorService : BaseProxySelectorService<HouseDocProxy>
    {
        private readonly List<FieldDescriptor> fieldDescriptors;
        private readonly Dictionary<string, int> componentCodes;
        private Dictionary<string, Dictionary<long, string>> dictCache;
        private readonly string[] replaceDateFields = { "93", "95" };
        private readonly string[] replaceDecimalFields = { "62", "110", "202", "215" };
        private readonly string[] yearToIntFields = { "19", "23", "5", "28", "34", "12", "100", "144" };

        public HouseDocSelectorService(IPassportProvider passportProvider)
        {
            this.fieldDescriptors = new List<FieldDescriptor>
            {
                new FieldDescriptor("Form_3_2", "Form_3_2", 1, 3, "18", true),
                new FieldDescriptor("Form_3_2", "Form_3_2_3", 10, "19"),
                new FieldDescriptor("Form_3_2", "Form_3_2", 1, 3, "20"),
                new FieldDescriptor("Form_3_2", "Form_3_2_3", 11, "41"),
                new FieldDescriptor("Form_3_2", "Form_3_2_3", 13, "213"),
                new FieldDescriptor("Form_3_2", "Form_3_2_4", 2, "42"),
                new FieldDescriptor("Form_3_2", "Form_3_2_3", 18, "43"),
                new FieldDescriptor("Form_3_2", "Form_3_2_3", 19, "107"),
                new FieldDescriptor("Form_3_2", "Form_3_2_4", 3, "50"),
                new FieldDescriptor("Form_3_2", "Form_3_2_3", 16, "51"),
                new FieldDescriptor("Form_3_2", "Form_3_2_4", 1, "52"),
                new FieldDescriptor("Form_3_2_CW", "Form_3_2_CW", 1, 3, "21", true),
                new FieldDescriptor("Form_3_2_CW", "Form_3_2_CW", 1, 3, "22"),
                new FieldDescriptor("Form_3_2_CW", "Form_3_2CW_3", 2, "23"),
                new FieldDescriptor("Form_3_2_CW", "Form_3_2CW_3", 3, "38"),
                new FieldDescriptor("Form_3_2_CW", "Form_3_2CW_3", 5, "212"),
                new FieldDescriptor("Form_3_2_CW", "Form_3_2CW_3", 10, "106"),
                new FieldDescriptor("Form_3_2_CW", "Form_3_2CW_4", 2, "39"),
                new FieldDescriptor("Form_3_2_CW", "Form_3_2CW_3", 9, "40"),
                new FieldDescriptor("Form_3_2_CW", "Form_3_2CW_3", 7, "8"),
                new FieldDescriptor("Form_3_2_CW", "Form_3_2CW_4", 1, "24"),
                new FieldDescriptor("Form_3_3", "Form_3_3_3", 14, "5"),
                new FieldDescriptor("Form_3_3", "Form_3_3", 1, 3, "6"),
                new FieldDescriptor("Form_3_3", "Form_3_3_3", 15, "7"),
                new FieldDescriptor("Form_3_3", "Form_3_3_3", 17, "216"),
                new FieldDescriptor("Form_3_3_Water", "Form_3_3_Water", 1, 3, "25"),
                new FieldDescriptor("Form_3_3_Water", "Form_3_3_Water_3", 1, "26"),
                new FieldDescriptor("Form_3_3_Water", "Form_3_3_Water", 1, 3, "27", true),
                new FieldDescriptor("Form_3_3_Water", "Form_3_3_Water_2", 2, "28"),
                new FieldDescriptor("Form_3_3_Water", "Form_3_3_Water_2", 4, "214"),
                new FieldDescriptor("Form_3_4", "Form_3_4_2", 7, "32"),
                new FieldDescriptor("Form_3_4", "Form_3_4", 1, 3, "33"),
                new FieldDescriptor("Form_3_4", "Form_3_4_2", 5, "34"),
                new FieldDescriptor("Form_3_4", "Form_3_4", 1, 3, "35", true),
                new FieldDescriptor("Form_3_4", "Form_3_4_2", 8, "215"),
                new FieldDescriptor("Form_3_1", "Form_3_1_3", 20, "9"),
                new FieldDescriptor("Form_3_1", "Form_3_1_4", 1, "10"),
                new FieldDescriptor("Form_3_1", "Form_3_1", 1, 3, "11", true),
                new FieldDescriptor("Form_3_1", "Form_3_1_3", 19, "12"),
                new FieldDescriptor("Form_3_1", "Form_3_1", 1, 3, "13"),
                new FieldDescriptor("Form_3_1", "Form_3_1_3", 22, "211"),
                new FieldDescriptor("Form_3_1", "Form_3_1_3", 34, "104"),
                new FieldDescriptor("Form_3_1", "Form_3_1_3", 33, "105"),
                new FieldDescriptor("Form_3_1", "Form_3_1_3", 32, "53"),
                new FieldDescriptor("Form_3_1", "Form_3_1_4", 6, "54"),
                new FieldDescriptor("Form_3_1", "Form_3_1_3", 30, "103"),
                new FieldDescriptor("Form_3_1", "Form_3_1_4", 3, "15"),
                new FieldDescriptor("Form_3_1", "Form_3_1_3", 24, "16"),
                new FieldDescriptor("Form_3_1", "Form_3_1_4", 2, "17"),
                new FieldDescriptor("Form_3_1", "Form_3_1_3", 29, "29"),
                new FieldDescriptor("Form_3_1", "Form_3_1_4", 4, "30"),
                new FieldDescriptor("Form_3_1", "Form_3_1_4", 5, "31"),
                new FieldDescriptor("Form_1", "Form_1", 34, "37"),
                new FieldDescriptor("Form_6_1", "Form_6_1_1", 7, "61"),
                new FieldDescriptor("Form_1", "Form_1", 20, "62"),
                new FieldDescriptor("Form_1", "Form_1", 36, "63"),
                new FieldDescriptor("Form_1", "Form_1", 12, "64"),
                new FieldDescriptor("Form_1", "Form_1", 35, "65"),
                new FieldDescriptor("Form_1_2", "Form_1_2_3", 4, "73"),
                new FieldDescriptor("Form_1_2", "Form_1_2_3", 4, "74"),
                new FieldDescriptor("Form_1", "Form_1", 2, "88"),
                new FieldDescriptor("Form_1", "Form_1_1", 1, "89"),
                new FieldDescriptor("Form_1", "Form_1", 3, "90"),
                new FieldDescriptor("Form_1", "Form_1", 28, "91"),
                new FieldDescriptor("Form_1", "Form_1_1", 2, "92"),
                new FieldDescriptor("Form_1", "Form_1", 31, "93"),
                new FieldDescriptor("Form_1", "Form_1", 45, "95"),
                new FieldDescriptor("Form_1", "Form_1", 48, "130"),
                new FieldDescriptor("Form_1_3", "Form_1_3", 3, "108"),
                new FieldDescriptor("Form_1_3", "Form_1_3", 5, "109"),
                new FieldDescriptor("Form_1", "Form_1", 7, "110"),
                new FieldDescriptor("Form_1", "Form_1", 30, "59"),
                new FieldDescriptor("Form_2", "Form_2", 1, 3, "60"),
                new FieldDescriptor("Form_1", "Form_1", 37, "1"),
                new FieldDescriptor("Form_1", "Form_1", 38, "3"),
                new FieldDescriptor("Form_5_3", "Form_5_3", 1, 3, "55", true),
                new FieldDescriptor("Form_5_3", "Form_5_3_2", 4, 4, "205"),
                new FieldDescriptor("Form_5_8", "Form_5_8", 40, "97"),
                new FieldDescriptor("Form_5_8", "Form_5_8_2", 2, "98"),
                new FieldDescriptor("Form_5_8", "Form_5_8_2", 1, "99"),
                new FieldDescriptor("Form_5_8", "Form_5_8", 39, "100"),
                new FieldDescriptor("Form_5_8", "Form_5_8_2", 3, "101"),
                new FieldDescriptor("Form_1", "Form_1", 39, "2"),
                new FieldDescriptor("Form_5_5", "Form_5_5", 17, "209"),
                new FieldDescriptor("Form_5_5", "Form_5_5_2", 1, "4"),
                new FieldDescriptor("Form_5_5", "Form_5_5", 15, "208"),

                new FieldDescriptor("Form_5_1", "Form_5_1", 1, 3, "46"),
                new FieldDescriptor("Form_5_1", "Form_5_1", 2, 3, "46"),
                new FieldDescriptor("Form_5_1", "Form_5_1", 3, 3, "46"),
                new FieldDescriptor("Form_5_1", "Form_5_1", 4, 3, "46"),
                new FieldDescriptor("Form_5_1", "Form_5_1", 5, 3, "46"),

                new FieldDescriptor("Form_5_1", "Form_5_1_2", 1, "47"),
                new FieldDescriptor("Form_5_1", "Form_5_1_1", 2, 4, "48"),
                new FieldDescriptor("Form_5_1", "Form_5_1_1", 6, 4, "49"),
                new FieldDescriptor("Form_5_1", "Form_5_1_1", 5, 4, "203"),
                new FieldDescriptor("Form_5_2", "Form_5_2", 1, 3, "58"),
                new FieldDescriptor("Form_5_2", "Form_5_2_2", 3, 4, "204"),
                new FieldDescriptor("Form_5_6", "Form_5_6", 1, 3, "147", true),
                new FieldDescriptor("Form_5_6", "Form_5_6_3", 2, "146"),
                new FieldDescriptor("Form_5_6", "Form_5_6", 3, 3, "56"),
                new FieldDescriptor("Form_5_6", "Form_5_6_3", 1, "57"),
                new FieldDescriptor("Form_5_6", "Form_5_6_3", 3, "143"),
                new FieldDescriptor("Form_5_6", "Form_5_6_2", 27, "144"),
                new FieldDescriptor("Form_5_6", "Form_5_6_2", 30, "207"),
                new FieldDescriptor("Form_1_2", "Form_1_2_1", 1, "133"),
                new FieldDescriptor("Form_5_11", "Form_5_11", 3, "134"),
                new FieldDescriptor("Form_2", "Form_2_1", 1, 3, "137"),
                new FieldDescriptor("Form_3_1", "Form_3_1_4", 1, "152"),
                new FieldDescriptor("Form_3_2", "Form_3_2_3", 21, "157"),
                new FieldDescriptor("Form_1", "Form_1", 13, "71")
            };

            var editors = ((List<EditorTechPassport>)passportProvider.GetEditors())
                .ToDictionary(x => x.Code);

            //дополняем описание типом поля и названием сущности (для словарей)
            foreach (var fieldDescription in this.fieldDescriptors)
            {
                var comp = (ComponentTechPassport)passportProvider.GetComponentBy(fieldDescription.FormCode, fieldDescription.ComponentCode);
                
                if (comp.Elements.Count > 0)
                {
                    var cell = comp.Elements.First(x => x.Code.Equals(fieldDescription.CellCode));

                    fieldDescription.TypeEditor = cell.Editor;

                    if (fieldDescription.TypeEditor == TypeEditor.Dict
                        || fieldDescription.TypeEditor == TypeEditor.MultiDict)
                    {
                        fieldDescription.EntityName = editors.Get(cell.EditorCode).EntityType;
                    }
                }
                else
                {
                    fieldDescription.TypeEditor = comp.Columns.Where(x => x.Code == fieldDescription.ColumnId.ToStr()).Select(x => x.Editor).FirstOrDefault();
                }
            }

            this.componentCodes = this.fieldDescriptors.Select(x => new { x.ComponentCode, x.ColumnId })
                .DistinctBy(x => x.ComponentCode)
                .AsEnumerable()
                .GroupBy(x => x.ComponentCode)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ColumnId).FirstOrDefault());
        }

        /// <inheritdoc />
        protected override IDictionary<long, HouseDocProxy> GetCache()
        {
            return this.GetProxies(this.ProxySelectorFactory.GetSelector<HouseProxy>().ExtProxyListCache)
                .ToDictionary(x => x.Id);
        }

        protected virtual IEnumerable<HouseDocProxy> GetProxies(ICollection<HouseProxy> houseProxies)
        {
            var tehPassportCacheService = this.Container.Resolve<ITehPassportCacheService>();

            using (this.Container.Using(tehPassportCacheService))
            {
                houseProxies = houseProxies.ToList();

                //получаем данные из техпаспорта
                var cache = new List<TehPassportCacheCell>();
                var houseIds = houseProxies.Select(x => x.Id).ToList();

                foreach (var formCode in this.componentCodes)
                {
                    cache.AddRange(tehPassportCacheService.GetCacheByRealityObjectsAndRows(formCode.Key, formCode.Value, houseIds));
                }

                var techPassportCache = cache.GroupBy(x => $"{x.FormCode}#{x.RowId}")
                    .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.RealityObjectId));

                //получаем данные из словарей
                this.InitDictCache();

                int row = 1;
                
                var data = new List<HouseDocProxy>();

                foreach (var fieldDescriptor in this.fieldDescriptors)
                {
                    var valueDict = techPassportCache.Get(fieldDescriptor.CacheKey) ?? new Dictionary<long, TehPassportCacheCell>();
                    foreach (var houseId in houseIds)
                    {
                        if (valueDict.ContainsKey(houseId))
                        {
                            data.AddRange(this.CreateProxies(fieldDescriptor, valueDict.Get(houseId), ref row, houseId));
                        }
                        else
                        {
                            data.Add(this.CreateEmptyProxy(fieldDescriptor, houseId, ref row));
                        }
                    }
                }

                return data.Where(x => x.DateValue != null ||
                        !string.IsNullOrEmpty(x.DictValue) ||
                        !string.IsNullOrEmpty(x.TextValue) ||
                        x.BoolValue != null ||
                        x.DecimalValue != null ||
                        x.IntValue != null ||
                        x.FileValue != null);
            }
        }

        protected void InitDictCache()
        {
            var dictNames = this.fieldDescriptors.Where(x => x.TypeEditor == TypeEditor.Dict || x.TypeEditor == TypeEditor.MultiDict)
                .Select(x => x.EntityName)
                .ToHashSet();

            var result = new Dictionary<string, Dictionary<long, string>>();

            foreach (var dictName in dictNames)
            {
                var repository = this.GetRepository(dictName);
                if (repository != null)
                {
                    using (this.Container.Using(repository))
                    {
                        result.Add(dictName, this.GetDictData(repository as dynamic));
                    }
                }
            }

            this.dictCache = result;
        }

        private IRepository GetRepository(string className)
        {
            var entityType = Type.GetType($"{className}, Bars.Gkh");
            if (entityType != null && entityType.IsSubclassOf(typeof(BaseGkhDict)))
            {
                var reposType = typeof(IRepository<>).MakeGenericType(entityType);
                var repository = this.Container.Resolve(reposType);
                return repository as IRepository;
            }

            return null;
        }

        private Dictionary<long, string> GetDictData<T>(IRepository<T> repos)
            where T : BaseGkhDict
        {
            return repos.GetAll()
                .ToDictionary(x => x.Id, x => x.Code);
        }

        private List<HouseDocProxy> CreateProxies(FieldDescriptor fieldDescriptor, TehPassportCacheCell cell, ref int row, long houseId)
        {
            var tpCompareCodeRepo = this.Container.ResolveRepository<TechnicalPassportCompareCode>();
            var roRepo = this.Container.ResolveRepository<RealityObject>();

            using (this.Container.Using(tpCompareCodeRepo, roRepo))
            {
                var result = new List<HouseDocProxy>();

                if (fieldDescriptor.TypeEditor != TypeEditor.MultiDict)
                {
                    var entity = new HouseDocProxy
                    {
                        Id = row,
                        HouseId = cell.RealityObjectId,
                        Code = fieldDescriptor.ExportCode
                    };

                    if (this.replaceDateFields.Contains(fieldDescriptor.ExportCode))
                    {
                        entity.DateValue = this.ReplaceStringDateToDateTime(cell.Value);

                        result.Add(entity);
                        row++;
                        return result;
                    }

                    if (this.replaceDecimalFields.Contains(fieldDescriptor.ExportCode))
                    {
                        entity.DecimalValue = cell.Value.ToDecimal();

                        result.Add(entity);
                        row++;
                        return result;
                    }

                    if (this.yearToIntFields.Contains(fieldDescriptor.ExportCode))
                    {
                        entity.IntValue = this.ReplaceStringDateToDateTime(cell.Value).Value.Year;

                        result.Add(entity);
                        row++;
                        return result;
                    }

                    if (fieldDescriptor.ExportCode == "71")
                    {
                        var typeHouse = roRepo.Get(houseId).TypeHouse;
                        if (typeHouse == TypeHouse.BlockedBuilding || typeHouse == TypeHouse.Individual)
                        {
                            entity.IntValue = cell.Value.ToInt();
                        }

                        result.Add(entity);
                        row++;
                        return result;
                    }

                    if (fieldDescriptor.ExportCode == "88")
                    {
                        entity.IntValue = cell.Value.ToInt();
                        
                        result.Add(entity);
                        row++;
                        return result;
                    }

                    if (fieldDescriptor.ExportCode == "46")
                    {
                        if (cell.Value != "0")
                        {
                            switch (fieldDescriptor.CellCode)
                            {
                                case "1:3":
                                    entity.DictValue = "6";
                                    break;
                                case "2:3":
                                    entity.DictValue = "16";
                                    break;
                                case "3:3":
                                    entity.DictValue = "4";
                                    break;
                                case "4:3":
                                    entity.DictValue = "5";
                                    break;
                                case "5:3":
                                    entity.DictValue = "1";
                                    break;
                            }

                            result.Add(entity);
                            row++;
                        }

                        return result;
                    }

                    if (fieldDescriptor.ComponentCode == "Form_1_2_3" && fieldDescriptor.CellCode == "4:1")
                    {
                        switch (fieldDescriptor.ExportCode)
                        {
                            case "73":
                                entity.BoolValue = cell.Value != "0";
                                break;
                            case "74":
                                entity.DictValue = cell.Value;
                                break;
                        }
                        result.Add(entity);
                        row++;

                        return result;
                    }

                    if (fieldDescriptor.ComponentCode == "Form_1_1" && fieldDescriptor.CellCode == "1:1")
                    {
                        entity.DictValue = cell.Value;

                        result.Add(entity);
                        row++;

                        return result;
                    }

                    if (fieldDescriptor.CodeRis)
                    {
                        if (cell.Value != null)
                        {
                            entity.DictValue = tpCompareCodeRepo.GetAll().Where(x => x.FormCode.ToLower() == fieldDescriptor.FormCode.ToLower() &&
                                    x.CellCode.ToLower() == fieldDescriptor.CellCode.ToLower() &&
                                    x.Value.ToLower() == cell.Value.ToLower())
                                .Select(x => x.CodeRis)
                                .FirstOrDefault();
                        }

                        result.Add(entity);
                        row++;

                        return result;
                    }

                    switch (fieldDescriptor.TypeEditor)
                    {
                        case 0:
                        case TypeEditor.Text:
                            entity.TextValue = cell.Value;
                            break;

                        case TypeEditor.Int:
                            entity.IntValue = HouseDocSelectorService.GetIntValueByFieldDescriptor(fieldDescriptor, cell.Value);
                            break;

                        case TypeEditor.Decimal:
                        case TypeEditor.Double:
                            entity.DecimalValue = cell.Value.ToDecimal();
                            break;

                        case TypeEditor.Bool:
                            
                                entity.BoolValue = cell.Value.ToBool();
                            break;

                        // если значение = отсутствует, то передавать нет
                        case TypeEditor.TypeHotWater:
                            entity.BoolValue = cell.Value != "6";
                            break;

                        case TypeEditor.TypeColdWater:
                        case TypeEditor.TypeSewage:
                        case TypeEditor.TypeGas:
                            entity.BoolValue = cell.Value != "3";
                            break;

                        case TypeEditor.TypePower:
                            entity.BoolValue = cell.Value != "2";
                            break;
                            
                        case TypeEditor.TypeHeating:
                            entity.BoolValue = cell.Value != "0";
                            break;
                            
                        case TypeEditor.Date:
                        {
                            DateTime date;
                            if (DateTime.TryParse(cell.Value, out date))
                            {
                                entity.DateValue = date.ToDateTime();
                            }

                            break;
                        }

                        case TypeEditor.Dict:
                            entity.DictValue = this.dictCache.Get(fieldDescriptor.EntityName).Get(cell.Value.ToLong());
                            break;
                    }

                    result.Add(entity);
                    row++;
                }
                else
                {
                    //в итоговом файле создается не 1 строка, а много, по числу выбранных из словаря значений              
                    var ids = cell.Value.Trim('[', ']')
                        .Split(new []{ "," }, StringSplitOptions.RemoveEmptyEntries);

                    var codes = fieldDescriptor.CodeRis
                        ? tpCompareCodeRepo.GetAll()
                            .Where(x => x.FormCode.ToLower() == fieldDescriptor.FormCode.ToLower() &&
                                x.CellCode.ToLower() == fieldDescriptor.CellCode.ToLower() &&
                                ids.Contains(x.Value))
                            .Select(x => x.CodeRis)
                        : this.dictCache.Get(fieldDescriptor.EntityName)
                        .WhereIf(!(ids.Length == 1 && ids[0] == "-1"),
                            x => ids.Select(y => y.ToLong()).Contains(x.Key))
                        .Select(x => x.Value);

                    foreach (var code in codes)
                    {
                        result.Add(
                            new HouseDocProxy
                            {
                                Id = row,
                                HouseId = cell.RealityObjectId,
                                Code = fieldDescriptor.ExportCode,
                                DictValue = code
                            });
                        row++;
                    }
                }

                return result;
            }
        }

        private static int? GetIntValueByFieldDescriptor(FieldDescriptor fieldDescriptor, string value)
        {
            var valueLength = value?.Length ?? 0;
            //если значение одного из следующих свойств:
            //1."Год проведения последнего капитального ремонта"
            //2."Год проведения последнего капитального ремонта печей, каминов"
            //3."Год проведения последнего капитального ремонта крыши"
            // то оставляем последние 4 символа (год)
            if (valueLength > 4 && (string.Equals(fieldDescriptor.ComponentCode, "Form_5_1_1") && string.Equals(fieldDescriptor.CellCode, "6:4") 
                || string.Equals(fieldDescriptor.ComponentCode, "Form_3_1_3") && string.Equals(fieldDescriptor.CellCode, "34:1")
                || string.Equals(fieldDescriptor.ComponentCode, "Form_5_6") && string.Equals(fieldDescriptor.CellCode, "3:3")))
            {
                value = value.Substring(valueLength - 4, 4);
            }
            
            return value.ToInt();
        }

        private DateTime? ReplaceStringDateToDateTime(string date)
        {
            DateTime outputDate;
            var dateInt = date.ToInt();
            // тут и подменяем, но бывает что дата приходит не в формате дата-строка, но и просто год,
            return DateTime.TryParse(date, out outputDate) 
                ? outputDate 
                : dateInt > 0 ? new DateTime(dateInt, 1, 1) : (DateTime?)null;
        }

        private HouseDocProxy CreateEmptyProxy(FieldDescriptor fieldDescriptor, long houseId, ref int row)
        {
            var entity = new HouseDocProxy
            {
                Id = row,
                HouseId = houseId,
                Code = fieldDescriptor.ExportCode
            };

            row++;
            return entity;
        }

        protected class FieldDescriptor
        {
            public FieldDescriptor(string formCode, string componentCode, int rowId, string exportCode, bool codeRis = false)
            {
                this.FormCode = formCode;
                this.ComponentCode = componentCode;
                this.RowId = rowId;
                this.ColumnId = 1;
                this.ExportCode = exportCode;
                this.CodeRis = codeRis;
            }

            public FieldDescriptor(string formCode, string componentCode, int rowId, int columnId, string exportCode, bool codeRis = false)
            {
                this.FormCode = formCode;
                this.ComponentCode = componentCode;
                this.RowId = rowId;
                this.ColumnId = columnId;
                this.ExportCode = exportCode;
                this.CodeRis = codeRis;
            }

            // брать ли значение code_ris из таблицы сопоставлятора - tp_compare_code 
            public bool CodeRis { get; set; }

            public static int DefaultColumn => 1;

            public string FormCode { get; set; }

            public string ComponentCode { get; set; }

            public int RowId { get; set; }

            public int ColumnId { get; set; }

            public string ExportCode { get; set; }

            public TypeEditor TypeEditor { get; set; }

            public string CacheKey => $"{this.ComponentCode}#{this.RowId}";

            public string CellCode => $"{this.RowId}:{this.ColumnId}";

            /// <summary>
            /// Имя сущности (для Dict и MultiDict)
            /// </summary>
            public string EntityName { get; set; }
        }
    }
}