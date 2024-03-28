namespace Bars.GisIntegration.Gkh.Service.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.UI.Service;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для получения объектов при выполнении экспорта/импорта данных через сервис Services
    /// </summary>
    public class ServicesService : IServicesService
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод получения списка объектов текущего ремонта
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult GetRepairObjectList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var extractor = this.Container.Resolve<IDataSelector<RepairObject>>("WorkListSelector");

            try
            {
                var repairObjects = extractor.GetExternalEntities(new DynamicDictionary());

                var result = repairObjects
                    .Select(
                        x => new
                        {
                            x.Id,
                            RepairProgramName = x.RepairProgram.Name,
                            RepairProgramPeriod = x.RepairProgram.Period.Name,
                            RepairProgramState = x.RepairProgram.TypeProgramRepairState,
                            x.RealityObject.Address
                        })
                    .AsQueryable()
                    .Filter(loadParams, this.Container)
                    .OrderBy(x => x.RepairProgramName)
                    .ThenBy(x => x.Address);

                return new ListDataResult(result.Paging(loadParams).ToList(), result.Count());
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Метод возвращает список активных программ текущего ремонта
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult GetRepairProgramList(BaseParams baseParams)
        {
            var repairProgramDomain = this.Container.ResolveDomain<RepairProgram>();
            var loadParams = baseParams.GetLoadParam();

            using (this.Container.Using(repairProgramDomain))
            {
                var data = repairProgramDomain.GetAll()
                    .Where(x => x.TypeProgramRepairState == TypeProgramRepairState.Active)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Name,
                            x.Period.DateStart.Year,
                            State = x.TypeProgramRepairState
                        })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
        }

        /// <summary>
        /// Метод возвращает список актов выполненных работ
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult GetCompletedWorkList(BaseParams baseParams)
        {
            var performedRepairWorkActDomain = this.Container.ResolveDomain<PerformedRepairWorkAct>();
            var loadParams = baseParams.GetLoadParam();

            using (this.Container.Using(performedRepairWorkActDomain))
            {
                var data = performedRepairWorkActDomain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.ActDate,
                            x.ActNumber,
                            x.ActDescription,
                            x.RepairWork.RepairObject.RealityObject.Address
                        })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
        }
    }
}
