namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Entities;

    using Castle.Windsor;
    using System;

    public class PrescriptionViolService : IPrescriptionViolService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PrescriptionViol> ViolDomain { get; set; }

        public IDomainService<PrescriptionRealityObject> PrescriptionRoDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<ViolationGji> DictViolationDomain { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDataResult ListRealityObject(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs<long>("documentId", 0);

            var dictViol =
                ViolDomain.GetAll()
                          .Where(x => x.Document.Id == documentId)
                          .Select(x => new { x.Id, RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0, })
                          .AsEnumerable()
                          .GroupBy(x => x.RealityObjectId)
                          .ToDictionary(x => x.Key, y => y.Count());

            var resultList = new List<ProxyRoViol>();

            if (dictViol.ContainsKey(0))
            {
                // Короче если ест ьнарушения без домов, т отогда добавляем фиктивную запись чтобы просто видеть эти нарушения
                // что они есть 
                resultList.Add(new ProxyRoViol
                {
                    Id = 0,
                    ViolationCount = dictViol[0],
                    RealityObject = "Дом отсутствует"
                });
            }

            resultList.AddRange(
                PrescriptionRoDomain.GetAll()
                        .Where(x => x.Prescription.Id == documentId)
                        .Select(
                            x => new { RealityObject = x.RealityObject.Address, RealityObjectId = x.RealityObject.Id, })
                        .AsQueryable()
                        .OrderIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                        .AsEnumerable()
                        .Select(
                            x =>
                            new ProxyRoViol
                            {
                                Id = x.RealityObjectId,
                                ViolationCount = dictViol.ContainsKey(x.RealityObjectId) ? dictViol[x.RealityObjectId] : 0,
                                RealityObject = x.RealityObject
                            })
                            .ToList());

            int totalCount = resultList.Count();

            return new ListDataResult(resultList.AsQueryable().Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        protected class ProxyRoViol
        {
            public long Id { get; set; }
            public int ViolationCount { get; set; }
            public string RealityObject { get; set; }
        }

        public IDataResult AddViolations(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var insViolIds = baseParams.Params.GetAs<long[]>("insViolIds");

            var violIds = new long[0];

            var currentRo =
                PrescriptionRoDomain.GetAll()
                                    .Where(x => x.Prescription.Id == documentId)
                                    .Select(x => x.Id)
                                    .AsEnumerable()
                                    .GroupBy(x => x)
                                    .ToDictionary(x => x.Key, y => y.First());


            var prescription = this.PrescriptionDomain.GetAll().FirstOrDefault(x => x.Id == documentId);

            var listPrescriptionViols = new List<PrescriptionViol>();
            var listPrescriptionRo = new List<PrescriptionRealityObject>();

            var dictInsViols = InspectionViolDomain.GetAll()
                                    .Where(x => insViolIds.Contains(x.Id))
                                    .Select(x => new { x.Id, violId = x.Violation.Id, roId = x.RealityObject != null ? x.RealityObject.Id : 0 })
                                    .ToList()
                                    .GroupBy(x => x.violId)
                                    .ToDictionary(x => x.Key, y => y.ToList());
            if (dictInsViols.Any())
            {
                violIds = dictInsViols.Keys.ToArray();
            }

            // в этом списке будут id нарушений, которые уже связаны с этим предписанием
            // (чтобы недобавлять несколько одинаковых документов в одно и тоже предписание)
            var listIds =
                ViolDomain.GetAll()
                          .Where(x => x.Document.Id == documentId)
                          .Where(x => violIds.Contains(x.InspectionViolation.Violation.Id))
                          .Select(
                              x =>
                              new
                              {
                                  x.InspectionViolation.Violation.Id,
                                  roId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0
                              })
                          .ToList()
                          .GroupBy(x => x.Id + "_" + x.roId)
                          .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            // получаю словарь на случай если будет выбрано мног онарушений будет работат ьбыстрее чем получение по отдельности Нарушения
            var dictViolations = DictViolationDomain.GetAll()
                                .Where(x => violIds.Contains(x.Id))
                                .Select(x => new { x.Id, x.Name })
                                .ToList()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.FirstOrDefault());


            foreach (var id in violIds)
            {
                // если нет в словаре то тоже выходим
                if (!dictViolations.ContainsKey(id))
                {
                    continue;
                }

                if (insViolIds.Any())
                {
                    // В этом случае имеем дело с нарушениями котоыревыбраны для приказа
                    // из родительского документа, а значит бежим по каждому нарушению и создаем для него комбинацию
                    if (!dictInsViols.ContainsKey(id))
                    {
                        continue;
                    }

                    foreach (var insViol in dictInsViols[id])
                    {
                        // В этом случае имеем дело с нарушениями котоыре выбраны просто из справочника
                        // Если для дома такая комбинация уже есть то тогда пролетаем мимо 
                        if (listIds.ContainsKey(id + "_" + insViol.roId))
                        {
                            continue;
                        }

                        var presViol = new PrescriptionViol()
                        {
                            Document = prescription,
                            InspectionViolation = new InspectionGjiViol { Id = insViol.Id },
                            TypeViolationStage = TypeViolationStage.InstructionToRemove
                        };

                        listPrescriptionViols.Add(presViol);

                        if (insViol.roId > 0 && !currentRo.ContainsKey(insViol.roId))
                        {
                            currentRo.Add(insViol.roId, insViol.roId);

                            var presRo = new PrescriptionRealityObject()
                            {
                                RealityObject =
                                                     new RealityObject { Id = insViol.roId },
                                Prescription = prescription
                            };

                            listPrescriptionRo.Add(presRo);
                        }
                    }
                }

            }

            using (var transaction = Container.Resolve<IDataTransaction>())
            {

                try
                {
                    listPrescriptionViols.ForEach(x => ViolDomain.Save(x));

                    listPrescriptionRo.ForEach(x => PrescriptionRoDomain.Save(x));

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult(new { success = false, message = e.Message });
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        public IDataResult SetNewDatePlanRemoval(BaseParams baseParams, DateTime paramdate, long documentId)
        {
            return new BaseDataResult();
        }
        public IDataResult AddPrescriptionViolations(BaseParams baseParams)
        {
            return new BaseDataResult();
        }
        public IDataResult ListPrescriptionViolation(BaseParams baseParams)
        {
            return new ListDataResult();
        }
    }
}