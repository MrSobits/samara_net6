namespace Bars.Gkh1468.Controllers.Passport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh1468.DomainService.Passport;
    using Bars.Gkh1468.Entities;

    public class MetaAttributeController : B4.Alt.DataController<MetaAttribute>
    {
        public override ActionResult Delete(BaseParams baseParams)
        {
            var attrId = baseParams.Params.Get("id", (long)0);
            var service = Container.Resolve<IDomainService<MetaAttribute>>();

            var result = Container.Resolve<IMetaAttributeService>().RemoveMetaAttribute(attrId, service);
            return new JsonNetResult(result);
        }

        /// <summary>
        /// Сохранить новый порядок метаатрибутов. В том числе структуру дерева.
        /// </summary>
        /// <param name="baseParams">
        /// Параметры в которых передается ИД атрибута, новый ИД родителя и новый порядок атрибута.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult SaveNewAttributeOrder(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<MetaAttribute>>();
            var result = new BaseDataResult();

            try
            {
                var attrebutesForSave = new List<MetaAttribute>();
                var records = baseParams.Params.GetAs<AttributeProxy[]>("records");

                // выбираем все ИД атрибутов, чтобы вытащить их из БД
                var attributeIds = records.Select(x => x.Id)
                    .Union(records.Select(x => x.ParentId))
                    .Distinct().ToList();

                var metaAttributes = service.GetAll()
                    .Where(x => attributeIds.Contains(x.Id))
                    .Where(x => x.Id != 0) // исключаем 0 на всякий случай, потому что 0 - это признак отсутствия родителя
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                foreach (var proxy in records)
                {
                    var item = metaAttributes.Get(proxy.Id);
                    var parentItem = metaAttributes.Get(proxy.ParentId);

                    if (item != null)
                    {
                        item.Parent = proxy.ParentId == 0 ? null : parentItem;
                        item.OrderNum = proxy.OrderNum;

                        attrebutesForSave.Add(item);
                    }
                }

                Container.InTransaction(() =>
                        {
                            foreach (var metaAttribute in attrebutesForSave)
                            {
                                service.Update(metaAttribute);
                            }
                        });
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                Container.Release(service);
            }

            return new JsonNetResult(result);
        }

        private MetaAttribute GetMetaAttributeObjectFromData(object data)
        {
            return ((IEnumerable<MetaAttribute>)data.GetType()
                    .GetProperty("data")
                    .GetValue(data, null))
                .FirstOrDefault();
        }

        /// <summary>
        /// Прокси класс метаатрибута для получения из клиентской части как параметра.
        /// </summary>
        public sealed class AttributeProxy
        {
            /// <summary>
            /// ИД метаатрибута.
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// ИД родительского метаатрибута.
            /// </summary>
            public long ParentId { get; set; }

            /// <summary>
            /// Порядок атрибута
            /// </summary>
            public int OrderNum { get; set; }
        }
    }
}