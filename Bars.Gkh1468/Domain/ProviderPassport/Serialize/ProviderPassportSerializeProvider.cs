namespace Bars.Gkh1468.Domain.ProviderPassport.Serialize
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.Gkh1468.Entities;

    using Castle.Windsor;

    /// <summary>Провайдер для формирования Xml по паспорту</summary>
    public class ProviderPassportSerializeProvider
    {
        private readonly IDomainService<MetaAttribute> domainServiceMetaAttribute;

        private readonly IDomainService<HouseProviderPassportRow> houseDomainServiceRow;
        private readonly IDomainService<OkiProviderPassportRow> okiDomainServiceRow;

        public ProviderPassportSerializeProvider(IWindsorContainer container)
        {
            domainServiceMetaAttribute = container.Resolve<IDomainService<MetaAttribute>>();
            houseDomainServiceRow = container.Resolve<IDomainService<HouseProviderPassportRow>>();
            okiDomainServiceRow = container.Resolve<IDomainService<OkiProviderPassportRow>>();
        }

        /// <summary>Получить Xml</summary>
        /// <param name="houseProviderPassport">Паспорт поставщика дома</param>
        /// <returns>Xml</returns>
        public Stream GetStreamXml(HouseProviderPassport houseProviderPassport)
        {
            var values = houseDomainServiceRow.GetAll()
                .Where(x => x.ProviderPassport.Id == houseProviderPassport.Id)
                .Cast<BaseProviderPassportRow>()
                .ToArray();

            return StreamXml(houseProviderPassport, values);
        }

        /// <summary>Получить Xml</summary>
        /// <param name="okiProviderPassport">Паспорт поставщика дома</param>
        /// <returns>Xml</returns>
        public Stream GetStreamXml(OkiProviderPassport okiProviderPassport)
        {
            var values = okiDomainServiceRow.GetAll()
                .Where(x => x.ProviderPassport.Id == okiProviderPassport.Id)
                .Cast<BaseProviderPassportRow>()
                .ToArray();

            return StreamXml(okiProviderPassport, values);
        }

        private Stream StreamXml(BaseProviderPassport houseProviderPassport, BaseProviderPassportRow[] values)
        {
            var metaAttributes = domainServiceMetaAttribute.GetAll()
                .Where(x => x.ParentPart.Struct.Id == houseProviderPassport.PassportStruct.Id)
                .ToArray();

            var passport = GetPassport(houseProviderPassport, metaAttributes);

            /* Получаем атрибуты, только для на один уровень вниз, тут не нужен рекурсивный вызов */
            foreach (var element in passport.Elements)
            {
                AddAttributes(element, metaAttributes, values);
                foreach (var children in element.Elements)
                {
                    AddAttributes(children, metaAttributes, values);
                }
            }

            foreach (var element in passport.Elements)
            {
                Sort(element, new PassportElementComparer());
            }

            var serializer = new XmlSerializer(typeof (Passport));

            var result = new MemoryStream();
            var writer = XmlWriter.Create(result);

            serializer.Serialize(writer, passport);

            result.Seek(0, SeekOrigin.Begin);

            return result;
        }

        /// <summary>Отсортировать элементы</summary>
        /// <param name="element">Родительский элемент</param>
        /// <param name="comparer">Реализация IComparer</param>
        protected virtual void Sort(PassportElement element, IComparer<PassportElement> comparer)
        {
            element.Elements.Sort(comparer);
            foreach (var children in element.Elements)
            {
                Sort(children, comparer);
            }
        }

        /// <summary>Получить атрибуты</summary>
        /// <param name="parent">Родитель</param>
        /// <param name="metaAttributes">Массив атрибутов</param>
        /// <param name="values">Массив значений</param>
        protected virtual void AddAttributes(PassportElement parent, MetaAttribute[] metaAttributes, BaseProviderPassportRow[] values)
        {
            /* Обработка элемонтов для Part и Attribute происходит по разному */
            if (parent.Part != null)
            {
                var attributes = metaAttributes.Where(x => x.ParentPart.Id == parent.Part.Id && x.Parent == null).ToArray();
                foreach (var attribute in attributes)
                {
                    var value = values.FirstOrDefault(y => y.MetaAttribute.Id == attribute.Id);
                    var children = new PassportElement
                    {
                        Attribute = attribute,
                        Type = PassportElementType.Attribute,
                        Code = attribute.Code,
                        Name = attribute.Name,
                        Value = value == null ? null : value.Value
                    };
                    parent.Elements.Add(children);

                    AddAttributes(children, metaAttributes, values);
                }
            }
            else
            {
                var attributes = metaAttributes.Where(x => x.Parent != null && x.Parent.Id == parent.Attribute.Id).ToArray();
                foreach (var attribute in attributes)
                {
                    var value = values.FirstOrDefault(y => y.MetaAttribute.Id == attribute.Id);
                    var children = new PassportElement
                    {
                        Attribute = attribute,
                        Type = PassportElementType.Attribute,
                        Code = attribute.Code,
                        Name = attribute.Name,
                        Value = value == null ? null : value.Value
                    };
                    parent.Elements.Add(children);

                    AddAttributes(children, metaAttributes, values);
                }
            }
        }

        /// <summary>Получить паспорт</summary>
        /// <param name="providerPassport">Базовый паспорт</param>
        /// <param name="metaAttributes">Массив атрибутов</param>
        /// <returns>Паспорт</returns>
        protected virtual Passport GetPassport(BaseProviderPassport providerPassport, MetaAttribute[] metaAttributes)
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
             
            var passport = new Passport
            {
                Elements = new List<PassportElement>(),
                ReportYear = providerPassport.ReportYear,
                State = providerPassport.State == null ? null : providerPassport.State.Name,
                Percent = providerPassport.Percent,
                Contragent = providerPassport.Contragent == null ? null : new PassportContragent
                {
                    ContragentType = providerPassport.ContragentType,
                    Name = providerPassport.Contragent.Name,
                    Inn = providerPassport.Contragent.Inn,
                    Kpp = providerPassport.Contragent.Kpp,
                    Ogrn = providerPassport.Contragent.Ogrn
                }
            };

            foreach (var part in parts)
            {
                var element = new PassportElement
                {
                    Part = part,
                    Code = part.Code,
                    Name = part.Name,
                    Type = PassportElementType.Part
                };
                foreach (var subPart in subParts.Where(x => x.Parent.Id == part.Id).ToArray())
                {
                    element.Elements.Add(new PassportElement
                    {
                        Part = subPart,
                        Code = subPart.Code,
                        Name = subPart.Name,
                        Type = PassportElementType.Part
                    });
                    subParts.Remove(subPart);
                }

                passport.Elements.Add(element);
            }

            /* Обробатываем оставшиеся подразделы, т.к. могут остаться в случае, если у разделов нет атрибутов */
            foreach (var part in subParts)
            {
                var children = passport.Elements.FirstOrDefault(x => x.Part.Id == part.Parent.Id)
                    ?? new PassportElement
                    {
                        Part = part.Parent,
                        Code = part.Parent.Code,
                        Name = part.Parent.Name,
                        Type = PassportElementType.Part
                    };
                children.Elements.Add(new PassportElement
                {
                    Part = part,
                    Code = part.Code,
                    Name = part.Name,
                    Type = PassportElementType.Part
                });
                passport.Elements.Add(children);
            }

            return passport;
        }
    }
}