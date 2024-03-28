namespace Bars.GkhDi.DomainService
{
    using B4;
    using B4.Utils;
    using Entities;

    public class DocumentsViewModel : BaseViewModel<Documents>
    {
        public override IDataResult Get(IDomainService<Documents> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params["id"].To<long>());

            return new BaseDataResult(
                new
                {
                    obj.Id,
                    DisclosureInfo = obj.DisclosureInfo.Id,
                    obj.FileProjectContract,
                    obj.NotAvailable,
                    obj.DescriptionProjectContract,
                    obj.DescriptionCommunalCost,
                    obj.DescriptionCommunalTariff,
                    obj.FileCommunalService,
                    obj.FileServiceApartment
                });
        }
    }
}