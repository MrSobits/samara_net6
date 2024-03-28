using System.Collections;

namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Entities;
    using Castle.Windsor;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Entities.Dicts;
    using Converter = B4.DomainService.BaseParams.Converter;

    public class TypeWorkCrService : ITypeWorkCrService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkDomain { get; set; }

        public IDomainService<ProtocolCrTypeWork> ProtocolCrTypeWorkDomain { get; set; }

        public IDomainService<PerformedWorkAct> ActDomain { get; set; }

        public IDataResult CalcPercentOfCompletion(BaseParams baseParams)
        {
            /* Процент выполнения делается так = (Объем факт. /Объем план.)*100, где
            * Объем факт - сумма объемов из всех актов выполненных работ по этой работе
            * Объем план - объем по работе из вкладки "Виды работ"
            */

            var objectId = baseParams.Params.GetAsId("objectId");

            var types = TypeWorkDomain.GetAll()
                .Where(x => x.ObjectCr.Id == objectId)
                .ToDictionary(x => x.Id);

            var actsVolume =
                ActDomain.GetAll()
                    .Where(x => x.TypeWorkCr.ObjectCr.Id == objectId && x.Volume.HasValue)
                    .Select(x => new {x.TypeWorkCr.Id, x.Volume})
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.Volume.Value));

            var typesToSave = new List<TypeWorkCr>();

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
            
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    typesToSave.ForEach(x => TypeWorkDomain.Update(x));
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
                var data = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == realityObjectId && x.ObjectCr.ProgramCr.Period.Id == periodId)
                    .Select(x => x.Work)
                    .Filter(loadParam, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }

            return new ListDataResult();
        }

        public IList ListByProgramCr(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var loadParam = baseParams.GetLoadParam();
            var programId = baseParams.Params.GetAs<long>("programId");

            var data = TypeWorkDomain.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                .Select(x => new
                {
                    x.Id,
                    TypeWork = x.Work.Name,
                    RoMunicipality = x.ObjectCr.RealityObject.Municipality.Name,
                    RoAddress = x.ObjectCr.RealityObject.Address
                })
                .Filter(loadParam, Container);

            totalCount = data.Count();

            return data.Order(loadParam).Paging(loadParam).ToList();
        }

        public IDataResult CreateTypeWork(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAs<long>("roId");
            var programId = baseParams.Params.GetAs<long>("programId");
            var workId = baseParams.Params.GetAs<long>("workId");

            var objectCrDomain = Container.ResolveDomain<Entities.ObjectCr>();
            var workDomain = Container.ResolveDomain<Work>();
            var roDomain = Container.ResolveDomain<RealityObject>();
            var programDomain = Container.ResolveDomain<ProgramCr>();
            var typeWorkRepo = Container.ResolveRepository<TypeWorkCr>();

            TypeWorkCr typeWork;

            if (typeWorkRepo.GetAll()
                .Any(x => x.ObjectCr.RealityObject.Id == roId
                          && x.ObjectCr.ProgramCr.Id == programId
                          && x.Work.Id == workId))
            {
                return new BaseDataResult(false,
                    "Объект КР с таким жилым домом, программой и видом работы уже существует");
            }

            using (var tr = Container.Resolve<IDataTransaction>())
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
                        objectCr = new Entities.ObjectCr(ro, program);
                        objectCrDomain.Save(objectCr);
                    }

                    typeWork = new TypeWorkCr
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

        public IDataResult MoveTypeWork(BaseParams baseParams, Int64 programId, Int64 typeworkToMoveId)
        {
            
            var objectCrDomain = Container.ResolveDomain<Entities.ObjectCr>();
            var workDomain = Container.ResolveDomain<Work>();
            var roDomain = Container.ResolveDomain<RealityObject>();
            var programDomain = Container.ResolveDomain<ProgramCr>();
            var typeWorkRepo = Container.ResolveRepository<TypeWorkCr>();

            if (typeworkToMoveId > 0 && programId > 0)
            {
                var program = programDomain.Get(programId);
                var typeWorkCr = typeWorkRepo.Get(typeworkToMoveId);
                var realityObject = typeWorkCr.ObjectCr.RealityObject;
                var objectCrInNewPr = objectCrDomain.GetAll()
                    .Where(x => x.RealityObject == realityObject)
                    .Where(x => x.ProgramCr.Id == programId).FirstOrDefault();
                var currentProgramName = typeWorkCr.ObjectCr.ProgramCr.Name;
                if (objectCrInNewPr != null)
                {
                    typeWorkCr.ObjectCr = objectCrInNewPr;
                    typeWorkCr.Description = "Перенесена из программы " + currentProgramName;
                    typeWorkRepo.Update(typeWorkCr);
                }
                else
                {
                    var objectCr = new Entities.ObjectCr(realityObject, program) { };
                    objectCrDomain.Save(objectCr);
                    typeWorkCr.ObjectCr = objectCr;
                    typeWorkCr.Description = "Перенесена из программы " + currentProgramName;
                    typeWorkRepo.Update(typeWorkCr);
                }
                //меняем год в ДПКР
                try
                {
                    int year = program.Period.DateStart.Year;
                   // var structElement = StructuralElementWorkDomain.GetAll()
                   //.Where(x => x.Job. == structuralElement)
                   //.Select(x => x.Job.Work)
                   //.FirstOrDefault();
                }
                catch
                {
                    return new BaseDataResult(false, "Работа КПКР перенесена в новый период, ООИ в ДПКР не определен, отредактируйте версию вручную");
                }
            }
            else
            {
                return new BaseDataResult(false, "Не определены новая программа или не найдена работа");
            }
            return new BaseDataResult(true);
        }
    }
}