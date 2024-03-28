namespace Bars.Gkh1468.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;
    using Bars.Gkh1468.ProxyModels;

    public abstract class BaseProviderPassportRowViewModel<T> : BaseViewModel<T> where T : BaseProviderPassportRow
    {
        public abstract IDomainService DomainServiceProviderPassport { get; }

        public IDomainService<MetaAttribute> DomainServiceMetaAttribute { get; set; }

        public IDomainService<Part> DomainServicePart { get; set; }

        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var isPart = baseParams.Params["isPart"].ToBool();
            var partId = baseParams.Params["partId"].ToLong();
            var metaId = baseParams.Params["metaId"].ToLong();

            var providerPassportId = baseParams.Params["providerPassportId"].ToLong();
            var providerPassport = DomainServiceProviderPassport.Get(providerPassportId) as BaseProviderPassport;

            if (providerPassport == null)
            {
                return new ListDataResult(null, 0);
            }

            Element root = null;
            if (metaId == 0)
            {
                root = GetRoot(isPart, providerPassport.PassportStruct.Id, partId);
            }
            else
            {
                var meta = DomainServiceMetaAttribute.Get(metaId);
                    
                if (meta == null)
                {
                    throw new Exception("Не удалось найти структуру раздела.");
                }

                root = new Element
                {
                    Attribute = meta,
                };
            }
            
            if (!isPart && partId > 0)
            {
                var metaAttributes = DomainServiceMetaAttribute.GetAll()
                .Where(x => x.ParentPart.Struct.Id == providerPassport.PassportStruct.Id)
                .Where(x => x.ParentPart.Id == partId)
                .ToArray();

                AddRootAttributes(root, metaAttributes, false);
            }

            Sort(root, new ElementComparer());

            //var values = GetValuesForMeta(root, GetValues(domainService, providerPassportId, partId));
         
            //return new ListDataResult(new { meta = root, data = values }, root.Childrens.Return(x => x.Count));
            return new ListDataResult(new { meta = root }, root.Childrens.Return(x => x.Count));
        }

        private IList<ElementValue> GetValuesForMeta(Element root, IQueryable<T> values)
        {
            var attributes = CollectAttributes(root).Select(x => x.Id).ToList();
            return values.Where(x => attributes.Contains(x.MetaAttribute.Id))
                .Select(x => new ElementValue
                {
                    MetaId = x.MetaAttribute.Id,
                    //GroupKey = x.GroupKey,
                    Value = x.Value,
                    ValueId = x.Id
                }).ToList();
        }

        private IEnumerable<MetaAttribute> CollectAttributes(Element root)
        {
            var attributes = new List<MetaAttribute>();
            if (root.Attribute != null && 
                (root.Attribute.Type == MetaAttributeType.Simple ||
                root.Attribute.Type == MetaAttributeType.GroupedWithValue))
            {
                attributes.Add(root.Attribute);
            }

            if (root.Childrens != null)
            {
                foreach (var child in root.Childrens)
                {
                    attributes.AddRange(CollectAttributes(child));    
                }
            }
            return attributes;
        }

        protected abstract IQueryable<T> GetValues(IDomainService<T> domainService, long providerPassportId, long partId);

        /// <summary>Отсортировать элементы</summary>
        /// <param name="element">Родительский элемент</param>
        /// <param name="comparer">Реализация IComparer</param>
        protected virtual void Sort(Element element, IComparer<Element> comparer)
        {
            element.Childrens.Sort(comparer);
            foreach (var children in element.Childrens)
            {
                Sort(children, comparer);
            }
        }

        /// <summary>Получить атрибуты</summary>
        /// <param name="parent">Родитель</param>
        /// <param name="metaAttributes">Массив атрибутов</param>
        /// <param name="excludeComplex"></param>
        protected virtual void AddRootAttributes(Element parent, MetaAttribute[] metaAttributes, bool excludeComplex)
        {
            var attributes = metaAttributes
                    .WhereIf(parent.Part != null, x => x.ParentPart.Id == parent.Part.Id && x.Parent == null)
                    .WhereIf(parent.Part == null, x => x.Parent != null && x.Parent.Id == parent.Attribute.Id)
                    .ToArray();

            foreach (var attribute in attributes)
            {
                var children = new Element { Attribute = attribute };
                parent.Childrens.Add(children);
                if (attribute.Type != MetaAttributeType.GroupedComplex)
                {
                    AddRootAttributes(children, metaAttributes, excludeComplex);
                }
            }
        }

        /// <summary>Получить заполненный корневой элемент</summary>
        /// <param name="allParts">Все разделы</param>
        /// <param name="passportStructId">Идентификатор структуры паспорта</param>
        /// <param name="partId">Идентификатор раздела</param>
        /// <returns>Корневой элемент</returns>
        public virtual Element GetRoot(bool allParts, long passportStructId, long partId)
        {
            var root = new Element();
            if (allParts)
            {
                var parts = DomainServicePart.GetAll()
                    .Where(x => x.Struct.Id == passportStructId)
                    .ToArray();

                var mainParts = parts
                    .Where(x => x.Parent == null)
                    .OrderBy(x => x.SortNumber)
                    .ToList();

                var subParts = parts
                    .Where(x => x.Parent != null)
                    .OrderBy(x => x.SortNumber)
                    .ToList();

                foreach (var part in mainParts)
                {
                    var children = new Element { Part = part };
                    foreach (var subPart in subParts.Where(x => x.Parent.Id == part.Id).ToArray())
                    {
                        children.Childrens.Add(new Element { Part = subPart });
                        subParts.Remove(subPart);
                    }

                    root.Childrens.Add(children);
                }
            }
            else
            {
                root.Part = DomainServicePart.Get(partId);
            }

            return root;
        }
    }
}