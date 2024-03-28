namespace Bars.Gkh1468.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.StimulReport;
    using Bars.Gkh.Utils;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;

    using Castle.Windsor;

    public abstract class BasePassportReport : StimulReport, IBaseReport
    {
        private ElementComparer comparer
        {
            get
            {
                return new ElementComparer();
            }
        }

        public IWindsorContainer Container { get; set; }

        public string Name
        {
            get { return "Отчет"; }
        }
        
        public Stream GetTemplate()
        {
#if DEBUG
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../Bars.Gkh1468/resources/Report.mrt");
            if (File.Exists(path))
            {
                return new ReportTemplateBinary(File.ReadAllBytes(path)).GetTemplate();
            }
#endif
            return new ReportTemplateBinary(Properties.Resources.Report).GetTemplate();
        }

        public void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["ЗаголовокОтчета"] = GetTitle();

            var passports = GetProviderPassports();
            var values = GetProviderPassportsRows(passports);

            this.DataSources.Add(new MetaData
            {
                SourceName = "Организации",
                MetaType = nameof(ReportOrganization),
                Data = GetOrganizations(passports)
            });

            var structId = passports.Any() ? passports.First().PassportStruct.Id : 0;
            var metaAttributes = Container.Resolve<IDomainService<MetaAttribute>>().GetAll()
                .Where(x => x.ParentPart.Struct.Id == structId)
                .ToArray();

            var root = new ElementPart();
            foreach (var passport in passports)
            {
                AddParts(root, passport, metaAttributes);
                
                foreach (var part in root.SubParts)
                {
                    AddAttributes(part, passport, metaAttributes, values);
                    
                    foreach (var child in part.SubParts)
                    {
                        AddAttributes(child, passport, metaAttributes, values);
                    }
                }
            }

            var data = new List<ReportData>();

            // сортируем изначальные самые крупные разделы
            root.SubParts.Sort(comparer);

            for (var i = 0; i < root.SubParts.Count; i++)
            {
                var part = root.SubParts[i];
                AddPassportElement(part, i + 1, data);
            }

            // нужна логически правильная сортировка (1,2,3,10,11,... а не 1,10,11,2,3,...)
            data = data
                .OrderBy(x => x.РазделНомер)
                .ThenBy(x => x.Организация)
                .ThenBy(x => (x.Код ?? string.Empty).Trim(), new NumericComparer())
                .ToList();

            // В отчете не сортируем заново, а просто выводим строки по порядку
            var j = 0;
            data.ForEach(x => x.Порядок = j++);

            this.DataSources.Add(new MetaData
            {
                SourceName = "Данные",
                MetaType = nameof(ReportData),
                Data = data
            });
        }

        protected abstract string GetTitle();

        protected virtual ReportOrganization[] GetOrganizations(BaseProviderPassport[] passports)
        {
            return passports.Where(x => x.Contragent != null).Select(x => x.Contragent).Distinct()
                .Select(x => new ReportOrganization
                {
                    Наименование = x.Name,
                    ОГРН = x.Ogrn,
                    ИНН = x.Inn,
                    КПП = x.Kpp
                })
                .ToArray();
        }

        protected abstract BaseProviderPassport[] GetProviderPassports();

        protected abstract BaseProviderPassportRow[] GetProviderPassportsRows(BaseProviderPassport[] passports);

        /// <summary>Отсортировать элементы</summary>
        /// <param name="element">Родительский элемент</param>
        /// <param name="comparer">Реализация IComparer</param>
        protected virtual void SortAttribute(ElementAttribute element, IComparer<ElementAttribute> comparer)
        {
            element.Children.Sort(comparer);
            foreach (var children in element.Children)
            {
                SortAttribute(children, comparer);
            }
        }

        /// <summary>Добавить разделы в корневой элемент</summary>
        /// <param name="root">Корневой элемент</param>
        /// <param name="passport">Паспорт</param>
        /// <param name="metaAttributes">Массив атрибутов</param>
        protected virtual void AddParts(ElementPart root, BaseProviderPassport passport, MetaAttribute[] metaAttributes)
        {
            var parts = metaAttributes
                .Select(x => x.ParentPart)
                .Where(x => x.Parent == null)
                .Distinct()
                .OrderBy(x => x.OrderNum)
                .ToList();

            var subParts = metaAttributes
                .Select(x => x.ParentPart)
                .Where(x => x.Parent != null)
                .Distinct()
                .ToList();

            foreach (var part in parts)
            {
                var child = new ElementPart { Passport = passport, Part = part };
                foreach (var subPart in subParts.Where(x => x.Parent.Id == part.Id).ToArray())
                {
                    child.SubParts.Add(new ElementPart { Passport = passport, Part = subPart });
                    subParts.Remove(subPart);
                }

                root.SubParts.Add(child);
            }

            /* Обробатываем оставшиеся подразделы, могут остаться если у разделов нет аттрибутов */
            foreach (var part in subParts)
            {
                var child = root.SubParts.FirstOrDefault(x => x.Part.Id == part.Parent.Id);

                if (child == null)
                {
                    child = new ElementPart { Passport = passport, Part = part.Parent };
                    root.SubParts.Add(child);
                }

                child.SubParts.Add(new ElementPart { Passport = passport, Part = part });
            }
        }

        /// <summary>Добавить атрибуты</summary>
        /// <param name="element">Родитель</param>
        /// <param name="passport">Паспорт</param>
        /// <param name="metaAttributes">Массив атрибутов</param>
        /// <param name="values">Массив значений</param>
        protected virtual void AddAttributes(ElementPart element, BaseProviderPassport passport, MetaAttribute[] metaAttributes, BaseProviderPassportRow[] values)
        {
            var attributes = metaAttributes.Where(x => x.ParentPart.Id == element.Part.Id && x.Parent == null).ToArray();
            
            foreach (var attribute in attributes)
            {
                if (attribute.Type == MetaAttributeType.GroupedComplex)
                {
                    var valueList = values
                        .Where(x => x.MetaAttribute.Id == attribute.Id && x.Passport.Id == passport.Id)
                        .OrderBy(x => x.Value)
                        .ToList();

                    foreach (var value in valueList)
                    {
                        var child = new ElementAttribute
                        {
                            ElementPart = element,
                            Attribute = attribute,
                            Value = value == null ? null : value.Value,
                            Id = value == null ? 0 : value.Id
                        };
                        element.Attributes.Add(child);

                        AddAttributes(child, metaAttributes, values);
                    }
                }
                else
                {
                    var value = values
                        .FirstOrDefault(x => x.MetaAttribute.Id == attribute.Id && x.Passport.Id == passport.Id);

                    var elementAttribute = new ElementAttribute
                    {
                        ElementPart = element,
                        Attribute = attribute,
                        Value = value == null ? null : value.Value
                    };
                    element.Attributes.Add(elementAttribute);

                    AddAttributes(elementAttribute, metaAttributes, values);
                }
            }
        }

        /// <summary>Добавить атрибуты</summary>
        /// <param name="element">Родитель</param>
        /// <param name="metaAttributes">Массив атрибутов</param>
        /// <param name="values">Массив значений</param>
        protected virtual void AddAttributes(ElementAttribute element, MetaAttribute[] metaAttributes, BaseProviderPassportRow[] values)
        {
            var attributes = metaAttributes.Where(x => x.Parent != null && x.Parent.Id == element.Attribute.Id).ToArray();

            foreach (var attribute in attributes)
            {
                if (attribute.Type == MetaAttributeType.GroupedComplex)
                {
                    var valueList = values
                        .Where(x => x.MetaAttribute.Id == attribute.Id && x.Passport.Id == element.ElementPart.Passport.Id)
                        .WhereIf(element.Id > 0, x => x.ParentValue.HasValue && x.ParentValue.Value == element.Id)
                        .OrderBy(x => x.Value)
                        .ToList();

                    foreach (var value in valueList)
                    {
                        var child = new ElementAttribute
                        {
                            ElementPart = element.ElementPart,
                            Attribute = attribute,
                            Value = value == null ? null : value.Value,
                            Id = value == null ? 0 : value.Id
                        };
                        element.Children.Add(child);

                        AddAttributes(child, metaAttributes, values);
                    }
                }
                else
                {
                    var value = values.Where(x => x.MetaAttribute.Id == attribute.Id && x.Passport.Id == element.ElementPart.Passport.Id)
                        .WhereIf(element.Id > 0, x => x.ParentValue.HasValue && x.ParentValue.Value == element.Id)
                        .FirstOrDefault();

                    var child = new ElementAttribute
                    {
                        ElementPart = element.ElementPart,
                        Attribute = attribute,
                        Value = value == null ? null : value.Value,
                        Id = value == null ? 0 : value.Id
                    };
                    element.Children.Add(child);

                    AddAttributes(child, metaAttributes, values);
                }
            }
        }

        protected void AddPassportElement(ElementPart element, decimal partNumber, List<ReportData> data)
        {
            // сортировка выполняется для каждой группы элементов по мере надобности 
            element.Attributes.Sort(comparer);
            foreach (var attribute in element.Attributes)
            {
                AddPassportElement(attribute, partNumber, data);
                
                if(attribute.Children.Any())
                {
                    AddPassportElement(attribute.Children, partNumber, data);
                } 
            }

            foreach (var subPart in element.SubParts)
            {
                AddPassportElement(subPart, partNumber + 0.1M, data);
            }
        }
        
        /// <summary>
        /// Добавляет в массив дочерние элементы атрибутов и дочерние дочерних(рекурсия до нужной глубины)
        /// </summary>
        /// <param name="children">массив дочерних элементов</param>
        /// <param name="partNumber"></param>
        /// <param name="data">массив данных для будущей пдф-ки</param>
        protected void AddPassportElement(List<ElementAttribute> children, decimal partNumber, List<ReportData> data)
        {
            children.Sort(comparer);
            
            foreach (var child in children)
            {
                AddPassportElement(child, partNumber, data); 
                
                if (child.Children.Any())
                {
                    child.Children.Sort(comparer);
                    AddPassportElement(child.Children, partNumber, data);
                }
            }
        }

        protected void AddPassportElement(ElementAttribute element, decimal partNumber, List<ReportData> data)
        {
            data.Add(new ReportData
            {
                Раздел = element.ElementPart.Part.Name,
                РазделНомер = element.ElementPart.Part.Code.ToDecimal(),
                Организация = element.ElementPart.Passport.Contragent != null ? element.ElementPart.Passport.Contragent.Name : "",
                Номер = element.Attribute.SortNumber.ToString(),
                Наименование = element.Attribute.Name,
                ЕдиницаИзмерения = element.Attribute.UnitMeasure.Return(x => x.Name),
                Информация = element.Value,
                Серый = element.Attribute.Type != MetaAttributeType.Simple,
                Группа = element.Attribute.Type == MetaAttributeType.GroupedWithValue,
                Код = element.Attribute.Code
            });
        }

        protected string GetMonthName(int month)
        {
            string result = null;
            switch (month)
            {
                case 1:
                    result = "январь";
                    break;
                case 2:
                    result = "февраль";
                    break;
                case 3:
                    result = "март";
                    break;
                case 4:
                    result = "апрель";
                    break;
                case 5:
                    result = "май";
                    break;
                case 6:
                    result = "июнь";
                    break;
                case 7:
                    result = "июль";
                    break;
                case 8:
                    result = "август";
                    break;
                case 9:
                    result = "сентябрь";
                    break;
                case 10:
                    result = "октябрь";
                    break;
                case 11:
                    result = "ноябрь";
                    break;
                case 12:
                    result = "декабрь";
                    break;
            }

            return result;
        }

        protected class ReportOrganization
        {
            public string Наименование { get; set; }

            public string ОГРН { get; set; }

            public string КПП { get; set; }

            public string ИНН { get; set; }

            public string ФИО { get; set; }
        }

        protected class ReportData
        {
            public string Раздел { get; set; }

            public decimal РазделНомер { get; set; }

            public string Организация { get; set; }

            public string Номер { get; set; }

            public string Наименование { get; set; }

            public string ЕдиницаИзмерения { get; set; }

            public string Информация { get; set; }

            public bool Серый { get; set; }

            public bool Группа { get; set; }

            public string Код { get; set; }

            public int Порядок { get; set; }
        }

        protected class ElementPart
        {
            public ElementPart()
            {
                SubParts = new List<ElementPart>();
                Attributes = new List<ElementAttribute>();
            }

            public BaseProviderPassport Passport { get; set; }

            public Part Part { get; set; }

            public List<ElementPart> SubParts { get; set; }

            public List<ElementAttribute> Attributes { get; set; }

            public override string ToString()
            {
                return string.Format("Part: {0}, Passport: {1}", Part.Return(x => x.Name), Passport.Return(x => x.Id));
            }
        }

        protected class ElementAttribute
        {
            public ElementAttribute()
            {
                Children = new List<ElementAttribute>();
            }

            public long Id { get; set; }

            public ElementPart ElementPart { get; set; }

            public MetaAttribute Attribute { get; set; }

            public string Value { get; set; }

            public List<ElementAttribute> Children { get; set; }

            public override string ToString()
            {
                return string.Format(
                    "ElementPart: {0}, MetaAttribute: {1}, Value: {2}",
                    ElementPart.Return(x => x.Part.Return(y => y.Name)), 
                    Attribute.Return(x => x.Name),
                    Value);
            }
        }

        // сортировки сортируют по номерам если их нет сортируют по именам в алфавитном порядке
        protected class ElementComparer : IComparer<ElementPart>, IComparer<ElementAttribute>, IComparer<ReportData>
        {
            private int result = 0;

            public int Compare(ElementPart x, ElementPart y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                if (x.Part.SortNumber < y.Part.SortNumber)
                {
                    return -1;
                }
                
                if (x.Part.SortNumber > y.Part.SortNumber)
                {
                    return 1;
                }

                if (x.Part.SortNumber == y.Part.SortNumber)
                {
                    result = string.Compare(x.Part.Name, y.Part.Name, StringComparison.Ordinal);

                    if (result == 0)
                    {
                        return 0;
                    }

                    if (result < 0)
                    {
                        return -1;
                    }
                    
                    if (result > 0)
                    {
                        return 1;
                    }
                }

                // если не отработало ни одно условие то какая-то фигня произошла 
                // вернём ноль на всякий случай
                return 0;
            }

            public int Compare(ElementAttribute x, ElementAttribute y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                if (x.Attribute.SortNumber < y.Attribute.SortNumber)
                {
                    return -1;
                }
                
                if (x.Attribute.SortNumber > y.Attribute.SortNumber)
                {
                    return 1;
                }

                if (x.Attribute.SortNumber == y.Attribute.SortNumber)
                {
                    result = string.Compare(x.Attribute.Name, y.Attribute.Name, System.StringComparison.Ordinal);
                    
                    if (result == 0)
                    {
                        return 0;
                    }
                    
                    if (result < 0)
                    {
                        return -1;
                    }
                    
                    if (result > 0)
                    {
                        return 1;
                    }
                }

                // если не отработало ни одно условие то какая-то фигня произошла 
                // вернём ноль на всякий случай
                return 0;
            }

            public int Compare(ReportData x, ReportData y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                if (x.РазделНомер < y.РазделНомер)
                {
                    return -1;
                }
                
                if (x.РазделНомер > y.РазделНомер)
                {
                    return 1;
                }

                if (x.РазделНомер == y.РазделНомер)
                {
                    result = string.Compare(x.Организация, y.Организация, StringComparison.Ordinal);

                    if (result == 0)
                    {
                        result = string.Compare(x.Номер, y.Номер, StringComparison.Ordinal);
                        
                        if (result == 0)
                        {
                            return 0;
                        }

                        if (result < 0)
                        {
                            return -1;
                        }

                        if (result > 0)
                        {
                            return 1;
                        }
                    }

                    if (result < 0)
                    {
                        return -1;
                    }

                    if (result > 0)
                    {
                        return 1;
                    }
                }

                // если не отработало ни одно условие то какая-то фигня произошла 
                // вернём ноль на всякий случай
                return 0;
            }
        }
    }
}
