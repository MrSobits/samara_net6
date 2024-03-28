namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Вьюмодель для технических заказчиков
    /// </summary>
    public class TechnicalCustomerViewModel : BaseViewModel<TechnicalCustomer>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<TechnicalCustomer> domain, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("Id");

            var rec = domain.Get(id);

            if (rec == null)
            {
                return new BaseDataResult(null);
            }

            return new BaseDataResult(
                new
                {
                    rec.Id,
                    rec.Contragent,
                    OrganizationForm = rec.Contragent.OrganizationForm?.Name ?? string.Empty,
                    rec.Period,
                    rec.File
                });
        }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<TechnicalCustomer> domain, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);
            var query = domain.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        Contragent = x.Contragent.Name,
                        OrganizationForm = x.Contragent.OrganizationForm != null ? x.Contragent.OrganizationForm.Name : string.Empty,
                        Period = x.Period.Name,
                        x.File
                    })
                .Filter(loadParam, this.Container);

            return new ListDataResult(query.Order(loadParam).Paging(loadParam), query.Count());
        }
    }
}