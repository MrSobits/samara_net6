using System.Collections;

namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.GkhCr.Enums;

    using Entities;
    using Castle.Windsor;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Entities.Dicts;
    using Converter = B4.DomainService.BaseParams.Converter;

    public class SpecialTypeWorkCrService : ISpecialTypeWorkCrService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<SpecialTypeWorkCr> TypeWorkDomain { get; set; }

        public IDomainService<SpecialProtocolCrTypeWork> ProtocolCrTypeWorkDomain { get; set; }

        public IDomainService<SpecialPerformedWorkAct> ActDomain { get; set; }

        public IDataResult CalcPercentOfCompletion(BaseParams baseParams)
        {
            /* Процент выполнения делается так = (Объем факт. /Объем план.)*100, где
            * Объем факт - сумма объемов из всех актов выполненных работ по этой работе
            * Объем план - объем по работе из вкладки "Виды работ"
            */

            var objectId = baseParams.Params.GetAsId("objectId");

            var types = this.TypeWorkDomain.GetAll()
                .Where(x => x.ObjectCr.Id == objectId)
                .ToDictionary(x => x.Id);

            var actsVolume = this.ActDomain.GetAll()
                    .Where(x => x.TypeWorkCr.ObjectCr.Id == objectId && x.Volume.HasValue)
                    .Select(x => new {x.TypeWorkCr.Id, x.Volume})
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.Volume.Value));

            var typesToSave = new List<SpecialTypeWorkCr>();

            foreach (var t in types)
            {
                var type = t.Value;

                var oldPercent = type.PercentOfCompletion;

                if (!actsVolume.ContainsKey(t.Key))
                {
                    continue;
                }

                var factVolume = actsVolume[t.Key];

                type.PercentOfCompletion = 0;

                if (factVolume > 0 && type.Volume.HasValue && type.Volume.Value > 0)
                {
                    type.PercentOfCompletion = (factVolume * 100m / type.Volume.Value).RoundDecimal(2);
                }
                else if (factVolume > 0)
                {
                    type.PercentOfCompletion = 100;
                }

                if (oldPercent != type.PercentOfCompletion)
                {
                    typesToSave.Add(type);
                }
            }
            
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    typesToSave.ForEach(x => this.TypeWorkDomain.Update(x));
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, exc.Message);
                }
            }

            return new BaseDataResult();
        }

        public IDataResult ListRealityObjectWorksByPeriod(BaseParams baseParams)
        {
            var loadParam = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var periodId = baseParams.Params.GetAs<long>("periodId");

            if (realityObjectId > 0 && periodId > 0)
            {
                var data = this.Container.Resolve<IDomainService<SpecialTypeWorkCr>>().GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == realityObjectId && x.ObjectCr.ProgramCr.Period.Id == periodId)
                    .Select(x => x.Work)
                    .Filter(loadParam, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }

            return new ListDataResult();
        }

        public IList ListByProgramCr(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var loadParam = baseParams.GetLoadParam();
            var programId = baseParams.Params.GetAs<long>("programId");

            var data = this.TypeWorkDomain.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                .Select(x => new
                {
                    x.Id,
                    TypeWork = x.Work.Name,
                    RoMunicipality = x.ObjectCr.RealityObject.Municipality.Name,
                    RoAddress = x.ObjectCr.RealityObject.Address
                })
                .Filter(loadParam, this.Container);

            totalCount = data.Count();

            return data.Order(loadParam).Paging(loadParam).ToList();
        }

        public IDataResult CreateTypeWork(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("roId");
            var programId = baseParams.Params.GetAs<long>("programId");
            var workId = baseParams.Params.GetAs<long>("workId");

            var objectCrDomain = this.Container.ResolveDomain<Entities.SpecialObjectCr>();
            var workDomain = this.Container.ResolveDomain<Work>();
            var roDomain = this.Container.ResolveDomain<RealityObject>();
            var programDomain = this.Container.ResolveDomain<ProgramCr>();
            var typeWorkRepo = this.Container.ResolveRepository<SpecialTypeWorkCr>();

            SpecialTypeWorkCr typeWork;

            if (typeWorkRepo.GetAll()
                .Any(x => x.ObjectCr.RealityObject.Id == roId
                          && x.ObjectCr.ProgramCr.Id == programId
                          && x.Work.Id == workId))
            {
                return new BaseDataResult(false,
                    "Объект КР с таким жилым домом, программой и видом работы уже существует");
            }

            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var objectCr = objectCrDomain.GetAll()
                        .Where(x => x.RealityObject.Id == roId)
                        .FirstOrDefault(x => x.ProgramCr.Id == programId);

                    if (objectCr == null)
                    {
                        var ro = roDomain.Get(roId);
                        var program = programDomain.Get(programId);
                        objectCr = new Entities.SpecialObjectCr(ro, program);
                        objectCrDomain.Save(objectCr);
                    }

                    typeWork = new SpecialTypeWorkCr
                    {
                        ObjectCr = objectCr,
                        Work = workDomain.Get(workId),
                        IsActive = true
                    };

                    typeWorkRepo.Save(typeWork);

                    tr.Commit();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new BaseDataResult(typeWork);
        }

        public IDataResult ListWorksCr(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var programIds = baseParams.Params.GetAs<string>("program").ToLongArray();
            var muIds = baseParams.Params.GetAs<string>("mu").ToLongArray();
            var workIds = baseParams.Params.GetAs<string>("work").ToLongArray();

            var repo = this.Container.ResolveRepository<TypeWorkCr>();
            try
            {
                var data = repo.GetAll()
                    .Where(x => x.IsActive)
                    .WhereIf(programIds.IsNotEmpty(), x => programIds.Contains(x.ObjectCr.ProgramCr.Id))
                    .WhereIf(muIds.IsNotEmpty(), x => muIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                    .WhereIf(workIds.IsNotEmpty(), x => workIds.Contains(x.Work.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        ProgramCr = x.ObjectCr.ProgramCr.Name,
                        Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                        x.ObjectCr.RealityObject.Address,
                        Work = x.Work.Name,
                        ObjectCr = x.ObjectCr.Id
                    })
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(repo);
            }
        }

        /// <inheritdoc />
        public IDataResult ListFinanceSources(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId", 0);
            
            var specialObjectCrDomain = this.Container.Resolve<IDomainService<Entities.SpecialObjectCr>>();
            var programCrFinSourceDomain = this.Container.Resolve<IDomainService<ProgramCrFinSource>>();

            try
            {
                var objectCr = specialObjectCrDomain.Load(objectCrId);

                if (objectCr != null)
                {

                    var data = programCrFinSourceDomain
                        .GetAll()
                        .Where(x => x.ProgramCr.Id == objectCr.ProgramCr.Id)
                        .Select(x => new
                        {
                            x.FinanceSource.Id,
                            x.FinanceSource.TypeFinance,
                            x.FinanceSource.TypeFinanceGroup,
                            x.FinanceSource.Name,
                            x.FinanceSource.Code
                        })
                        .Filter(loadParams, this.Container);

                    var totalCount = data.Count();

                    data = data.Order(loadParams).Paging(loadParams);

                    return new ListDataResult(data.ToList(), totalCount);
                }

                return new ListDataResult();
            }
            finally
            {
                this.Container.Release(specialObjectCrDomain);
                this.Container.Release(programCrFinSourceDomain);
            }
        }
    }
}