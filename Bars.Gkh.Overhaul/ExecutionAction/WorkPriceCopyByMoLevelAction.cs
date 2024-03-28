namespace Bars.Gkh.Overhaul.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Entities;

    public class WorkPriceCopyByMoLevelAction : BaseExecutionAction
    {
        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<WorkPrice> WorkPriceDomain { get; set; }

        public override string Name => "Cоздать расценки по всем нижним уровням МО как по верхнему уровню МО";

        public override string Description => @"Cоздаем расценки по всем нижним уровням МО как по верхнему уровню МО.";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var muByParent = this.Container.Resolve<IRepository<Municipality>>().GetAll()
                .Where(x => x.ParentMo != null)
                .Select(x => new {x.Id, ParentId = x.ParentMo.Id})
                .AsEnumerable()
                .GroupBy(x => x.ParentId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id).ToList());

            var copyWorkPricesByMuId = this.WorkPriceDomain.GetAll()
                .Where(x => x.Municipality.ParentMo == null)
                .AsEnumerable()
                .GroupBy(x => x.Municipality.Id)
                .ToDictionary(x => x.Key, y => y.ToList());

            var existWorkPrices = this.WorkPriceDomain.GetAll()
                .Where(x => x.Municipality.ParentMo != null)
                .AsEnumerable()
                .GroupBy(x => string.Format("{0}_{1}_{2}", x.Municipality.Id, x.Job.Id, x.Year))
                .ToDictionary(x => x.Key, y => y.First());

            var listToSaveOrUpdate = new List<WorkPrice>();

            foreach (var parentMu in muByParent)
            {
                var parentWorkPrices = copyWorkPricesByMuId.ContainsKey(parentMu.Key)
                    ? copyWorkPricesByMuId[parentMu.Key]
                    : new List<WorkPrice>();

                foreach (var childrenMuId in parentMu.Value)
                {
                    var muProxy = this.MunicipalityDomain.Load(childrenMuId);

                    foreach (var parentWorkPrice in parentWorkPrices)
                    {
                        var key = string.Format("{0}_{1}_{2}", childrenMuId, parentWorkPrice.Job.Id, parentWorkPrice.Year);

                        WorkPrice item;

                        if (existWorkPrices.ContainsKey(key))
                        {
                            item = existWorkPrices[key];
                        }
                        else
                        {
                            item = new WorkPrice
                            {
                                Municipality = muProxy,
                                Job = parentWorkPrice.Job,
                                Year = parentWorkPrice.Year
                            };
                        }

                        item.NormativeCost = parentWorkPrice.NormativeCost;
                        item.SquareMeterCost = parentWorkPrice.SquareMeterCost;

                        listToSaveOrUpdate.Add(item);
                    }
                }
            }

            var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

            using (var tr = session.BeginTransaction())
            {
                try
                {
                    listToSaveOrUpdate.ForEach(
                        x =>
                        {
                            if (x.Id > 0)
                            {
                                session.Update(x);
                            }
                            else
                            {
                                session.Insert(x);
                            }
                        });

                    tr.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        tr.Rollback();

                        return new BaseDataResult
                        {
                            Success = false,
                            Message = exc.Message
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exc);
                    }
                }
            }

            return new BaseDataResult {Success = true};
        }
    }
}