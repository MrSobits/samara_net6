using System;
using System.Collections;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Enums;
using Bars.GkhCr.DomainService;
using Bars.GkhCr.Entities;

namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Entities;

    using Castle.Windsor;

    public class CompetitionLotTypeWorkService : ICompetitionLotTypeWorkService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<CompetitionLotTypeWork> CompetitionLotTypeWorkDomain { get; set; }

        public IDomainService<CompetitionLot> CompetitionLotDomain { get; set; }

        public IDataResult AddWorks(BaseParams baseParams)
        {
            var lotId = baseParams.Params.GetAs("lotId", 0L);
            var typeWorkIds = baseParams.Params.GetAs("typeWorkIds", new long[0]);

            var lot = CompetitionLotDomain.GetAll().FirstOrDefault(x => x.Id == lotId);

            if (lot == null)
            {
                return new BaseDataResult(false, "Не удалось определить лот по Id " + lot.ToStr());
            }

            var listToSave = new List<CompetitionLotTypeWork>();

            var currentIds = CompetitionLotTypeWorkDomain.GetAll()
                .Where(x => x.Lot.Id == lotId)
                .Select(x => x.TypeWork.Id)
                .Distinct()
                .ToList();

            foreach (var id in typeWorkIds)
            {
                if (currentIds.Contains(id))
                {
                    continue;
                }

                listToSave.Add(new CompetitionLotTypeWork
                {
                    TypeWork = new TypeWorkCr { Id = id },
                    Lot = lot
                });
            }

            if (listToSave.Any())
            {
                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToSave.ForEach(x => CompetitionLotTypeWorkDomain.Save(x));

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        return new BaseDataResult(false, exc.Message);
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}