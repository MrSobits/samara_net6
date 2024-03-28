namespace Bars.GkhGji.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    using NHibernate.Linq;

    public class NormativeDocInitializationAction : BaseExecutionAction
    {
        /// <summary>
        /// Статический код регистрации.
        /// </summary>
        /// <summary>
        /// IoC контейнер.
        /// </summary>
        /// <summary>
        /// Код для регистрации.
        /// </summary>
        /// <summary>
        /// Описание действия.
        /// </summary>
        public override string Description => "Добавить нормативные документы и пункты на основе нарушений";

        /// <summary>
        /// Название для отображения.
        /// </summary>
        public override string Name => "Добавить нормативные документы и пункты на основе нарушений";

        /// <summary>
        /// Действие.
        /// </summary>
        public override Func<IDataResult> Action => this.MakeDocs;

        /// <summary>
        /// Метод действия.
        /// </summary>
        /// <returns>
        /// The <see cref="BaseDataResult" />.
        /// </returns>
        public BaseDataResult MakeDocs()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var docsDomain = this.Container.ResolveDomain<NormativeDoc>();
                var itemsDomain = this.Container.ResolveDomain<NormativeDocItem>();
                var violationsDomain = this.Container.ResolveDomain<ViolationGji>();
                var violationNdiDomain = this.Container.ResolveDomain<ViolationNormativeDocItemGji>();

                this.CleanData();

                var dictionaryNormaDocs = docsDomain.GetAll()
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, y => y.First());

                try
                {
                    var docPprf170 = new NormativeDoc
                    {
                        Category = NormativeDocCategory.HousingSupervision,
                        Code = 100,
                        Name = "ПП РФ №170"
                    };
                    var docPprf25 = new NormativeDoc
                    {
                        Category = NormativeDocCategory.HousingSupervision,
                        Code = 101,
                        Name = "ПП РФ №25"
                    };
                    var docPprf307 = new NormativeDoc
                    {
                        Category = NormativeDocCategory.HousingSupervision,
                        Code = 102,
                        Name = "ПП РФ №307"
                    };
                    var docPprf491 = new NormativeDoc
                    {
                        Category = NormativeDocCategory.HousingSupervision,
                        Code = 103,
                        Name = "ПП РФ №491"
                    };
                    var docOthers = new NormativeDoc
                    {
                        Category = NormativeDocCategory.HousingSupervision,
                        Code = 104,
                        Name = "Прочие нормативные документы"
                    };
                    var docJkrf = new NormativeDoc
                    {
                        Category = NormativeDocCategory.HousingSupervision,
                        Code = 105,
                        Name = "ЖК РФ"
                    };

                    this.GetEntityOrSave(dictionaryNormaDocs, docsDomain, ref docPprf170);
                    this.GetEntityOrSave(dictionaryNormaDocs, docsDomain, ref docPprf170);
                    this.GetEntityOrSave(dictionaryNormaDocs, docsDomain, ref docPprf25);
                    this.GetEntityOrSave(dictionaryNormaDocs, docsDomain, ref docPprf307);
                    this.GetEntityOrSave(dictionaryNormaDocs, docsDomain, ref docPprf491);
                    this.GetEntityOrSave(dictionaryNormaDocs, docsDomain, ref docOthers);
                    this.GetEntityOrSave(dictionaryNormaDocs, docsDomain, ref docJkrf);

                    var index = 106;

                    var violationsCodePin = violationsDomain.GetAll()
                        .Select(x => new {x.Id, x.CodePin})
                        .ToArray()
                        .Where(x => !string.IsNullOrWhiteSpace(x.CodePin))
                        .GroupBy(x => x.CodePin);
                    foreach (var violation in violationsCodePin)
                    {
                        NormativeDocItem item;
                        if (violation.Key.ToLower().Contains("пин"))
                        {
                            item = new NormativeDocItem
                            {
                                NormativeDoc = docPprf170,
                                Number = violation.Key
                            };
                            itemsDomain.Save(item);
                        }
                        else
                        {
                            var docItem = new NormativeDoc
                            {
                                Category = NormativeDocCategory.HousingSupervision,
                                Code = index,
                                Name = violation.Key
                            };
                            this.GetEntityOrSave(dictionaryNormaDocs, docsDomain, ref docItem);

                            item = new NormativeDocItem
                            {
                                NormativeDoc = docItem,
                                Number = violation.Key
                            };
                            itemsDomain.Save(item);

                            index++;
                        }

                        foreach (var violationItem in violation)
                        {
                            violationNdiDomain.Save(
                                new ViolationNormativeDocItemGji
                                {
                                    NormativeDocItem = item,
                                    ViolationGji = new ViolationGji
                                    {
                                        Id = violationItem.Id
                                    }
                                });
                        }
                    }

                    var violationsGkRf = violationsDomain.GetAll()
                        .Select(x => new {x.Id, x.GkRf})
                        .ToArray()
                        .Where(x => !string.IsNullOrWhiteSpace(x.GkRf))
                        .GroupBy(x => x.GkRf);
                    foreach (var violation in violationsGkRf)
                    {
                        var item = new NormativeDocItem
                        {
                            NormativeDoc = docJkrf,
                            Number = violation.Key
                        };
                        itemsDomain.Save(item);

                        foreach (var violationItem in violation)
                        {
                            violationNdiDomain.Save(
                                new ViolationNormativeDocItemGji
                                {
                                    NormativeDocItem = item,
                                    ViolationGji = new ViolationGji
                                    {
                                        Id = violationItem.Id
                                    }
                                });
                        }
                    }

                    var violationsOtherNormDocs = violationsDomain.GetAll()
                        .Select(x => new {x.Id, x.OtherNormativeDocs})
                        .ToArray()
                        .Where(x => !string.IsNullOrWhiteSpace(x.OtherNormativeDocs))
                        .GroupBy(x => x.OtherNormativeDocs);
                    foreach (var violation in violationsOtherNormDocs)
                    {
                        var item = new NormativeDocItem
                        {
                            NormativeDoc = docOthers,
                            Number = violation.Key
                        };
                        itemsDomain.Save(item);

                        foreach (var violationItem in violation)
                        {
                            violationNdiDomain.Save(
                                new ViolationNormativeDocItemGji
                                {
                                    NormativeDocItem = item,
                                    ViolationGji = new ViolationGji
                                    {
                                        Id = violationItem.Id
                                    }
                                });
                        }
                    }

                    var violationsPpRf170 = violationsDomain.GetAll()
                        .Select(x => new {x.Id, x.PpRf170})
                        .ToArray()
                        .Where(x => !string.IsNullOrWhiteSpace(x.PpRf170))
                        .GroupBy(x => x.PpRf170);
                    foreach (var violation in violationsPpRf170)
                    {
                        var item = new NormativeDocItem
                        {
                            NormativeDoc = docPprf170,
                            Number = violation.Key
                        };
                        itemsDomain.Save(item);

                        foreach (var violationItem in violation)
                        {
                            violationNdiDomain.Save(
                                new ViolationNormativeDocItemGji
                                {
                                    NormativeDocItem = item,
                                    ViolationGji = new ViolationGji
                                    {
                                        Id = violationItem.Id
                                    }
                                });
                        }
                    }

                    var violationsPpRf25 = violationsDomain.GetAll()
                        .Select(x => new {x.Id, x.PpRf25})
                        .ToArray()
                        .Where(x => !string.IsNullOrWhiteSpace(x.PpRf25))
                        .GroupBy(x => x.PpRf25);
                    foreach (var violation in violationsPpRf25)
                    {
                        var item = new NormativeDocItem
                        {
                            NormativeDoc = docPprf25,
                            Number = violation.Key
                        };
                        itemsDomain.Save(item);

                        foreach (var violationItem in violation)
                        {
                            violationNdiDomain.Save(
                                new ViolationNormativeDocItemGji
                                {
                                    NormativeDocItem = item,
                                    ViolationGji = new ViolationGji
                                    {
                                        Id = violationItem.Id
                                    }
                                });
                        }
                    }

                    var violationsPpRf307 = violationsDomain.GetAll()
                        .Select(x => new {x.Id, x.PpRf307})
                        .ToArray()
                        .Where(x => !string.IsNullOrWhiteSpace(x.PpRf307))
                        .GroupBy(x => x.PpRf307);
                    foreach (var violation in violationsPpRf307)
                    {
                        var item = new NormativeDocItem
                        {
                            NormativeDoc = docPprf307,
                            Number = violation.Key
                        };
                        itemsDomain.Save(item);

                        foreach (var violationItem in violation)
                        {
                            violationNdiDomain.Save(
                                new ViolationNormativeDocItemGji
                                {
                                    NormativeDocItem = item,
                                    ViolationGji = new ViolationGji
                                    {
                                        Id = violationItem.Id
                                    }
                                });
                        }
                    }

                    var violationsPpRf491 = violationsDomain.GetAll()
                        .Select(x => new {x.Id, x.PpRf491})
                        .ToArray()
                        .Where(x => !string.IsNullOrWhiteSpace(x.PpRf491))
                        .GroupBy(x => x.PpRf491);
                    foreach (var violation in violationsPpRf491)
                    {
                        var item = new NormativeDocItem
                        {
                            NormativeDoc = docPprf491,
                            Number = violation.Key
                        };
                        itemsDomain.Save(item);

                        foreach (var violationItem in violation)
                        {
                            violationNdiDomain.Save(
                                new ViolationNormativeDocItemGji
                                {
                                    NormativeDocItem = item,
                                    ViolationGji = new ViolationGji
                                    {
                                        Id = violationItem.Id
                                    }
                                });
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(docsDomain);
                    this.Container.Release(itemsDomain);
                    this.Container.Release(violationsDomain);
                    this.Container.Release(violationNdiDomain);
                }
            }

            return new BaseDataResult();
        }

        private void CleanData()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var itemsDomain = this.Container.ResolveDomain<NormativeDocItem>();
                var violationNdiDomain = this.Container.ResolveDomain<ViolationNormativeDocItemGji>();

                try
                {
                    var violationNdiIds = violationNdiDomain.GetAll()
                        .Fetch(x => x.NormativeDocItem)
                        .ThenFetch(x => x.NormativeDoc)
                        .Select(x => x.Id).Distinct().ToList();

                    foreach (var id in violationNdiIds)
                    {
                        violationNdiDomain.Delete(id);
                    }

                    var docitemIds = itemsDomain.GetAll()
                        .Fetch(x => x.NormativeDoc)
                        .Where(x => x.NormativeDoc != null && x.NormativeDoc.Category == NormativeDocCategory.HousingSupervision)
                        .Select(x => x.Id).Distinct().ToList();

                    foreach (var id in docitemIds)
                    {
                        itemsDomain.Delete(id);
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(itemsDomain);
                    this.Container.Release(violationNdiDomain);
                }
            }
        }

        private void GetEntityOrSave(Dictionary<int, NormativeDoc> normativeDocs, IDomainService<NormativeDoc> domain, ref NormativeDoc doc)
        {
            if (normativeDocs.ContainsKey(doc.Code) && normativeDocs[doc.Code] != null)
            {
                doc = normativeDocs[doc.Code];
            }
            else
            {
                domain.Save(doc);
            }
        }
    }
}