namespace Bars.Gkh.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Utils;
    using System.Linq;
    using Bars.Gkh.Entities.Administration.PrintCertHistory;

    public class PrintCertHistoryViewModel : BaseViewModel<PrintCertHistory>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<PrintCertHistory> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.AccNum,
                    x.Address,
                    x.Type,
                    x.Name,
                    x.PrintDate,
                    x.Username,
                    x.Role
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}