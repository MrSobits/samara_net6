namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Entities;

    /// <inheritdoc />
    public class AppCitAdmonAppealViewModel : BaseViewModel<AppCitAdmonAppeal>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<AppCitAdmonAppeal> domainService, BaseParams baseParams)
        {

            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("AppealCitsAdmonition", 0L);

            var data = domainService.GetAll()
               .Where(x => x.AppealCitsAdmonition.Id == id)
               .Select(x => new
               {
                   x.Id,
                   x.AppealCits,
                   x.AppealCitsAdmonition,
                   AppealNumber = x.AppealCits.NumberGji,
                   AppealDate = x.AppealCits.DateFrom,
                   Correspondent = x.AppealCits.Correspondent,
                   Address = x.AppealCits.RealityAddresses
               })
               .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}