namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.Entities;

    public class HousekeeperReportServiceInterceptor : EmptyDomainInterceptor<HousekeeperReport>
    {
        public override IDataResult BeforeCreateAction(IDomainService<HousekeeperReport> service, HousekeeperReport entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<HousekeeperReport> service, HousekeeperReport entity)
        {
            var housekeeperReportFileService = Container.Resolve<IDomainService<HousekeeperReportFile>>();
            var housekeeperReportFileList =
                housekeeperReportFileService.GetAll()
                    .Where(x => x.HousekeeperReport.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToArray();
            foreach (var id in housekeeperReportFileList)
            {
                housekeeperReportFileService.Delete(id);
            }

            return Success();
        }
    }
}