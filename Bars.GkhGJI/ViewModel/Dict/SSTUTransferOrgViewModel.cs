namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities.Dict;

    public class SSTUTransferOrgViewModel : BaseViewModel<SSTUTransferOrg>
    {
        public override IDataResult List(IDomainService<SSTUTransferOrg> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);


            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Fio,
                    x.Position,
                    x.Address,
                    x.Guid,
                    x.Description

                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
