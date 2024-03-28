namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using B4;
    using B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService.Contracts;
    using Gkh.Authentification;
    using Bars.GkhGji.Entities;
    using Enums;
    using Castle.Windsor;
    using Utils = Bars.GkhGji.Utils.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис для Приказ
    /// </summary>
    public class PreventiveVisitService : IPreventiveVisitService
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Получить информацию
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var info = GetInfo(documentId);
                return new BaseDataResult(new { inspectorNames = info.InspectorNames, inspectorIds = info.InspectorIds, baseName = info.BaseName, planName = info.PlanName });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }        

		/// <summary>
		/// Получить информацию о приказе
		/// </summary>
		/// <param name="documentId">Идентификатор документа</param>
		/// <returns>Результат выполнения запроса</returns>
        public DisposalInfo GetInfo(long documentId)
        {
            var serviceDocInspector = Container.Resolve<IDocumentGjiInspectorService>();
            var servicePreventiveVisit = Container.Resolve<IDomainService<PreventiveVisit>>();
            var serviceChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var baseName = string.Empty;
                var planName = string.Empty;

                // Сначала пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var dataInspectors =
                    serviceDocInspector.GetInspectorsByDocumentId(documentId)
                        .Select(x => new {InspectorId = x.Inspector.Id, x.Inspector.Fio})
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

                return new DisposalInfo(inspectorNames, inspectorIds, "", "");
            }
            catch (ValidationException e)
            {
                throw e;
            }
            finally
            {
                Container.Release(serviceDocInspector);
                Container.Release(servicePreventiveVisit);
                Container.Release(serviceChildren);
            }
        }

        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<PreventiveVisitRealityObject>>();
            var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceInspection = Container.Resolve<IDomainService<PreventiveVisit>>();

            using (var transaction = Container.Resolve<IDataTransaction>())
            {

                try
                {
                    var documentId = baseParams.Params.GetAs<long>("documentId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    var existRecs =
                        service.GetAll()
                            .Where(x => x.PreventiveVisit.Id == documentId)
                            .Where(x => objectIds.Contains(x.RealityObject.Id))
                            .Select(x => x.RealityObject.Id)
                            .Distinct()
                            .ToList();

                    var inspection = serviceInspection.Load(documentId);

                    foreach (var id in objectIds)
                    {
                        // Если среди существующих домов данно инспекции уже есть такой дом то пролетаем мимо
                        if (existRecs.Contains(id))
                            continue;

                        // Если такого дома еще нет то добалвяем
                        var newObj = new PreventiveVisitRealityObject
                        {
                            PreventiveVisit = inspection,
                            RealityObject = serviceRobject.Load(id)
                        };

                        service.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(service);
                    Container.Release(serviceInspection);
                    Container.Release(serviceRobject);
                }
            }
        }

        public IDataResult GetListRoForResultPV(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var documentId = baseParams.Params.ContainsKey("documentId")
                                  ? baseParams.Params["documentId"].ToLong()
                                  : 0;
            var servicePreventiveVisit = Container.Resolve<IDomainService<PreventiveVisit>>();
            var serviceinspectionRo = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            var inspId = servicePreventiveVisit.Get(documentId);
            if (inspId != null)
            {
                var data = serviceinspectionRo.GetAll()
               .Where(x => x.Inspection.Id == inspId.Inspection.Id)
               .Select(x => new
               {
                   x.RealityObject.Id,
                   Municipality = x.RealityObject.Municipality.Name,
                   x.RealityObject.Address
               })
               .Filter(loadParams, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }

            return null;
        }
    }
}