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

    public class DeletePassportRowDuplicates : BaseExecutionAction
    {
        public override string Description => "Удаление дублей данных в паспорте дома 1468 (к 36838)";

        public override string Name => "Удаление дублей данных в паспорте дома 1468 (к 36838)";

        public override Func<IDataResult> Action => this.DeleteRowDuplicates;

        public BaseDataResult DeleteRowDuplicates()
        {
            var houseProvidePassportRowRepository = this.Container.Resolve<IRepository<HouseProviderPassportRow>>();

            var duplicates = houseProvidePassportRowRepository.GetAll()
                .Join(
                    houseProvidePassportRowRepository.GetAll(),
                    x => x.MetaAttribute.Id,
                    y => y.MetaAttribute.Id,
                    (a, b) => new {a, b})
                .Where(x => x.a.Id != x.b.Id)
                .Where(x => x.a.ProviderPassport.Id == x.b.ProviderPassport.Id)
                .Where(x => x.a.GroupKey == x.b.GroupKey)
                .Select(
                    x => new
                    {
                        MetaAttributeId = x.a.MetaAttribute.Id,
                        providerPassportId = x.a.ProviderPassport.Id,
                        x.a.GroupKey,
                        aValue = x.a.Value,
                        bValue = x.b.Value,
                        aId = x.a.Id,
                        bId = x.b.Id
                    })
                .ToList();

            this.Container.Release(houseProvidePassportRowRepository);

            var listsToDel = duplicates
                .GroupBy(x => new {x.MetaAttributeId, x.providerPassportId, x.GroupKey})
                .Select(
                    x =>
                    {
                        // Получим все идентификаторы "зеркальных" записей
                        var identicalValueIds = x
                            .SelectMany(y => new List<long> {y.aId, y.bId})
                            .Distinct()
                            .ToList();

                        var pairsList = x
                            .SelectMany(
                                y => new List<Tuple<long, string>> {new Tuple<long, string>(y.aId, y.aValue), new Tuple<long, string>(y.bId, y.bValue)})
                            .ToList();

                        var maxValue = pairsList.Max(y => y.Item2);
                        var maxValueId = pairsList.First(y => y.Item2 == maxValue).Item1;

                        // Оставим единственную запись, остальное на удаление
                        identicalValueIds = identicalValueIds.Where(y => y != maxValueId).ToList();

                        return identicalValueIds;
                    })
                .ToList();

            var idsToDel = listsToDel.SelectMany(x => x).ToList();

            this.BatchTransactionalDelete(idsToDel, 1000);

            return new BaseDataResult();
        }

        private void BatchTransactionalDelete(List<long> idsToDel, int batchSize)
        {
            var deletedCount = 0;

            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            using (this.Container.Using(sessionProvider))
            {
                using (var session = sessionProvider.GetCurrentSession())
                {
                    var query = session.CreateSQLQuery("DELETE FROM GKH_HOUSE_PROV_PASS_ROW WHERE ID = :id");

                    while (deletedCount <= idsToDel.Count())
                    {
                        var toDelete = idsToDel.Skip(deletedCount).Take(batchSize).ToList();

                        deletedCount += batchSize;

                        using (var tr = session.BeginTransaction())
                        {
                            try
                            {
                                foreach (var idToDel in toDelete)
                                {
                                    query.SetParameter("id", idToDel);
                                    query.ExecuteUpdate();
                                }

                                tr.Commit();
                            }
                            catch (Exception exp)
                            {
                                tr.Rollback();
                                throw;
                            }
                        }

                        toDelete.Clear();
                    }
                }
            }
        }
    }
}