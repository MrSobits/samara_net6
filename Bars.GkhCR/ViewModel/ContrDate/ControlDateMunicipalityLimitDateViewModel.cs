namespace Bars.GkhCr.ViewModel.ContrDate
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    public class ControlDateMunicipalityLimitDateViewModel : BaseViewModel<ControlDateMunicipalityLimitDate>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ControlDateMunicipalityLimitDate> domainService, BaseParams baseParams)
        {
            var controlDateId = baseParams.Params.GetAsId("controlDateId");
            return domainService.GetAll()
                .Where(x => x.ControlDate.Id == controlDateId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    LimitDate = x.LimitDate.ToString("dd.MM.yyyy")
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
