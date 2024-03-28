namespace Bars.GkhGji.Regions.Tula.DomainService
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tula.Entities;

    /// <summary>
    /// В сахе изменяется список в реестре документов ГЖИ - Акты обследований
    /// колонки новые
    /// </summary>
    public class TulaActSurveyService : GkhGji.DomainService.ActSurveyService
    {
        public override IDataResult GetInfo(long? documentId)
        {
            var serviceInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceRealityObjects = Container.Resolve<IDomainService<ActSurveyRealityObject>>();
            var documentDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var disposalDomain = Container.Resolve<IDomainService<Bars.GkhGji.Entities.Disposal>>();

            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var objectAddress = string.Empty;

                // Пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов


                var dataInspectors =
                    serviceInspector.GetAll()
                                    .Where(x => x.DocumentGji.Id == documentId)
                                    .Select(x => new { InspectorId = x.Inspector.Id, x.Inspector.Fio })
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
                var dataObjects =
                    serviceRealityObjects.GetAll()
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


                var disposalKindCheckCode =
                    disposalDomain.GetAll()
                                  .Where(x => x.KindCheck != null)
                                  .Where(
                                      x =>
                                      documentDomain.GetAll()
                                                    .Any(
                                                        y =>
                                                        y.Children.Id == documentId
                                                        && y.Parent.TypeDocumentGji == TypeDocumentGji.Disposal
                                                        && y.Parent.Id == x.Id
                                                        ))
                                  .Select(x => x.KindCheck)
                                  .FirstOrDefault();

                return new BaseDataResult(new { inspectorNames, inspectorIds, objectAddress, disposalCode = (disposalKindCheckCode != null ? disposalKindCheckCode.Code : 0) });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                Container.Release(serviceInspector);
                Container.Release(serviceRealityObjects);
                Container.Release(documentDomain);
                Container.Release(disposalDomain);
            }
        }

        public override IList ListView(BaseParams baseParams, bool pagging, ref int totalCount)
        {
            var tulaSurveyDomain = Container.Resolve<IDomainService<TulaActSurvey>>();

            try
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

            // Так желаю чтобы просто вьюху неправить sql 
                var dictTulaActSurveys =
                    tulaSurveyDomain.GetAll()
                                    .Select(x => new { x.Id, x.ConclusionIssued })
                                    .AsEnumerable()
                                    .GroupBy(x => x.Id)
                                    .ToDictionary(x => x.Key, y => y.Select(x => x.ConclusionIssued).First());

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
                    x.MunicipalityId,
                    x.RealityObjectAddresses,
                    FactSurveyed = x.TypeSurvey,
                    x.InspectorNames,
                    x.InspectionId,
                    x.TypeBase,
                    x.TypeDocumentGji
                })
                .AsEnumerable()
                .Select(x => new {
                    x.Id,
                    x.State,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.MunicipalityNames,
                    x.MunicipalityId,
                    x.RealityObjectAddresses,
                    x.FactSurveyed,
                    x.InspectorNames,
                    x.InspectionId,
                    x.TypeBase,
                    x.TypeDocumentGji,
                    ConclusionIssued = dictTulaActSurveys.Get(x.Id)
                })
                .AsQueryable()
                .Filter(loadParam, Container);

            totalCount = data.Count();

            if (pagging)
            {
                return data.Order(loadParam).Paging(loadParam).ToList();
            }

            return data.Order(loadParam).ToList();
            }
            finally 
            {
                Container.Release(tulaSurveyDomain);
            }
            
        }
    }
}