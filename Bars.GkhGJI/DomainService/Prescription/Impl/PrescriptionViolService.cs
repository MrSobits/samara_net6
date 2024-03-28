namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Entities;

    using Castle.Windsor;
    using System;

    public class PrescriptionViolService : IPrescriptionViolService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListPrescriptionViolation(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var prescriptionId = baseParams.Params.ContainsKey("documentId")
            ? baseParams.Params.GetValue("documentId").ToInt()
            : 0;           

            if (prescriptionId>0)
            {
                var prescrViolationDomain = Container.Resolve<IDomainService<PrescriptionViol>>();             

              var data = prescrViolationDomain.GetAll()
              .Where(x => x.Document.Id == prescriptionId)
              .Select(x => new
              {
                  x.Id,
                  x.DatePlanRemoval,
                  x.DatePlanExtension,
                  x.NotificationDate,
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

        public virtual IDataResult ListRealityObject(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * Параметры могут входить следующие6
             * 1. documentId - идентификатор предписаня
             * Тут мы по id предписания группируем нарушения по Домума и выводим
             * Адрес Дома и напротив Количество нарушений
             */

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var service = Container.Resolve<IDomainService<PrescriptionViol>>();

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
                .Filter(loadParam, Container)
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

        public virtual IDataResult AddViolations(BaseParams baseParams)
        {
            return new BaseDataResult();
        }

        public IDataResult AddPrescriptionViolations(BaseParams baseParams)
        {
            return new BaseDataResult();
        }

        public IDataResult SetNewDatePlanRemoval(BaseParams baseParams, DateTime paramdate, long documentId)
        {
            return new BaseDataResult();
        }
    }
}