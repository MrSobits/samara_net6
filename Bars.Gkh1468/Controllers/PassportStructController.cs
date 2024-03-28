using System;
using System.IO;
using NHibernate.Util;

namespace Bars.Gkh1468.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.DataAccess;

    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;
    using Bars.Gkh1468.PassportStructExport;

    using Newtonsoft.Json;

    public class PassportStructController : B4.Alt.DataController<PassportStruct>
    {
        public IDomainService<PassportStruct> PassportStructDomain { get; set; }

        public IDomainService<BaseProviderPassport> BaseProviderPassportDomain { get; set; }

        public IDomainService<Part> PartDomain { get; set; }

        public IDomainService<MetaAttribute> MetaAttributeDomain { get; set; }

        public override ActionResult Delete(BaseParams baseParams)
        {
            var passId = baseParams.Params.GetAs<long>("id");

            var domain = Container.ResolveDomain<PassportStruct>();

            var passHaveDependency = Container.Resolve<IModuleDependencies>("Bars.Gkh1468 dependencies")
                    .CheckAnyDependencies<PassportStruct>(passId);
            if (passHaveDependency)
            {
                return new JsonNetResult(
                    new { success = false, message = "На удаляемую структуру паспорта ссылаются другие таблицы" });
            }

            domain.Delete(passId);

            return JsSuccess();
        }

        //public ActionResult Get(int id)
        //{
        //    var domain = Container.ResolveDomain<PassportStruct>();
        //    var pStruct = domain.Get(id);
        //    return new JsonGetResult(pStruct);
        //}

        public ActionResult GetParts(BaseParams baseParams)
        {
            var passportStructId = baseParams.Params.GetAs<long>("structId");

            var parts = PartDomain.GetAll()
                .Where(x => x.Struct.Id == passportStructId)
                .ToArray();

            var root = new ElementPart();
            foreach (var part in parts.Where(x => x.Parent == null))
            {
                var children = new ElementPart(part);
                children.Childrens.AddRange(parts.Where(x => x.Parent != null && x.Parent.Id == part.Id).Select(x => new ElementPart(x)));
                root.Childrens.Add(children);
            }

            var comparer = new ElementOrderNumComparer();
            root.Childrens.Sort(comparer);
            foreach (var children in root.Childrens)
            {
                children.Childrens.Sort(comparer);
            }

            return new JsonNetResult(root);
        }

        public ActionResult GetAttributes(BaseParams baseParams)
        {
            var partId = baseParams.Params.GetAs<long>("partId");

            var metaAttributes = MetaAttributeDomain.GetAll()
                .Where(x => x.ParentPart.Id == partId)
                .ToArray();

            var root = new ElementAttribute();
            foreach (var metaAttribute in metaAttributes.Where(x => x.Parent == null))
            {
                var children = new ElementAttribute(metaAttribute);
                root.Childrens.Add(children);
                AddAttributes(children, metaAttributes);
            }

            SortAttributes(root, new ElementOrderNumComparer());

            return new JsonNetResult(root);
        }

        public ActionResult CopyPassportStruct(BaseParams baseParams)
        {
            var passportStructId = baseParams.Params.GetAs<long>("passportStructId");
            var name = baseParams.Params.GetAs<string>("Name");
            var month = baseParams.Params.GetAs<int>("ValidFromMonth");
            var year = baseParams.Params.GetAs<int>("ValidFromYear");

            var passStruct = PassportStructDomain.GetAll().First(x => x.Id == passportStructId);

            // Проверяем период действия для заданного типа структуры
            if (PassportStructDomain.GetAll().Any(x => x.PassportType == passStruct.PassportType
                                                       && x.ValidFromYear == year
                                                       && x.ValidFromMonth == month))
            {
                return JsFailure("В системе уже существует структура этого типа с аналогичным периодом действия");
            }

            // Проверяем период действия паспортов для заданного типа структуры
            if (BaseProviderPassportDomain.GetAll().Any(x => x.PassportStruct.PassportType == passStruct.PassportType
                                                             && x.ReportYear >= year
                                                             && x.ReportMonth >= month))
            {
                return JsFailure("В системе уже существует паспорт структуры этого типа за этот период");
            }

            var newPassStruct = new PassportStruct
                {
                    Id = passportStructId,
                    Name = name,
                    ValidFromMonth = month,
                    ValidFromYear = year,
                    PassportType = passStruct.PassportType
                };


            var exporter = Container.Resolve<IPassportStructExporter>();
            var expStream = (Stream)exporter.Export(newPassStruct).Data;
            var result = exporter.Import(expStream);
            
            if (result.Success)
            {
                return JsSuccess(result.Data);
            }
            return JsFailure("При копировании структуры паспорта произошла ошибка");
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var structId = baseParams.Params.GetAs<long>("structId", ignoreCase: true);
            var exporter = Container.Resolve<IPassportStructExporter>();
            var passportStruct = Container.ResolveDomain<PassportStruct>().FirstOrDefault(x => x.Id == structId);
            return File((Stream)exporter.Export(passportStruct).Data, "application/json", string.Format("{0}.json", passportStruct.Name));
        }

        public ActionResult Import(BaseParams baseParams)
        {
            var exporter = Container.Resolve<IPassportStructExporter>();
            var fileStream = new MemoryStream(baseParams.Files["importfile"].Data);
            var result = exporter.Import(fileStream);
            if (result.Success)
            {
                return JsSuccess(result.Data);
            }
            return JsFailure(result.Message);
        }

        private void AddAttributes(ElementAttribute parent, MetaAttribute[] metaAttributes)
        {
            var attributes = metaAttributes.Where(x => x.Parent != null && x.Parent.Id == parent.Id).ToArray();
            foreach (var attribute in attributes)
            {
                var children = new ElementAttribute(attribute);
                parent.Childrens.Add(children);

                AddAttributes(children, metaAttributes);
            }
        }

        private void SortAttributes(ElementAttribute element, IComparer<ElementAttribute> comparer)
        {
            element.Childrens.Sort(comparer);
            foreach (var children in element.Childrens)
            {
                SortAttributes(children, comparer);
            }
        }

        private void CreatePart(Part part, PassportStruct newPassStruct, Dictionary<long, long> dictPartMeets)
        {
            if (part.Parent != null)
            {
                CreatePart(part.Parent, newPassStruct, dictPartMeets);
            }

            // Значит раздел структуры уже добавлен ранее
            if (dictPartMeets.ContainsKey(part.Id))
            {
                return;
            }

            var newPart = part.Clone();
            newPart.Struct = newPassStruct;

            if (part.Parent != null)
            {
                newPart.Parent = PartDomain.GetAll().First(x => x.Id == dictPartMeets[part.Parent.Id]);
            }

            PartDomain.Save(newPart);
            // добавляем соотвествие копируемого раздела структуры к новому разделу, чтобы потом найти родителя записи
            dictPartMeets.Add(part.Id, newPart.Id);

            var dictAttributeMeets = new Dictionary<long, long>();
            MetaAttributeDomain.GetAll().Where(x => x.ParentPart == part).ForEach(x => CreateAttribute(x, newPart, dictAttributeMeets));
        }

        private void CreateAttribute(MetaAttribute attribute, Part newPart, Dictionary<long, long> dictAttributeMeets)
        {
            if (attribute.Parent != null)
            {
                CreateAttribute(attribute.Parent, newPart, dictAttributeMeets);
            }

            // Значит атрибут уже добавлен ранее
            if (dictAttributeMeets.ContainsKey(attribute.Id))
            {
                return;
            }

            var newAttribute = attribute.Clone();
            newAttribute.ParentPart = newPart;

            if (attribute.Parent != null)
            {
                newAttribute.Parent =
                    MetaAttributeDomain.GetAll().First(x => x.Id == dictAttributeMeets[attribute.Parent.Id]);
            }

            MetaAttributeDomain.Save(newAttribute);
            // добавляем соотвествие копируемого атрибута к новому атрибуту, чтобы потом найти родителя записи
            dictAttributeMeets.Add(attribute.Id, newAttribute.Id);
        }

        private class ElementPart
        {
            public ElementPart()
            {
                Childrens = new List<ElementPart>();
            }

            public ElementPart(Part part)
                : this()
            {
                Id = part.Id;
                Code = part.Code;
                Name = part.Name;
                Parent = part.Parent;
                OrderNum = part.OrderNum;
                Uo = part.Uo;
                Pku = part.Pku;
                Pr = part.Pr;
                IntegrationCode = part.IntegrationCode;
            }

            /// <summary>Идентификатор</summary>
            public long Id { get; set; }

            /// <summary>Номер для сортировки, берется последнее целое число из свойства Code</summary>
            [JsonIgnore]
            public virtual int SortNumber
            {
                get { return string.IsNullOrEmpty(Code) ? 0 : Code.Replace(',', '.').TrimEnd('.').Split('.').Last().ToInt(); }
            }

            /// <summary>Код раздела</summary>
            public string Code { get; set; }

            /// <summary>Наименование раздела</summary>
            public string Name { get; set; }

            /// <summary>Порядковый номер (считается автоматически)</summary>
            public int OrderNum { get; set; }

            /// <summary>Родительский раздел</summary>
            public Part Parent { get; set; }

            /// <summary>Заполняется УО</summary>
            public bool Uo { get; set; }

            /// <summary>Заполняется ПКУ</summary>
            public bool Pku { get; set; }

            /// <summary>Заполняется ПР</summary>
            public bool Pr { get; set; }

            /// <summary>Код интеграции</summary>
            public string IntegrationCode { get; set; }

            public List<ElementPart> Childrens { get; set; }
        }

        private class ElementAttribute
        {
            public ElementAttribute()
            {
                Childrens = new List<ElementAttribute>();
            }

            public ElementAttribute(MetaAttribute metaAttribute)
                : this()
            {
                Id = metaAttribute.Id;
                Code = metaAttribute.Code;
                Name = metaAttribute.Name;
                Parent = metaAttribute.Parent;
                OrderNum = metaAttribute.OrderNum;
                Type = metaAttribute.Type;
                ValueType = metaAttribute.ValueType;
                DictCode = metaAttribute.DictCode;
                ParentPart = metaAttribute.ParentPart;
                ValidateChilds = metaAttribute.ValidateChilds;
                GroupText = metaAttribute.GroupText;
                UnitMeasure = metaAttribute.UnitMeasure;
                IntegrationCode = metaAttribute.IntegrationCode;
                MaxLength = metaAttribute.MaxLength;
                MinLength = metaAttribute.MinLength;
                Pattern = metaAttribute.Pattern;
                Exp = metaAttribute.Exp;
                Required = metaAttribute.Required;
                AllowNegative = metaAttribute.AllowNegative;
                DataFillerCode = metaAttribute.DataFillerCode;
                UseInPercentCalculation = metaAttribute.UseInPercentCalculation;
            }

            /// <summary>Идентификатор</summary>
            public long Id { get; set; }

            /// <summary>
            /// Номер для сортировки, берется порядок атрибута, если он больше нуля, иначе последнее целое число из свойства Code
            /// </summary>
            /// <remarks>
            /// Порядок атрибута изменяется при перетаскивании атрибутов мышкой
            /// </remarks>
            [JsonIgnore]
            public virtual int SortNumber
            {
                get
                {
                    if (this.OrderNum > 0)
                    {
                        return this.OrderNum;
                    }

                    return string.IsNullOrEmpty(this.Code) ? 0 : this.Code.Replace(',', '.').TrimEnd('.').Split('.').Last().ToInt();
                }
            }

            /// <summary>Код атрибута</summary>
            public string Code { get; set; }

            /// <summary>Наименование</summary>
            public string Name { get; set; }

            /// <summary>Тип атрибута</summary>
            public MetaAttributeType Type { get; set; }

            /// <summary>Тип хранимого значения</summary>
            public Enums.ValueType ValueType { get; set; }

            /// <summary>Тип хранимого значения</summary>
            public string DictCode { get; set; }

            /// <summary>Родительский раздел</summary>
            public Part ParentPart { get; set; }

            /// <summary>Родительский атрибут</summary>
            public MetaAttribute Parent { get; set; }

            /// <summary>Проверять соответсвие суммы значений дочерних атрибутов (для групповых со значением)</summary>
            public bool ValidateChilds { get; set; }

            /// <summary>Текст используемый при группировке (для групповых)</summary>
            public string GroupText { get; set; }

            /// <summary>Единица измерения</summary>
            public UnitMeasure UnitMeasure { get; set; }

            /// <summary>Код интеграции</summary>
            public string IntegrationCode { get; set; }

            /// <summary>Порядковый номер (считается автоматически)</summary>
            public int OrderNum { get; set; }

            /// <summary>Системное значение(код)</summary>
            public string DataFillerCode { get; set; }

            /// <summary>
            /// Использовать при расчете процента заполнения
            /// </summary>
            public bool UseInPercentCalculation { get; set; }

            /// <summary>
            /// Поля используемые при валидации
            /// </summary>
            public int MaxLength { get; set; }
            public int MinLength { get; set; }
            public string Pattern { get; set; }
            public int Exp { get; set; }
            public bool Required { get; set; }
            public bool AllowNegative { get; set; }

            public List<ElementAttribute> Childrens { get; set; }
        }

        private class ElementOrderNumComparer : IComparer<ElementPart>, IComparer<ElementAttribute>
        {
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

                var compareNum = x.SortNumber.CompareTo(y.SortNumber);
                var compareName = x.Name.CompareTo(y.Name);

                return compareNum == 0 ? compareName : compareNum;
            }

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

                var compareNum = x.SortNumber.CompareTo(y.SortNumber);
                var compareName = x.Name.CompareTo(y.Name);

                return compareNum == 0 ? compareName : compareNum;
            }
        }

        private class ElementComparer : IComparer<ElementPart>, IComparer<ElementAttribute>
        {
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

                return x.SortNumber.CompareTo(y.SortNumber);
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

                return x.SortNumber.CompareTo(y.SortNumber);
            }
        }
    }
}
