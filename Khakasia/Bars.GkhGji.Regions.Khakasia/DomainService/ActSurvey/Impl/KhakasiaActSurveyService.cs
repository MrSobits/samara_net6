namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    /// <summary>
    /// В Хакасии изменяется список в реестре документов ГЖИ - Акты обследований
    /// колонки новые
    /// </summary>
    public class KhakasiaActSurveyService : GkhGji.DomainService.ActSurveyService
    {
		/// <summary>
		/// Получить информацию об акте обследования
		/// </summary>
		/// <param name="documentId">Идентификатор документа ГЖИ</param>
		/// <returns>Результат выполнения запроса</returns>
        public override IDataResult GetInfo(long? documentId)
        {
            var serviceInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceRealityObjects = this.Container.Resolve<IDomainService<ActSurveyRealityObject>>();
            var documentDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var disposalDomain = this.Container.Resolve<IDomainService<Bars.GkhGji.Entities.Disposal>>();

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
                this.Container.Release(serviceInspector);
                this.Container.Release(serviceRealityObjects);
                this.Container.Release(documentDomain);
                this.Container.Release(disposalDomain);
            }
        }

		/// <summary>
		/// Получить список из view
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <param name="pagging">Нужна ли пагинация</param>
		/// <param name="totalCount">Общее количество записей</param>
		/// <returns></returns>
        public override IList ListView(BaseParams baseParams, bool pagging, ref int totalCount)
        {
            var khakasiaSurveyDomain = this.Container.Resolve<IDomainService<KhakasiaActSurvey>>();

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
                var dictKhakasiaActSurveys =
                    khakasiaSurveyDomain.GetAll()
                                    .Select(x => new { x.Id, x.ConclusionIssued })
                                    .AsEnumerable()
                                    .GroupBy(x => x.Id)
                                    .ToDictionary(x => x.Key, y => y.Select(x => x.ConclusionIssued).First());

            var data = this.GetViewList()
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
                    ConclusionIssued = dictKhakasiaActSurveys.Get(x.Id)
                })
                .AsQueryable()
                .Filter(loadParam, this.Container);

            totalCount = data.Count();

            if (pagging)
            {
                return data.Order(loadParam).Paging(loadParam).ToList();
            }

            return data.Order(loadParam).ToList();
            }
            finally 
            {
                this.Container.Release(khakasiaSurveyDomain);
            }
            
        }
    }
}