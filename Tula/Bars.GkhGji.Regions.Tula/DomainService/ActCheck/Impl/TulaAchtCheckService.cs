namespace Bars.GkhGji.Regions.Tula.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class TulaActCheckService : ActCheckService, ITulaActCheckService
    {
        public IDomainService<InspectionGjiRealityObject> InspRealObjDomain { get; set; }

        public IDataResult ListRealObjForActCheck(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var inspectionId = baseParams.Params.GetAsId("inspectionId");

            var data = InspRealObjDomain.GetAll()
               .Where(x => x.Inspection.Id == inspectionId)
               .Where(x => !ActCheckRoDomain.GetAll().Any(y => y.ActCheck.Inspection.Id == inspectionId && y.RealityObject.Id == x.RealityObject.Id))
               .Select(x => new
               {
                   x.Id,
                   MunicipalityName = x.RealityObject.Municipality.Name,
                   RealityObjectId = x.RealityObject.Id,
                   x.RealityObject.Address
               })
               .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public  override IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var isActView = baseParams.Params.GetAs<bool>("isActView");

            var serviceActCheckRobject = Container.ResolveDomain<ActCheckRealityObject>();
            var serviceDocumentChildren = Container.ResolveDomain<DocumentGjiChildren>();
            var serviceActCheck = Container.ResolveDomain<ActCheck>();

            var data = GetViewList()
                .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, y => serviceActCheckRobject.GetAll().Any(x => x.ActCheck.Id == y.Id && x.RealityObject.Id == realityObjectId))
                .WhereIf(isActView, y => serviceActCheck.GetAll().Any(x => x.Id == y.Id && x.TypeActCheck == TypeActCheckGji.ActView))
                .WhereIf(!isActView, y => !serviceActCheck.GetAll().Any(x => x.Id == y.Id && x.TypeActCheck == TypeActCheckGji.ActView))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    x.ContragentName,
                    RealityObjectCount = serviceActCheckRobject.GetAll().Count(y => y.ActCheck.Id == x.Id),
                    CountExecDoc = serviceDocumentChildren.GetAll()
                        .Count(y => y.Parent.Id == x.Id && (y.Children.TypeDocumentGji == TypeDocumentGji.Protocol || y.Children.TypeDocumentGji == TypeDocumentGji.Prescription)),
                    HasViolation = serviceActCheckRobject.GetAll().Any(y => y.ActCheck.Id == x.Id && y.HaveViolation == YesNoNotSet.Yes),
                    x.InspectorNames,
                    x.InspectionId,
                    x.TypeBase,
                    x.TypeDocumentGji
                })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}