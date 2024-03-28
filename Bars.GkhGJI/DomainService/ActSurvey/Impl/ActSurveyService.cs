namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Authentification;
    using Entities;

    using Castle.Windsor;

    // Для расширения в регионах
    public class ActSurveyService: IActSurveyService
    {
        public IWindsorContainer Container { get; set; }

        public virtual IDataResult GetInfo(long? documentId)
        {
            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var objectAddress = string.Empty;

                // Пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var serviceInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>();

                var dataInspectors = serviceInspector.GetAll()
                    .Where(x => x.DocumentGji.Id == documentId)
                    .Select(x => new
                    {
                        InspectorId = x.Inspector.Id,
                        x.Inspector.Fio
                    })
                    .ToList();

                foreach (var item in dataInspectors)
                {
                    if (!string.IsNullOrEmpty(inspectorNames))
                    {
                        inspectorNames += ", ";
                    }

                    inspectorNames += item.Fio;

                    if (!string.IsNullOrEmpty(inspectorIds))
                    {
                        inspectorIds += ", ";
                    }

                    inspectorIds += item.InspectorId.ToString();
                }

                // Пробегаемся по домам в акте обследования и формируем адреса домов
                var serviceRealityObjects = Container.Resolve<IDomainService<ActSurveyRealityObject>>();

                var dataObjects = serviceRealityObjects.GetAll()
                    .Where(x => x.ActSurvey.Id == documentId)
                    .Select(x => x.RealityObject.Address)
                    .ToList();

                foreach (var item in dataObjects)
                {
                    if (!string.IsNullOrEmpty(objectAddress))
                    {
                        objectAddress += "; ";
                    }

                    objectAddress += item;
                }

                return new BaseDataResult(new { inspectorNames, inspectorIds, objectAddress });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }

        public virtual IList ListView(BaseParams baseParams, bool pagging, ref int totalCount)
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

            var data = GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        x.MunicipalityId,
                        x.RealityObjectAddresses,
                        FactSurveyed = x.TypeSurvey,
                        x.InspectorNames,
                        x.InspectionId,
                        x.TypeBase,
                        x.TypeDocumentGji
                    })
                .Filter(loadParam, Container);

            totalCount = data.Count();

            if (pagging)
            {
                return data.Order(loadParam).Paging(loadParam).ToList();
            }
            
            return data.Order(loadParam).ToList();
        }

        public IQueryable<ViewActSurvey> GetViewList()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var inspectorIds = userManager.GetInspectorIds();
            var municipalityids = userManager.GetMunicipalityIds();

            var serviceDocumentInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>();

            return Container.Resolve<IDomainService<ViewActSurvey>>().GetAll()
                .WhereIf(inspectorIds.Count > 0, y => serviceDocumentInspector.GetAll()
                    .Any(x => x.DocumentGji.Id == y.Id && inspectorIds.Contains(x.Inspector.Id)))
                .WhereIf(municipalityids.Count > 0, x => x.MunicipalityId.HasValue && municipalityids.Contains(x.MunicipalityId.Value));
        }
    }
}