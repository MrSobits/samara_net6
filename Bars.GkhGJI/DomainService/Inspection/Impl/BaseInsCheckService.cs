namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using Entities;

    using Castle.Windsor;

    public class BaseInsCheckService : IBaseInsCheckService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            /*
             * метод нужен для получения списка домов управляющей организации для плановой инспекционной проверки
             * inspectionId - id проверки
             * условия выборки домов: жилой дом находится в разделе жилые дома управляющей организации и дата договора пересекается с датой плана проверки юр.лиц
             * realityObjIds - id домов которые можно выбирать вкачестве проверяемых через запятую
             * inspectorNames - ФИО инспекторов через запятую
             * inspectorIds - Id инспекторов через запятую
             * robjectNames - Адреса домов через запятую
             * robjectIds - Id текущих домов проверки через запятую 
             */
            try
            {
                var inspectorNames = "";
                var inspectorIds = "";
                var robjectNames = "";
                var robjectIds = "";
                var robjectArea = 0m;

                if (baseParams.Params.ContainsKey("inspectionId"))
                {
                    var inspectionId = baseParams.Params.GetAs<long>("inspectionId");

                    // Получаем информацию по существующим в проверке Инспекторам
                    var serviceInspector = Container.Resolve<IDomainService<InspectionGjiInspector>>();

                    var dataInspectors = 
                        serviceInspector
                            .GetAll()
                            .Where(x => x.Inspection.Id == inspectionId)
                            .Select(x => new
                                {
                                    x.Inspector.Id,
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

                        inspectorIds += item.Id.ToString();
                    }

                    var serviceRealityObject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

                    var dataRObjects = 
                        serviceRealityObject
                            .GetAll()
                            .Where(x => x.Inspection.Id == inspectionId)
                            .Select(x => new
                                {
                                    x.RealityObject.Address,
                                    RealityObjectId = x.RealityObject.Id,
                                    Area = x.RealityObject.AreaMkd
                                })
                            .ToList();

                    foreach (var item in dataRObjects)
                    {
                        if (!string.IsNullOrEmpty(robjectNames))
                        {
                            robjectNames += ", ";
                        }

                        robjectNames += item.Address;

                        if (!string.IsNullOrEmpty(robjectIds))
                        {
                            robjectIds += ", ";
                        }

                        robjectIds += item.RealityObjectId.ToString();

                        robjectArea += item.Area != null ? (decimal) item.Area : 0;
                    }
                    
                }
                return new BaseDataResult(new { inspectorIds, inspectorNames, robjectNames, robjectIds, robjectArea });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }

        public IDataResult GetStartFilters()
        {
            var plan = Container.Resolve<IDomainService<PlanInsCheckGji>>().GetAll()
                .Where(x => x.DateEnd >= DateTime.Now && x.DateStart <= DateTime.Now)
                .OrderByDescending(x => x.ObjectCreateDate)
                .FirstOrDefault();

            return plan != null ? new BaseDataResult(new { planId = plan.Id, planName = plan.Name, dateStart = plan.DateStart }) : new BaseDataResult();
        }
    }
}