namespace Bars.Gkh.UserActionRetention.DomainService.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Castle.Windsor;

    public class AuditLogMapService : IAuditLogMapService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public IDataResult List(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var ids = baseParams.Params.GetAs("Id", string.Empty);

            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.Trim()).ToArray() : new string[0];

            var logInfo = Container.Resolve<IChangeLogInfoProvider>()
                                .GetLoggedEntities()
                                .WhereIf(listIds.Length > 0, x => listIds.Contains(x.EntityType))
                                .Select(x => new
                                {
                                    Id = x.EntityType,
                                    Name = x.EntityName
                                }).ToList();

            //Добавление в список выбора фильтрации по отслеживанию сущьности MonthlyFeeAmountDecision
            logInfo.Add(new
            {
                Id = "Bars.Gkh.Decisions.Nso.Entities.MonthlyFeeAmountDecision",
                Name = "Размер ежемесячного взноса на КР (Протокол решений)"
            });

            var data = logInfo.AsQueryable().Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = Container.Resolve<IChangeLogInfoProvider>()
                                .GetLoggedEntities()
                                .AsQueryable()
                                .OrderBy(x => x.EntityName)
                                .Select(x => new
                                {
                                    Id = x.EntityType,
                                    Name = x.EntityName
                                })
                                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).ToList(), totalCount);
        }
    }
}
