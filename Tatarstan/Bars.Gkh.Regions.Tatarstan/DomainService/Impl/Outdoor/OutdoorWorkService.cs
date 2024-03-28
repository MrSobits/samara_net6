namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl.Outdoor
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class OutdoorWorkService : IOutdoorWorkService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListOutdoorWorksByPeriod(BaseParams baseParams)
        {
            IOutdoorTypeWorkProvider typeWorkProvider;
            var loadParams = baseParams.GetLoadParam();

            var outdoorId = baseParams.Params.GetAsId("outdoorId");
            var periodId = baseParams.Params.GetAsId("periodId");

            if (!this.Container.Kernel.HasComponent(typeof(IOutdoorTypeWorkProvider)))
            {
                return new ListDataResult { Success = false, Message = "Модуль капитального ремонта не подключен" };
            }

            using (this.Container.Using(typeWorkProvider = this.Container.Resolve<IOutdoorTypeWorkProvider>()))
            {
                var worksQuery = typeWorkProvider.GetWorks(outdoorId, periodId);

                return worksQuery
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Name
                        }).ToListDataResult(loadParams, this.Container);
            }
        }
    }
}