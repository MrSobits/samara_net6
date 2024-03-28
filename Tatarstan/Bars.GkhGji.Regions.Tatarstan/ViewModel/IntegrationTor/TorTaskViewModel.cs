namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.IntegrationTor
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    // TODO : Расскоментировать после реализации GisIntegration
   // using Bars.GisIntegration.Tor.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

   // public class TorTaskViewModel : BaseViewModel<TorTask>
   // {
        /// <inheritdoc />
       /* public override IDataResult List(IDomainService<TorTask> domainService, BaseParams baseParams)
        {
            var tatarstanDisposalDomain = this.Container.ResolveDomain<TatarstanDisposal>();

            using (this.Container.Using(tatarstanDisposalDomain))
            {
                var disposals= domainService.GetAll()
                    .Where(x => x.Disposal != null)
                    .Select(x => x.Disposal.Id)
                    .Distinct()
                    .ToHashSet();

                var tatarstanDisposalsDict = tatarstanDisposalDomain.GetAll()
                    .Where(x => disposals.Contains(x.Id))
                    .ToDictionary(x => x.Id, x => x.TorId);

                return domainService.GetAll()
                    .OrderByDescending(x => x.ObjectEditDate)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.SendObject,
                        x.TypeRequest,
                        TaskState = x.TorTaskState,
                        TorId = tatarstanDisposalsDict.ContainsKey(x.Disposal?.Id ?? 0) ? tatarstanDisposalsDict[x.Disposal.Id] : null,
                        RegistrationTorDate = x.ObjectEditDate,
                        RequestFileId = x.RequestFile?.Id,
                        ResponseFileId = x.ResponseFile?.Id,
                        LogFileId = x.LogFile?.Id
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }*/
   // }
}
