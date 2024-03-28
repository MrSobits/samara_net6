using System;
using System.Security.Cryptography.X509Certificates;
using Bars.B4.IoC;
using Bars.Gkh.Utils;
using Castle.MicroKernel.ModelBuilder.Descriptors;

namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    using Castle.Windsor;

    using Newtonsoft.Json.Linq;

    public class ViolationNormativeDocItemService : IViolationNormativeDocItemService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult UpdaeteViolationsByNpd(IQueryable<NormativeDocItem> npdQuery = null)
        {
            var violationsDomain = this.Container.ResolveDomain<ViolationGji>();
            var violationNpdDomain = this.Container.ResolveDomain<ViolationNormativeDocItemGji>();

            var listToSave = new List<ViolationGji>();

            var violNpdQuery = violationNpdDomain.GetAll()
                        .WhereIf(npdQuery != null, x => npdQuery.Any(y => y.Id == x.NormativeDocItem.Id));

            var violIds = violNpdQuery.Select(x => x.ViolationGji.Id).ToList();

            if (violIds.Any())
            {
                violIds = violIds.Distinct().ToList();
            }

            // получаем пункты толкьо по тем нарушениям по которым требуется провести обновление строк НПД
            var normativeDocDict = violationNpdDomain.GetAll()
                    .Where(x => violIds.Contains(x.ViolationGji.Id))
                    .Select(x => new
                    {
                        x.ViolationGji.Id,
                        x.NormativeDocItem.Number,
                        x.NormativeDocItem.NormativeDoc.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => string.Format("{0} {1}", x.Number, x.Name)).Distinct().AggregateWithSeparator(", "));

            var viols = violationsDomain.GetAll()
                            .Where(x => violIds.Contains(x.Id))
                            .ToList();

            foreach (var viol in viols)
            {
                var normDoc = normativeDocDict.ContainsKey(viol.Id) ? normativeDocDict[viol.Id] : string.Empty;

                if (normDoc != viol.NormativeDocNames)
                {
                    viol.NormativeDocNames = normDoc;

                    if (!string.IsNullOrEmpty(viol.NormativeDocNames) && viol.NormativeDocNames.Length > 2000)
                    {
                        viol.NormativeDocNames = viol.NormativeDocNames.Substring(0, 2000);
                    }

                    listToSave.Add(viol);
                }
            }

            if (listToSave.Any())
            {
                // тут Нельзя вставлять Транзакцию потому как данный метод используется в разных местах и даже в интерцепторах 
                //using (var transaction = this.Container.Resolve<IDataTransaction>())
                //{
                    try
                    {
                        foreach (var viol in listToSave)
                        {
                            violationsDomain.Update(viol);

                        }

                        //transaction.Commit();
                    }
                    catch
                    {
                        //transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        this.Container.Release(violationNpdDomain);
                        this.Container.Release(violationsDomain);
                    }
                //}
            }

            return new BaseDataResult();
        }

        public IDataResult ListTree(BaseParams baseParams)
        {
            var searchString = baseParams.Params.Get("workName", string.Empty).ToLower();

            var data = this.Container.ResolveDomain<NormativeDocItem>().GetAll()
                .WhereIf(searchString.IsNotEmpty(), x => x.Number.ToLower().Contains(searchString) || x.NormativeDoc.Name.ToLower().Contains(searchString))
                .ToArray()
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    NormativeDocId = x.NormativeDoc.Id,
                    NormativeDocName = x.NormativeDoc.Name
                })
                .GroupBy(x => x.NormativeDocName)
                .ToDictionary(
                    x => x.Key, 
                    x => x.GroupBy(y => y.Number)
                        .Select(y => y.First())
                        .Select(y => new WorkTreeNode
                        {
                            Id = y.Id,
                            Name = y.Number
                        }));

            var tree = this.ConvertDictToTree(data);

            return new BaseDataResult(tree["children"])
            {
                Success = true
            };
        }

        public IDataResult SaveNormativeDocItems(BaseParams baseParams)
        {
            try
            {
                var violationId = baseParams.Params.ContainsKey("violationId") ? baseParams.Params["violationId"].ToLong() : 0;
                var items = baseParams.Params.GetAs<List<ItemProxy>>("items");

                var domain = Container.Resolve<IDomainService<ViolationNormativeDocItemGji>>();
                var normDocDomain = Container.Resolve<IDomainService<NormativeDocItem>>();

                var existingNPDItems = domain.GetAll()
                .Where(x => x.ViolationGji.Id == violationId)
                .Select(x => x.NormativeDocItem.Id).ToList();

                var listToSave = new List<ViolationNormativeDocItemGji>();

                using (Container.Using(domain, normDocDomain))
                {
                    foreach (var item in items)
                    {
                        if (!existingNPDItems.Contains(item.NormDocItemId))
                        {
                            listToSave.Add(new ViolationNormativeDocItemGji
                            {
                                NormativeDocItem = new NormativeDocItem { Id = item.NormDocItemId },
                                ViolationGji = new ViolationGji { Id = violationId },
                                ViolationStructure = item.ViolStruct
                            });
                        }
                    }
                }

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        // удаляем старые пункты 
                  //      DeleteExistingItems(domain, violationId);

                        // сохраняем новые
                        foreach (var item in listToSave)
                        {
                            domain.Save(item);
                        }

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        throw;
                    }
                }

                var itemsIds = items.Select(x => x.NormDocItemId).ToArray();

                if (itemsIds.Any())
                {
                    var service = Container.Resolve<IViolationNormativeDocItemService>();
                    var npdDomain = Container.Resolve<IDomainService<NormativeDocItem>>();

                    try
                    {
                        service.UpdaeteViolationsByNpd(npdDomain.GetAll().Where(x => itemsIds.Contains(x.Id)));
                    }
                    finally
                    {
                        Container.Release(service);
                        Container.Release(npdDomain);
                    }    
                }
                else
                {
                    var violDomain = Container.Resolve<IDomainService<ViolationGji>>();

                    try
                    {
                        var viol = violDomain.GetAll().FirstOrDefault(x => x.Id == violationId);
                        if (viol != null)
                        {
                            viol.NormativeDocNames = null;
                            violDomain.Update(viol);
                        }
                    }
                    finally
                    {
                        Container.Release(violDomain);
                    }  
                }
                

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
        }

        private void DeleteExistingItems(IDomainService<ViolationNormativeDocItemGji> domain, long violationId)
        {
            var existingItems = domain.GetAll()
                .Where(x => x.ViolationGji.Id == violationId)
                .Select(x => new
                {
                    x.Id
                }).ToList();

            foreach (var item in existingItems)
            {
                domain.Delete(item.Id);
            }
        }

        private void SaveNewItems(IDomainService<ViolationNormativeDocItemGji> domain, long violationId, IEnumerable<long> itemsIds)
        {
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var itemId in itemsIds)
                    {
                        domain.Save(new ViolationNormativeDocItemGji
                        {
                            NormativeDocItem = new NormativeDocItem
                            {
                                Id = itemId
                            },
                            ViolationGji = new ViolationGji
                            {
                                Id = violationId
                            }
                        });
                    }

                    tr.Commit();
                }
                catch (Exception exc)
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// конвертация словаря в дерево
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private JObject ConvertDictToTree(Dictionary<string, IEnumerable<WorkTreeNode>> dict)
        {
            var root = new JObject();

            var groups = new JArray();

            foreach (var pair in dict)
            {
                var group = new JObject();

                group["id"] = pair.Key;
                group["text"] = pair.Key;

                var children = new JArray();

                foreach (var rec in pair.Value)
                {
                    var leaf = new JObject();

                    leaf["id"] = rec.Id;
                    leaf["text"] = rec.Name;
                    leaf["leaf"] = true;
                    leaf["checked"] = false;

                    children.Add(leaf);
                }

                group["children"] = children;

                groups.Add(group);
            }

            root["children"] = groups;

            return root;
        }

        /// <summary>
        /// Вспомогательный класс
        /// </summary>
        private class WorkTreeNode
        {
            public long Id;
            public string Name;
        }

        /// <summary>
        /// Вспомогательный класс
        /// </summary>
        public class ItemProxy
        {
            public long NormDocItemId { get; set; }
            public string ViolStruct { get; set; }
        }
    }
}