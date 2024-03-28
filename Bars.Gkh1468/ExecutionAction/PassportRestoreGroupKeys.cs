namespace Bars.Gkh1468.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;

    using NHibernate;

    public class PassportRestoreGroupKeys : BaseExecutionAction
    {
        public override string Description
            => "Восстановление ключей группировки групповых множественных полей паспортов 1468 домов и ОКИ, которые были утеряны " +
                "в связи с ошибкой при создании паспортов на основе существующих из предыдущего периода.";

        public override string Name => "Восстановление ключей группировки паспортов 1468";

        public override Func<IDataResult> Action => this.RestoreGroupKeys;

        public BaseDataResult RestoreGroupKeys()
        {
            var houseProvidePassportRowService = this.Container.Resolve<IDomainService<HouseProviderPassportRow>>();
            var okiProvidePassportRowService = this.Container.Resolve<IDomainService<OkiProviderPassportRow>>();

            //ДОМА
            //найти все аттрибуты, у которых родители групповые множественные
            var data = houseProvidePassportRowService.GetAll()
                .Where(x => x.MetaAttribute.Parent.Type == MetaAttributeType.GroupedComplex)
                .Select(
                    x => new
                    {
                        MetaAttributeId = x.MetaAttribute.Id,
                        MetaAttributeParentId = x.MetaAttribute.Parent.Id,
                        RealityObjectId = x.ProviderPassport.RealityObject.Id,
                        ContragentId = x.ProviderPassport.Contragent.Id,
                        PassportRow = x
                    })
                .ToArray();

            // с нулевым ключом группировки (возможно неверно скопированные)  (1)
            var rowDataByRo = data.Where(x => x.PassportRow.GroupKey == 0)
                .GroupBy(x => new {x.RealityObjectId, x.ContragentId})
                .ToDictionary(x => x.Key, x => x.ToList());

            // с ненулевым ключом группировки (исходные данные, использованные для копирвания в (1) )
            var baseRowsByRo = data.Where(x => x.PassportRow.GroupKey != 0)
                .GroupBy(x => new {x.RealityObjectId, x.ContragentId})
                .ToDictionary(x => x.Key, x => x.ToList());

            var listToUpdateHouseProviderPassportRow = new List<HouseProviderPassportRow>();

            foreach (var pair in rowDataByRo)
            {
                if (!baseRowsByRo.ContainsKey(pair.Key))
                {
                    continue;
                }

                var rows = pair.Value;
                var baseRows = baseRowsByRo[pair.Key];

                //восстановить групповой ключ элемента из базового
                foreach (var row in rows)
                {
                    var baseRow = baseRows
                        .Where(x => x.MetaAttributeId == row.MetaAttributeId)
                        .Where(x => x.MetaAttributeParentId == row.MetaAttributeParentId)
                        .Where(x => x.PassportRow.Value == row.PassportRow.Value)
                        .FirstOrDefault();

                    if (baseRow != null)
                    {
                        row.PassportRow.GroupKey = baseRow.PassportRow.GroupKey;

                        listToUpdateHouseProviderPassportRow.Add(row.PassportRow);
                    }
                }
            }

            //ОКИ
            //найти все аттрибуты, у которых родители групповые множественные
            var dataOki = okiProvidePassportRowService.GetAll()
                .Where(x => x.MetaAttribute.Parent.Type == MetaAttributeType.GroupedComplex)
                .Select(
                    x => new
                    {
                        MetaAttributeId = x.MetaAttribute.Id,
                        MetaAttributeParentId = x.MetaAttribute.Parent.Id,
                        MunicipalityId = x.ProviderPassport.Municipality.Id,
                        ContragentId = x.ProviderPassport.Contragent.Id,
                        PassportRow = x
                    })
                .ToArray();

            // с нулевым ключом группировки (возможно неверно скопированные)  (1)
            var rowDataOkiByMu = dataOki.Where(x => x.PassportRow.GroupKey == 0)
                .GroupBy(x => new {x.MunicipalityId, x.ContragentId})
                .ToDictionary(x => x.Key, x => x.ToList());

            // с ненулевым ключом группировки (исходные данные, использованные для копирвания в (1) )
            var baseRowsOkiByMu = dataOki.Where(x => x.PassportRow.GroupKey != 0)
                .GroupBy(x => new {x.MunicipalityId, x.ContragentId})
                .ToDictionary(x => x.Key, x => x.ToList());

            var listToUpdateOkiProviderPassportRow = new List<OkiProviderPassportRow>();

            foreach (var pair in rowDataOkiByMu)
            {
                if (!baseRowsOkiByMu.ContainsKey(pair.Key))
                {
                    continue;
                }

                var rows = pair.Value;
                var baseRows = baseRowsOkiByMu[pair.Key];

                //восстановить групповой ключ элемента из базового
                foreach (var row in rows)
                {
                    var baseRow = baseRows
                        .Where(x => x.MetaAttributeId == row.MetaAttributeId)
                        .Where(x => x.MetaAttributeParentId == row.MetaAttributeParentId)
                        .Where(x => x.PassportRow.Value == row.PassportRow.Value)
                        .FirstOrDefault();

                    if (baseRow != null)
                    {
                        row.PassportRow.GroupKey = baseRow.PassportRow.GroupKey;

                        listToUpdateOkiProviderPassportRow.Add(row.PassportRow);
                    }
                }
            }

            this.Container.Release(houseProvidePassportRowService);
            this.Container.Release(okiProvidePassportRowService);

            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            sessionProvider.CloseCurrentSession();

            using (this.Container.Using(sessionProvider))
            {
                using (var session = sessionProvider.OpenStatelessSession())
                {
                    this.BatchTransactionalUpdater(listToUpdateHouseProviderPassportRow, session, 1000);
                    this.BatchTransactionalUpdater(listToUpdateOkiProviderPassportRow, session, 1000);
                }
            }

            return new BaseDataResult();
        }

        private void BatchTransactionalUpdater<T>(List<T> items, IStatelessSession session, int batchSize) where T : PersistentObject
        {
            var savedCount = 0;

            while (savedCount <= items.Count())
            {
                var toSave = items.Skip(savedCount).Take(batchSize).ToList();

                savedCount += batchSize;

                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        toSave.ForEach(session.Update);
                        toSave.Clear();
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}