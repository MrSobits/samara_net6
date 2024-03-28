namespace Bars.GkhOverhaulTp.Regions.Chelyabinsk.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Entities;

    public class CopyStructElementsFromTpAction : BaseExecutionAction
    {
        private readonly Dictionary<string, MyClass> MappingDict;

        public CopyStructElementsFromTpAction()
        {
            this.MappingDict = new Dictionary<string, MyClass>();

            this.MappingDict["MKDOG5O01T02"] = new MyClass("Form_5_6_2", null, "23:1");
            this.MappingDict["NMKDG05O17E02"] = new MyClass("Form_5_6_2", null, "1:1");
            this.MappingDict["NMKDG05O17E03"] = new MyClass("Form_5_6_2", null, "25:1");
            this.MappingDict["NMKDG05O17E07"] = new MyClass("Form_5_6_2", null, "2:1");
            this.MappingDict["NMKDG05O17E04"] = new MyClass("Form_5_6_2", null, "3:1");
            this.MappingDict["MKDOG4O06Е02"] = new MyClass("Form_1_3_3", "5:1", "2:1");
            this.MappingDict["MKDOG2O01E03"] = new MyClass("Form_5_1_1", null, "2:4");

            this.MappingDict["MKDOG1O08E01"] = new MyClass("Form_4_2_1", "10", "Form_4_2", "1:1", true);

            this.MappingDict["56"] = new MyClass("Form_3_3_3", "14:1", "13:1");
            this.MappingDict["57"] = new MyClass("Form_3_3_3", "14:1", "4:1");
            this.MappingDict["58"] = new MyClass("Form_3_3_3", "14:1", "5:1");

            this.MappingDict["MKDOG1O02E02"] = new MyClass("Form_3_1_3", "19:1", "4:1", "5:1");
            this.MappingDict["MKDOG1O02E01"] = new MyClass("Form_3_1_3", "19:1", "3:1");
            this.MappingDict["MKDOG1O14E08"] = new MyClass("Form_3_1_3", null, "17:1");

            this.MappingDict["MKDOG1O04E01"] = new MyClass("Form_3_2CW_3", "2:1", "1:1");
            this.MappingDict["MKDOG1O05E02"] = new MyClass("Form_3_2_3", "10:1", "4:1");
            this.MappingDict["MKDOG1O05E03"] = new MyClass("Form_3_2_3", "10:1", "5:1");
            this.MappingDict["MKDOG1O05E01"] = new MyClass("Form_3_2_3", "10:1", "3:1", "9:1");
            this.MappingDict["MKDOG1O06E01"] = new MyClass("Form_3_3_Water_2", "2:1", "1:1");
            this.MappingDict["MKDOG1O03E01"] = new MyClass("Form_3_4_2", "5:1", "1:1");

            this.MappingDict["MKDOG3O01T05"] = new MyClass("Form_5_8", "39:1", "25:1");
            this.MappingDict["MKDOG3O01T03"] = new MyClass("Form_5_8", "39:1", "26:1");
            this.MappingDict["MKDOG3O01T04"] = new MyClass("Form_5_8", "39:1", "27:1");
            this.MappingDict["MKDOG3O01T01"] = new MyClass("Form_5_8", "39:1", "23:1");
            this.MappingDict["MKDOG3O01T02"] = new MyClass("Form_5_8", "39:1", "24:1");
            this.MappingDict["MKDOG3O01T06"] = new MyClass("Form_5_8", "39:1", "28:1");
        }

        public override string Name => "Перенести сведения из тех.паспорта в конструктивные характеристики дома";

        public override string Description
            => @"Если атрибуты тех.паспорта дома имеют значение (не ноль), то добавлять в раздел Конструктивные характеристики запись. 
Атрибутам КЭ присваивать значения в соответствии с описанием (http://tfs-app.bars-open.ru:8080/tfs/DefaultCollection/GKH.GKH.CMMI/_workitems/edit/13300). 
Атрибутам, которых нет в описании присваивать 0.
Если такой КЭ уже есть в доме, то заменять значения атрибутов на новые.";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            /*
Шиферная (код MKDOG5O01T02)	            5.6. Крыша, кровля в поле "Крыша/в т.ч. шиферная скатная (кв.м.)"                               Form_5_6_2 (23:1)
Металлическая (код NMKDG05O17E02)       5.6. Крыша, кровля в поле "Площадь кровли металлической(кв.м)"  	                            Form_5_6_2 (1:1)
Сталь оцинкованная (NMKDG05O17E03)	    5.6. Крыша, кровля в поле "Крыша/в т.ч. иная скатная (кв.м.)"                   	            Form_5_6_2 (25:1)
Рубероид (бикрост) (код NMKDG05O17E07)	5.6. Крыша, кровля в поле "Площадь кровли из рулонных материалов(кв.м);"        	            Form_5_6_2 (2:1)
Черепица (NMKDG05O17E04)	            5.6. Крыша, кровля в поле "Площадь кровли из штучных материалов(кв.м);"                         Form_5_6_2 (3:1)

Полы эксплуатируемых подвалов, в том числе технических в которых имеются инженерные коммуникации (MKDOG4O06Е02) 
                                        1.3. Характеристика нежилых помещений/Площадь подвалов (кв.м);Год посл капремонта	            Form_1_3_3 (2:1 ; 5:1)
Прифундаментные отмостки (MKDOG2O01E03)	5.1. Фундаменты/Площадь отмостки	                                                            Form_5_1_1 (2:4)
Пассажирские лифтовые кабины (MKDOG1O08E01)	
                                        4.2. Общие сведения о лифтах/Количество лифтов(шт.); Год последней модернизации/капитально-восстановительного ремонта(год)-условие: приотсутствии года капремонта не переносим	
                                                                                                                                        Form_4_2 (1:1); Form_4_2_1 (*:10)
Внутренние сети (проводка/кабели) освещения эксплуатируемых помещений общего пользования (56)	
             *                          3.5. Электроснабжение/Длина сетей в местах общего пользования (м);Дата посл капремонта          Form_3_3_3 (13:1 ; 14:1	)
Сеть питания систем автоматической пожарной сигнализации внутреннего противопожарного водопровода (57)	
             *                          3.5. Электроснабжение/Длина сетей коммунального освещения(м); Дата посл капремонта	            Form_3_3_3 (4:1 ; 14:1)
Сети (проводка/кабели) питания лифтовых установок (58)	
             *                          3.5. Электроснабжение/Длина сетей питания лифтов и электронасосов(м);Дата посл капремонта	    Form_3_3_3 (5:1 ; 14:1	)
Стояки системы отопления (MKDOG1O02E02)	3.1. Отопление (теплоснабжение)/Длина стояков в квартирах (м)+Длина разводки в квартирах (м); Год последнего кап.ремонт системы	
             *                                                                                                                          Form_3_1_3 (4:1 + 5:1;  19:1)
Узлы управления ресурсами, с оборудованием устройств автоматизации и диспетчеризации для обеспечения дистанционного учёта и управления (MKDOG1O14E08)
             *                          3.1. Отопление (теплоснабжение)/система отопления /количество теплоцентров                      Form_3_1_3 (17:1)

Разводящие магистрали системы отопления (MKDOG1O02E01)	
             *                          3.1. Отопление (теплоснабжение)/Длина стояков в подвалах (м); Год последнего кап.ремонт системы	Form_3_1_3 (3:1;  19:1)
Разводящие магистрали трубопровода системы холодного водоснабжения (MKDOG1O04E01)	
             *                          3.3. Холодное водоснабжение (ХВС)/Длина трубопроводов (м); Год последнего кап.ремонта системы	Form_3_2CW_3 (1:1 ; 2:1)
Стояки горячего водоснабжения (MKDOG1O05E02)	
             *                          3.2. Горячее водоснабжение (ГВС)/Длина стояков в квартирах (м); год капремонта	                Form_3_2_3 (4:1 ; 10:1)
Ответвления от стояков до первого отключающего устройства, расположенного на ответвлениях от стояков (MKDOG1O05E03)	
             *                          3.2. Горячее водоснабжение (ГВС)/Длина разводки в квартирах (м); год капремонта	                Form_3_2_3 (5:1 ; 10:1)
Разводящие магистрали трубопровода горячего водоснабжения (MKDOG1O05E01)	
             *                          3.2. Горячее водоснабжение (ГВС)/Длина стояков в подвалах (м)+Длина трубопроводов (м); год капремонта Form_3_2_3 (3:1 + 9:1; 10:1)
Внутренний трубопровод канализации до первого отвода к сантехническим приборам (MKDOG1O06E01)	
             *                          3.4. Водоотведение (канализация)/Длина трубопроводов (м); Год капремонта	                    Form_3_3_Water_2 (1:1 ; 2:1)
Внутридомовые разводящие магистрали газопровода (MKDOG1O03E01)	
             *                          3.6. Газоснабжение/Длина сетей газоснабжения(м); Год последнего кап.ремонт системы	            Form_3_4_2 (1:1 ; 5:1)
Панельный фасад (MKDOG3O01T05)	        5.8. Фасады/в т.ч. панельная (кв.м.); Год последнего кап.ремонта стен	                        Form_5_8 (25:1 ; 39:1)
Фасад из плитки (MKDOG3O01T03)	        5.8. Фасады/в т.ч. облицованная плиткой (кв.м.); Год последнего кап.ремонта стен	        	Form_5_8 (26:1 ; 39:1)
Фасад из сайдинга (MKDOG3O01T04)	    5.8. Фасады/в т.ч. облицованная плиткой (кв.м.); Год последнего кап.ремонта стен	        	Form_5_8 (27:1 ; 39:1)
Оштукатуренный (MKDOG3O01T01)	        5.8. Фасады/в т.ч. оштукатуренная (кв.м.); Год последнего кап.ремонта стен	                	Form_5_8 (23:1 ; 39:1)
Неоштукатуренный (MKDOG3O01T02)	        5.8. Фасады/в т.ч. неоштукатуренная (кв.м.); Год последнего кап.ремонта стен	            	Form_5_8 (24:1 ; 39:1)
Деревянный (MKDOG3O01T06)	            5.8. Фасады/в т.ч. деревянная (кв.м.); Год последнего кап.ремонта стен	                    	Form_5_8 (28:1 ; 39:1)
             */

            var serviceStructuralElement = this.Container.Resolve<IDomainService<StructuralElement>>();

            var structuralElements = serviceStructuralElement.GetAll()
                .Select(x => new {x.Id, x.Code})
                .AsEnumerable()
                .Where(x => this.MappingDict.Keys.Contains(x.Code))
                .ToList();

            if (!structuralElements.Any())
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "Не найдены коды КЭ для данных, переносимых из техпаспорта"
                };
            }

            var duplicateCodes = structuralElements.GroupBy(x => x.Code).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

            if (duplicateCodes.Any())
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "Обнаружены дублирующие коды КЭ: " + string.Join(", ", duplicateCodes)
                };
            }

            var structuralElementDict = structuralElements.ToDictionary(x => x.Code, x => x.Id);

            var formCodes = this.MappingDict.Select(x => x.Value.yearForm)
                .Union(this.MappingDict.Select(x => x.Value.volumeForm))
                .Distinct()
                .ToList();

            var realtyObjectTpDataDict = this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                .Where(x => formCodes.Contains(x.FormCode))
                .Where(x => x.Value != null)
                .Where(x => x.Value.Trim() != "")
                .Where(x => x.Value.Trim() != "0")
                .Select(x => new {x.FormCode, x.CellCode, Value = x.Value.Trim(), roId = x.TehPassport.RealityObject.Id})
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var tpData = x.GroupBy(y => y.FormCode)
                            .ToDictionary(
                                y => y.Key,
                                y => y.GroupBy(z => z.CellCode).ToDictionary(z => z.Key, z => z.First().Value));

                        var structElements = this.MappingDict
                            .Select(
                                y =>
                                {
                                    var volume = 0m;
                                    if (tpData.ContainsKey(y.Value.volumeForm))
                                    {
                                        var formData = tpData[y.Value.volumeForm];
                                        y.Value.volumeCodes.Where(formData.ContainsKey).ForEach(v => volume += formData[v].ToDecimal());
                                    }

                                    var year = 0;
                                    if (!string.IsNullOrEmpty(y.Value.yearCode) && tpData.ContainsKey(y.Value.yearForm))
                                    {
                                        var formData = tpData[y.Value.yearForm];
                                        var yearStr = string.Empty;

                                        if (y.Value.multiple)
                                        {
                                            var years = formData.Where(z => z.Key.EndsWith(y.Value.yearCode)).Select(z => z.Value).ToList();

                                            if (years.Any())
                                            {
                                                yearStr = years.Max();
                                            }
                                        }
                                        else if (formData.ContainsKey(y.Value.yearCode))
                                        {
                                            yearStr = formData[y.Value.yearCode];
                                        }

                                        if (yearStr.Length == 4)
                                        {
                                            int intVal;
                                            if (int.TryParse(yearStr, out intVal))
                                            {
                                                year = intVal;
                                            }
                                        }
                                        else
                                        {
                                            DateTime date;
                                            if (DateTime.TryParse(yearStr, out date))
                                            {
                                                year = date.Year;
                                            }
                                        }
                                    }

                                    return new {y.Key, volume, year};
                                })
                            .Where(v => v.volume != 0 || v.year != 0)
                            .ToList();

                        return structElements;
                    });

            var serviceRealityObject = this.Container.Resolve<IDomainService<RealityObject>>();
            var serviceRealityObjectStructuralElement = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            var roStructuralElementsDict = serviceRealityObjectStructuralElement.GetAll().ToDictionary(x => x.Id);

            var realityObjectStructuralElementsDict = serviceRealityObjectStructuralElement.GetAll()
                .Where(x => x.RealityObject != null)
                .Select(x => new {roId = x.RealityObject.Id, x.StructuralElement.Code, x.Id})
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.Code).ToDictionary(y => y.Key, y => y.First().Id));

            const int OperationsPerTransaction = 100;
            var i = 0;
            var realtyObjectIdsList = realtyObjectTpDataDict.Keys.ToList();

            while (i < realtyObjectIdsList.Count)
            {
                var limit = (i + OperationsPerTransaction) < realtyObjectIdsList.Count
                    ? (i + OperationsPerTransaction)
                    : realtyObjectIdsList.Count;

                var res = this.InTransaction(
                    () =>
                    {
                        for (; i < limit; ++i)
                        {
                            var realtyObjectId = realtyObjectIdsList[i];
                            var realtyObjectTpData = realtyObjectTpDataDict[realtyObjectId];

                            if (realityObjectStructuralElementsDict.ContainsKey(realtyObjectId))
                            {
                                var roStructElemDict = realityObjectStructuralElementsDict[realtyObjectId];

                                foreach (var tpElement in realtyObjectTpData)
                                {
                                    var seCode = tpElement.Key;

                                    if (roStructElemDict.ContainsKey(seCode))
                                    {
                                        var realityObjectStructElement = roStructuralElementsDict[roStructElemDict[seCode]];

                                        var needToUpdate = false;

                                        if (tpElement.year != 0 && realityObjectStructElement.LastOverhaulYear != tpElement.year)
                                        {
                                            realityObjectStructElement.LastOverhaulYear = tpElement.year;
                                            needToUpdate = true;
                                        }

                                        if (tpElement.volume != 0 && realityObjectStructElement.Volume != tpElement.volume)
                                        {
                                            realityObjectStructElement.Volume = tpElement.volume;
                                            needToUpdate = true;
                                        }

                                        if (needToUpdate)
                                        {
                                            serviceRealityObjectStructuralElement.Save(realityObjectStructElement);
                                        }
                                    }
                                    else
                                    {
                                        if (structuralElementDict.ContainsKey(seCode))
                                        {
                                            var realityObjectConstructElement = new RealityObjectStructuralElement
                                            {
                                                RealityObject = serviceRealityObject.Load(realtyObjectId),
                                                StructuralElement = serviceStructuralElement.Load(structuralElementDict[seCode]),
                                                LastOverhaulYear = tpElement.year,
                                                Volume = tpElement.volume
                                            };

                                            serviceRealityObjectStructuralElement.Save(realityObjectConstructElement);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var tpElement in realtyObjectTpData)
                                {
                                    var seCode = tpElement.Key;

                                    if (!structuralElementDict.ContainsKey(seCode))
                                    {
                                        continue;
                                    }

                                    var realityObjectConstructElement = new RealityObjectStructuralElement
                                    {
                                        RealityObject = serviceRealityObject.Load(realtyObjectId),
                                        StructuralElement = serviceStructuralElement.Load(structuralElementDict[seCode]),
                                        LastOverhaulYear = tpElement.year,
                                        Volume = tpElement.volume
                                    };

                                    serviceRealityObjectStructuralElement.Save(realityObjectConstructElement);
                                }
                            }
                        }
                    });

                if (res.Success != true)
                {
                    return res;
                }
            }

            return new BaseDataResult {Success = true};
        }

        private BaseDataResult InTransaction(Action action)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
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

                        return new BaseDataResult
                        {
                            Success = false,
                            Message = exc.Message
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exc);
                    }
                }
            }

            return new BaseDataResult {Success = true};
        }

        private class MyClass
        {
            public readonly string volumeForm;

            public readonly string yearForm;

            public readonly ReadOnlyCollection<string> volumeCodes;

            public readonly string yearCode;

            public readonly bool multiple;

            public MyClass(string yearForm, string yearCode, string volumeForm, string volumeCode, bool multiple)
            {
                this.yearForm = yearForm;
                this.yearCode = yearCode;
                this.volumeForm = volumeForm;
                this.volumeCodes = new ReadOnlyCollection<string>(new List<string> {volumeCode});
                this.multiple = multiple;
            }

            public MyClass(string form, string yearCode, params string[] volumeCodes)
            {
                this.yearForm = form;
                this.yearCode = yearCode;
                this.volumeForm = form;
                this.volumeCodes = new ReadOnlyCollection<string>(volumeCodes);
            }
        }
    }
}