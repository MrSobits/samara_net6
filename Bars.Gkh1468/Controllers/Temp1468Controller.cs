namespace Bars.Gkh1468.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4.IoC;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;

    public class Temp1468Controller : BaseController
    {
        public ISessionProvider SessionProvider { get; set; }

        private MetaAttribute GetParent(MetaAttribute node)
        {
            if (node.Parent == null)
            {
                return null;
            }

            if (node.Parent.Type == MetaAttributeType.GroupedComplex)
            {
                return node.Parent;
            }

            return GetParent(node.Parent);
        }

        public ActionResult SetParentValueForPassportRows(BaseParams baseParams)
        {
            var from = baseParams.Params.GetAs<DateTime>("dateFrom");
            var to = baseParams.Params.GetAs<DateTime>("dateTo");

            RestoreProviderPassportRow<HouseProviderPassportRow, HouseProviderPassport>(from, to);
            RestoreProviderPassportRow<OkiProviderPassportRow, OkiProviderPassport>(from, to);

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Метод восстановления родителя для записей паспорта когда восстановить родителя уже 
        ///     невозможно - group_key не задан, а самих родительских групп более чем одна.
        /// В этом случае полям присваивается случайно попавшаяся группа. Решение спорное, но Настя обсуждала это
        ///     с Андреем Патроновым и вместо удаления таких "осиротевших" параметров было выбрано это решение
        /// [GKH-1227]
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult RestoreParentForNoGroupPassportRows(BaseParams baseParams)
        {
            var from = baseParams.Params.GetAs<DateTime>("dateFrom");
            var to = baseParams.Params.GetAs<DateTime>("dateTo");

            RestoreParentForNoGroupPassportRowsInternal(from, to);

            return JsonNetResult.Success;
        }

        private void RestoreProviderPassportRow<TRow, TProvider>(DateTime from, DateTime to)
            where TRow : BaseProviderPassportRow<TProvider>, new()
            where TProvider : BaseProviderPassport
        {
            var domain = Container.ResolveDomain<TRow>();
            try
            {
                var rows =
                        domain.GetAll()
                            .WhereIf(from > DateTime.MinValue, x => x.ProviderPassport.ObjectCreateDate >= from)
                            .WhereIf(to > DateTime.MinValue, x => x.ProviderPassport.ObjectCreateDate < to)
                            .Where(x => x.GroupKey > 0)
                            .ToList();

                var metaGroup = rows.GroupBy(x => GetParent(x.MetaAttribute));

                var multyMetaRows =
                    domain.GetAll()
                        .WhereIf(from > DateTime.MinValue, x => x.ProviderPassport.ObjectCreateDate >= from)
                        .WhereIf(to > DateTime.MinValue, x => x.ProviderPassport.ObjectCreateDate < to)
                        .Where(x => x.MetaAttribute.Type == MetaAttributeType.GroupedComplex)
                        .GroupBy(x => new { meta = x.MetaAttribute.Id, passport = x.Passport.Id, provider = x.ProviderPassport.Id, value = x.Value })
                        .ToDictionary(x => x.Key, x => x.First().Id);

                var valuesForSave = new List<TRow>(5000);

                using (var session = SessionProvider.OpenStatelessSession())
                {
                    foreach (var keyPair in metaGroup)
                    {
                        if (keyPair.Key != null && keyPair.Key.Type == MetaAttributeType.GroupedComplex)
                        {
                            var meta = keyPair.Key;

                            var providerGroup = keyPair.GroupBy(x => x.ProviderPassport);

                            foreach (var providerRows in providerGroup)
                            {
                                var provider = providerRows.Key;

                                var passportGroup = providerRows.GroupBy(x => x.Passport);

                                foreach (var passports in passportGroup)
                                {
                                    var passport = passports.Key;
                                    var groupKeyRowsGroup = passports.GroupBy(x => x.GroupKey);

                                    foreach (var row in groupKeyRowsGroup)
                                    {
                                        var parentValue = row.Key.ToStr();
                                        var parentId = multyMetaRows.Get(new { meta = meta.Id, passport = passport.Id, provider = provider.Id, value = parentValue });

                                        if (parentId == 0)
                                        {
                                            parentId =
                                                (long)
                                                session.Insert(
                                                    new TRow { MetaAttribute = meta, Passport = passport, ProviderPassport = provider, Value = parentValue, ObjectCreateDate = DateTime.Now });
                                        }

                                        foreach (var x in row)
                                        {
                                            if (x.ParentValue == parentId)
                                            {
                                                continue;
                                            }

                                            x.ParentValue = parentId;
                                            valuesForSave.Add(x);

                                            if (valuesForSave.Count < 5000)
                                            {
                                                continue;
                                            }

                                            TransactionHelper.InsertInManyTransactions(Container, valuesForSave, useStatelessSession: true);
                                            valuesForSave.Clear();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(Container, valuesForSave, useStatelessSession: true);
                }
            }
            finally
            {
                Container.Release(domain);
            }
        }

        private void RestoreParentForNoGroupPassportRowsInternal(DateTime from, DateTime to)
        {
            var housePassportRowDomain = Container.ResolveDomain<HouseProviderPassportRow>();
            var housePassportDomain = Container.ResolveDomain<HouseProviderPassport>();
            var metaAttributeDomain = Container.ResolveDomain<MetaAttribute>();
            using (Container.Using(housePassportRowDomain, metaAttributeDomain))
            {
                var orphanRows =
                    housePassportRowDomain.GetAll()
                            .WhereIf(from > DateTime.MinValue, x => x.ProviderPassport.ObjectCreateDate >= from)
                            .WhereIf(to > DateTime.MinValue, x => x.ProviderPassport.ObjectCreateDate < to)
                            .Where(x => x.GroupKey == 0)
                            .Where(x => !x.ParentValue.HasValue)
                            .Where(x => x.MetaAttribute.Parent != null
                                    && x.MetaAttribute.Parent.Type == MetaAttributeType.GroupedComplex)
                            .GroupBy(x => x.ProviderPassport.Id)
                            .ToDictionary(x => x.Key, y => y.Select(x => x))
                            .ToList();

                var dictNewParentPassportRow = new Dictionary<Tuple<long, long, DateTime>, HouseProviderPassportRow>();
                var listPassportRowsForUpdate = new List<HouseProviderPassportRow>();
                foreach (var orphanRow in orphanRows)
                {
                    var passportId = orphanRow.Key;
                    // Единственный способ разделить строки только группировкой по дате создания - группа атрибутов 
                    //  создается в одно время, жуткий костыль согласен.
                    var orphanPassportRows = orphanRow.Value
                        .GroupBy(x => new
                        {
                            x.ObjectCreateDate,
                            ParentMetaId = x.MetaAttribute.Parent.Id
                        })
                        .ToDictionary(x => x.Key, y => y.Select(x => x));

                    foreach (var orphanPassportRow in orphanPassportRows)
                    {
                        var now = DateTime.Now;
                        var newParentPassportRow = new HouseProviderPassportRow
                        {
                            ObjectCreateDate = now,
                            ObjectEditDate = now,
                            MetaAttribute = metaAttributeDomain.Get(orphanPassportRow.Key.ParentMetaId),
                            ProviderPassport = housePassportDomain.Get(passportId)
                        };

                        dictNewParentPassportRow.Add(
                            new Tuple<long, long, DateTime>(passportId, orphanPassportRow.Key.ParentMetaId, orphanPassportRow.Key.ObjectCreateDate),
                            newParentPassportRow);
                    }

                    listPassportRowsForUpdate.AddRange(orphanPassportRows.SelectMany(x => x.Value).ToList());
                }

                if (listPassportRowsForUpdate.Count <= 0)
                {
                    return;
                }

                using (var session = SessionProvider.OpenStatelessSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            foreach (var newPassportRow in dictNewParentPassportRow.Values)
                            {
                                newPassportRow.Id = (long)session.Insert(newPassportRow);
                            }

                            listPassportRowsForUpdate.ForEach(x =>
                            {
                                x.ParentValue = dictNewParentPassportRow[
                                    new Tuple<long, long, DateTime>(x.ProviderPassport.Id, x.MetaAttribute.Parent.Id,
                                        x.ObjectCreateDate)].Id;
                                session.Update(x);
                            });

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }
}