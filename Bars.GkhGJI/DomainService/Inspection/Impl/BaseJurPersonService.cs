namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;

    using Entities;
    using Castle.Windsor;

    /// <summary>
    /// Базовый персональный сервер 
    /// </summary>
    public class BaseJurPersonService : IBaseJurPersonService
    {
        /// <summary>
        /// Контролер 
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Результат выполнения запроса для базовых параметров 
        /// </summary>
        /// <param name="baseParams">Базовые параметров </param>
        /// <returns></returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            /*
             * метод нужен для получения списка домов управляющей организации для плановой проверки юр лиц
             * inspectionId - id проверки
             * условия выборки домов: жилой дом находится в разделе жилые дома управляющей организации и дата договора пересекается с датой плана проверки юр.лиц
             * realityObjIds - id домов через запятую
             */

            var inspectionGjiInspectorDomainService = Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var inspectionGjiZonalInspectionDomainService = Container.Resolve<IDomainService<InspectionGjiZonalInspection>>();

            try
            {
                var inspectorIds = string.Empty;
                var inspectorNames = string.Empty;

                var zonalInspectionIds = string.Empty;
                var zonalInspectionNames = string.Empty;

                if (baseParams.Params.ContainsKey("inspectionId"))
                {
                    var inspectionId = baseParams.Params.GetAs<long>("inspectionId");

                    // получаем инспекторов по проверке
                    var inspectors = inspectionGjiInspectorDomainService.GetAll()
                        .Where(x => x.Inspection.Id == inspectionId)
                        .Select(x => new { x.Inspector.Id, x.Inspector.Fio })
                        .ToList();

                    foreach (var rec in inspectors)
                    {
                        if (!string.IsNullOrEmpty(inspectorIds))
                        {
                            inspectorIds += ", ";
                        }

                        inspectorIds += rec.Id.ToString();

                        if (!string.IsNullOrEmpty(inspectorNames))
                        {
                            inspectorNames += ", ";
                        }

                        inspectorNames += rec.Fio;
                    }

                    // получаем отделы по проверке
                    var zonalInspections = inspectionGjiZonalInspectionDomainService.GetAll()
                        .Where(x => x.Inspection.Id == inspectionId)
                        .Select(x => new { x.ZonalInspection.Id, x.ZonalInspection.ZoneName })
                        .ToList();

                    foreach (var rec in zonalInspections)
                    {
                        if (!string.IsNullOrEmpty(zonalInspectionIds))
                        {
                            zonalInspectionIds += ", ";
                        }

                        zonalInspectionIds += rec.Id.ToString();

                        if (!string.IsNullOrEmpty(zonalInspectionNames))
                        {
                            zonalInspectionNames += ", ";
                        }

                        zonalInspectionNames += rec.ZoneName;
                    }
                }

                return new BaseDataResult(new { inspectorIds, inspectorNames, zonalInspectionIds, zonalInspectionNames });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                Container.Release(inspectionGjiInspectorDomainService);
                Container.Release(inspectionGjiZonalInspectionDomainService);
            }
        }

        /// <summary>
        /// Запуск фильтра
        /// </summary>
        /// <returns></returns>
        public IDataResult GetStartFilters()
        {
            var plan = Container.Resolve<IDomainService<PlanJurPersonGji>>().GetAll()
                .OrderByDescending(x => x.ObjectCreateDate)
                .FirstOrDefault();

            return plan != null
                ? new BaseDataResult(new { planId = plan.Id, planName = plan.Name, dateStart = plan.DateStart })
                : new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult FillPlanNumber()
        {
            using (var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
            {
                var count = session.CreateSQLQuery(@"update GJI_INSPECTION_JURPERSON ij
                set plan_number = i.plan_number
                from(
                    select row_number() over(partition by plan_id order by object_create_date) plan_number, j.id
                    from GJI_INSPECTION_JURPERSON j
                join GJI_INSPECTION i on i.id = j.id
                where j.plan_number = 0) i where ij.id = i.id")
                    .ExecuteUpdate();

                return new BaseDataResult(count);
            }
        }
    }
}