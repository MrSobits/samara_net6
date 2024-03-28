namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class PrescriptionViolService : IPrescriptionViolService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PrescriptionViol> ViolDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<PrescriptionOfficialReport> PrescriptionOfficialReportDomain { get; set; }

        public IDomainService<PrescriptionOfficialReportViolation> PrescriptionOfficialReportViolationDomain { get; set; }

        public IDomainService<ViolationGji> DictViolationDomain { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDataResult ListPrescriptionViolation(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var prescriptionId = baseParams.Params.ContainsKey("documentId")
            ? baseParams.Params.GetValue("documentId").ToInt()
            : 0;

            if (prescriptionId > 0)
            {
                var prescrViolationDomain = Container.Resolve<IDomainService<PrescriptionViol>>();

                var data = prescrViolationDomain.GetAll()
                .Where(x => x.Document.Id == prescriptionId)
                .Select(x => new
                {
                    x.Id,
                    DatePlanRemoval = x.DatePlanRemoval.HasValue? x.DatePlanRemoval: x.InspectionViolation.DatePlanRemoval,
                    x.DatePlanExtension,
                    x.NotificationDate,
                    x.Description,
                    x.Action,
                    x.InspectionViolation.DateFactRemoval,
                    ViolationGji = x.InspectionViolation.Violation.Name,
                    ViolationGjiPin = x.InspectionViolation.Violation.CodePin,
                    CodesPin = x.InspectionViolation.Violation.NormativeDocNames,
                    RealityObject = x.InspectionViolation.RealityObject.Address
                })
                .Filter(loadParams, Container);
                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }

            return null;

        }

        public IDataResult ListRealityObject(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * Параметры могут входить следующие6
             * 1. documentId - идентификатор предписаня
             * Тут мы по id предписания группируем нарушения по Домума и выводим
             * Адрес Дома и напротив Количество нарушений
             */

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var service = this.Container.Resolve<IDomainService<PrescriptionViol>>();

            var data = service.GetAll()
                .Where(x => x.Document.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name : "",
                    RealityObject = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address : "",
                    RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .Filter(loadParam, this.Container)
                .AsEnumerable()
                .GroupBy(x => new { x.RealityObjectId, x.Municipality, x.RealityObject })
                .Select(x => new
                {
                    Id = x.Key.RealityObjectId,
                    ViolationCount = x.Count(),
                    x.Key.Municipality,
                    RealityObject = x.Key.RealityObjectId == 0 ? "Дом отсутствует" : x.Key.RealityObject
                })
                .AsQueryable();

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).ToList(), totalCount);
        }

        public IDataResult AddViolations(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var insViolIds = baseParams.Params.GetAs<long[]>("insViolIds");

            var violIds = new long[0];

            var prescription = this.PrescriptionDomain.GetAll().FirstOrDefault(x => x.Id == documentId);

            var listPrescriptionViols = new List<PrescriptionViol>();

            var dictInsViols = this.InspectionViolDomain.GetAll()
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
                this.ViolDomain.GetAll()
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
            var dictViolations = this.DictViolationDomain.GetAll()
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
                            InspectionViolation = new InspectionGjiViol { Id = insViol.Id},
                            TypeViolationStage = TypeViolationStage.InstructionToRemove
                        };

                        listPrescriptionViols.Add(presViol);
                    }
                }
            }

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    listPrescriptionViols.ForEach(x => this.ViolDomain.Save(x));

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

        public IDataResult AddPrescriptionViolations(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var insViolIds = baseParams.Params.GetAs<long[]>("objectIds");
            if (insViolIds == null)
            {
                try
                {
                    insViolIds = baseParams.Params.GetAs<string>("objectIds").Return(x => x.Split(',').Select(y => y.ToLong())).ToArray();
                }
                catch (Exception e)
                {

                }
            }

            var prescriptionOfficialReport = this.PrescriptionOfficialReportDomain.GetAll().FirstOrDefault(x => x.Id == documentId);

            var listPrescriptionViols = new List<PrescriptionOfficialReportViolation>();

            var dictInsViols = this.PrescriptionOfficialReportViolationDomain.GetAll()
                                    .Where(x => x.PrescriptionOfficialReport == prescriptionOfficialReport)
                                    .Select(x => x.PrescriptionViol.Id).ToList();            


            foreach (var id in insViolIds)
            {
                // если нет в словаре то тоже выходим
                if (!dictInsViols.Contains(id))
                {
                    try
                    {
                        PrescriptionOfficialReportViolation newViolation = new PrescriptionOfficialReportViolation
                        {
                            ObjectCreateDate = DateTime.Now,
                            ObjectEditDate = DateTime.Now,
                            PrescriptionOfficialReport = prescriptionOfficialReport,
                            PrescriptionViol = new PrescriptionViol { Id = id }
                        };
                        PrescriptionOfficialReportViolationDomain.Save(newViolation);
                    }
                    catch (Exception e)
                    {
                        return new BaseDataResult(new { success = false, message = e.Message });
                    }

                }
              
            }

            return new BaseDataResult();
        }

        public IDataResult SetNewDatePlanRemoval(BaseParams baseParams, DateTime paramdate, long documentId)
        {
            if (documentId > 0)
            {
                var prescrViolations = ViolDomain.GetAll()
                .Where(x => x.Document.Id == documentId)
                .ToList();

                var inspectionViolations = ViolDomain.GetAll()
                     .Where(x => x.Document.Id == documentId)
                    .Select(x => x.InspectionViolation).ToList();
                try
                {
                    foreach (InspectionGjiViol iv in inspectionViolations)
                    {
                        iv.DatePlanRemoval = paramdate;
                        InspectionViolDomain.Update(iv);
                    }

                    foreach (PrescriptionViol pv in prescrViolations)
                    {
                        pv.DatePlanRemoval = paramdate;
                        ViolDomain.Update(pv);
                    }
                }
                catch(Exception e)
                {
                    return new BaseDataResult(new { success = false, message = e.Message });
                }
            }
            return new BaseDataResult();
        }
    }
}