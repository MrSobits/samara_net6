namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;

    using Castle.Windsor;

    public class ProtocolViolationService : IProtocolViolationService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListRealityObject(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * Параметры могут входить следующие:
             * 1. documentId - идентификатор протокола
             * Тут мы по id протокола группируем нарушения по Дому и выводим
             * Адрес Дома и напротив Количество нарушений
             */

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var service = Container.Resolve<IDomainService<ProtocolViolation>>();

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
                        RealityObject = x.Key.RealityObject == "" ? "Дом отсутствует" : x.Key.RealityObject
                    })
                .AsQueryable();

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).ToList(), totalCount);
        }

        public virtual IDataResult AddViolations(BaseParams baseParams)
        {
            return new BaseDataResult();
        }
    }
}