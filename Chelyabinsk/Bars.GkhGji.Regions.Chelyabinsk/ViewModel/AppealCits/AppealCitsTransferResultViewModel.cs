namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel.AppealCits
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;

    /// <inheritdoc />
    public class AppealCitsTransferResultViewModel : BaseViewModel<AppealCitsTransferResult>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<AppealCitsTransferResult> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    AppealCitsId = x.AppealCits.Id,
                    AppealCitsNumber = x.AppealCits.DocumentNumber,
                    x.StartDate,
                    x.EndDate,
                    x.Type,
                    x.Status,
                    User = x.User.Name ?? x.User.Login,
                    x.LogFile
                }).ToListDataResult(baseParams.GetLoadParam());
        }
    }
}