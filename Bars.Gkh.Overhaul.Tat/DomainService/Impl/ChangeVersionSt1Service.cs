namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using GkhCr.DomainService;
    using GkhCr.Entities;
    using GkhCr.Enums;

    public class ChangeVersionSt1Service : IChangeVersionSt1Service
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ChangeYear(BaseParams baseParams)
        {
            var typeWorkId = baseParams.Params.GetAsId("typeWorkId");
            var records = baseParams.Params.GetAs<Stage1Proxy[]>("records");
            var versSt1TypeWorkDomain = Container.ResolveDomain<TypeWorkCrVersionStage1>();
            var typeWorkDomain = Container.ResolveRepository<TypeWorkCr>();
            var twHistoryDomain = Container.ResolveDomain<TypeWorkCrHistory>();
            var user = Container.Resolve<IUserIdentity>();
            try
            {
                var typeWork = typeWorkDomain.Get(typeWorkId);
                TypeWorkCr tempTypeWork = records.Any(x => x.Year == typeWork.YearRepair) ? null : typeWork;

                var periodName = typeWork.ObjectCr.ProgramCr.Period.Name;
                var programName = typeWork.ObjectCr.ProgramCr.Name;
                var dateStart = typeWork.ObjectCr.ProgramCr.Period.DateStart;
                var dateEnd = typeWork.ObjectCr.ProgramCr.Period.DateEnd;

                if (records.Any(x => dateStart.Year-2 > x.Year || (dateEnd.HasValue && dateEnd.Value.Year+2 < x.Year)))
                {
                    return new BaseDataResult(false, string.Format("Год ремонта не может выходить за период '{0}' краткосрочной программы '{1}'", periodName, programName));
                }

                var typeWorkYearsWithSameType = typeWorkDomain.GetAll()
                    .Where(x => x.Work.Id == typeWork.Work.Id && x.ObjectCr.Id == typeWork.ObjectCr.Id && x.Id != typeWork.Id)
                    .Select(x => x.YearRepair)
                    .ToArray();

                if (records.Any(x => typeWorkYearsWithSameType.Contains(x.Year)))
                {
                    return new BaseDataResult(false, "Есть запись с данной работой в этом году");
                }

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        records.GroupBy(x => x.Year)
                            .ForEach(x =>
                            {
                                if (tempTypeWork == null)
                                {
                                    tempTypeWork = new TypeWorkCr
                                    {
                                        ObjectCr = typeWork.ObjectCr,
                                        FinanceSource = typeWork.FinanceSource,
                                        Work = typeWork.Work,
                                        StageWorkCr = typeWork.StageWorkCr,
                                        HasPsd = typeWork.HasPsd,
                                        Description = typeWork.Description,
                                        IsActive = typeWork.IsActive,
                                        IsDpkrCreated = typeWork.IsDpkrCreated
                                    };
                                }

                                if (typeWork.YearRepair == x.Key)
                                {
                                    tempTypeWork = typeWork;
                                }

                                tempTypeWork.YearRepair = x.Key;

                                if (tempTypeWork.Id > 0)
                                {
                                    typeWorkDomain.Update(tempTypeWork);
                                }
                                else
                                {

                                   var  history = new TypeWorkCrHistory();

                                   history.TypeWorkCr = tempTypeWork;
                                   history.TypeAction = TypeWorkCrHistoryAction.Modification;
                                   history.TypeReason = TypeWorkCrReason.NotSet;
                                   history.FinanceSource = tempTypeWork.FinanceSource;
                                   history.Volume = tempTypeWork.Volume;
                                   history.Sum = tempTypeWork.Sum;
                                   history.YearRepair = typeWork.YearRepair;
                                   history.NewYearRepair = tempTypeWork.YearRepair;
                                   history.UserName = user.Name;

                                   typeWorkDomain.Save(tempTypeWork);
                                   twHistoryDomain.Save(history);
                                }

                                x.ForEach( y =>
                                {
                                    var rec = versSt1TypeWorkDomain.Load(y.Id);
                                    rec.TypeWorkCr = tempTypeWork;

                                    versSt1TypeWorkDomain.Update(rec);
                                });

                                tempTypeWork = null;
                            });

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                }
            }
            finally
            {
                Container.Release(versSt1TypeWorkDomain);
                Container.Release(typeWorkDomain);
                Container.Release(twHistoryDomain);
                Container.Release(user);
            }

            return new BaseDataResult();
        }


        private class Stage1Proxy
        {
            public long Id { get; set; }

            public int Year { get; set; }
        }
    }
}