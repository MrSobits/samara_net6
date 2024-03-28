namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class FinActivityService : IFinActivityService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetIdByDisnfoId(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

                var disclosureInfo = Container.Resolve<IDomainService<DisclosureInfo>>()
                    .GetAll()
                    .Where(x => x.Id == disclosureInfoId)
                    .Select(x => new
                                     {
                                         x.PeriodDi,
                                         x.ManagingOrganization.TypeManagement
                                     }).FirstOrDefault();

                var service = this.Container.Resolve<IDomainService<FinActivity>>();

                var finActivityId = service
                    .GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                return new BaseDataResult(new { Id = finActivityId, disclosureInfo })
                {
                    Success = true
                };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}
