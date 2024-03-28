namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.PassportProvider;

    using Castle.Windsor;
    
    /// <summary>
    /// Сервис работ и услуг
    /// </summary>
    public class WorkService : IWorkService
    {
        /// <summary>
        /// IoC Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список работ для дома по периоду
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список работ для дома по периоду</returns>
        public IDataResult ListWorksRealityObjectByPeriod(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var realityObjectId = baseParams.Params.ContainsKey("realityObjectId") ? baseParams.Params["realityObjectId"].ToInt() : 0;
            var periodId = baseParams.Params.ContainsKey("periodId") ? baseParams.Params["periodId"].ToInt() : 0;

            if (this.Container.Kernel.HasComponent(typeof(IRealtyObjTypeWorkProvider)))
            {
                var rObjectTypeWorkProvider = this.Container.Resolve<IRealtyObjTypeWorkProvider>();
                var workDomain = this.Container.Resolve<IDomainService<Work>>();

                var roWorksQuery = rObjectTypeWorkProvider.GetWorks(realityObjectId, periodId);

                var data = workDomain.GetAll()
                    .Where(x => roWorksQuery.Any(y => x.Id == y.Work.Id))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Name
                        }).Filter(loadParams, this.Container);

                var totalCount = data.Count();

                data = data.Order(loadParams).Paging(loadParams);

                return new ListDataResult(data.ToList(), totalCount);
            }

            return new ListDataResult {Success = false, Message = "Модуль капитального ремонта не подключен"};
        }

        /// <summary>
        /// Получить список работ без разделения на страницы
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>список работ без разделения на страницы</returns>
        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var domaneService = this.Container.Resolve<IDomainService<Work>>();

            var data = domaneService.GetAll().Select(x => new {x.Id, x.Name}).Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name)
                .Order(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        /// <summary>
        /// Добавить связанную запись справочника "Работы по содержанию и ремонту МКД"
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="domainService">Доменный сервис сущности Work</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult AddContentRepairMkdWorks(BaseParams baseParams, IDomainService<Work> domainService)
        {
            try
            {
                var signedOrganizationWorkId = baseParams.Params.GetAs<long>("signedOrganizationWorkId");
                var contentRepairMkdWorkIds = baseParams.Params.GetAs<long[]>("contentRepairMkdWorkIds");

                var organizationWork = domainService.Get(signedOrganizationWorkId);

                var contentRepairMkdWorkService = this.Container.ResolveDomain<ContentRepairMkdWork>();

                var exsisting = contentRepairMkdWorkService.GetAll()
                    .Where(x => x.Work != null && x.Work.Id == signedOrganizationWorkId)
                    .Select(x => x.Work.Id)
                    .ToList();

                foreach (var newId in contentRepairMkdWorkIds.Where(id => !exsisting.Contains(id)))
                {
                    var contentRepairMkdWork = contentRepairMkdWorkService.Get(newId);
                    contentRepairMkdWork.Work = organizationWork;
                    contentRepairMkdWorkService.Update(contentRepairMkdWork);
                }

                return new BaseDataResult {Success = true};
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult {Success = false, Message = exc.Message};
            }
        }

        /// <summary>
        /// Удалить связанную запись справочника "Работы по содержанию и ремонту МКД"
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="domainService">Доменный сервис сущности Work</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult DeleteContentRepairMkdWork(BaseParams baseParams, IDomainService<Work> domainService)
        {
            try
            {
                var contentRepairMkdWorkId = baseParams.Params.GetAs<long>("contentRepairMkdWorkId");

                var contentRepairMkdWorkService = this.Container.ResolveDomain<ContentRepairMkdWork>();

                var contentRepairMkdWork = contentRepairMkdWorkService.Get(contentRepairMkdWorkId);

                contentRepairMkdWork.Work = null;
                contentRepairMkdWorkService.Update(contentRepairMkdWork);

                return new BaseDataResult {Success = true};
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult {Success = false, Message = exc.Message};
            }
        }
    }
}