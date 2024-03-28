namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class InfoAboutReductionPaymentService : IInfoAboutReductionPaymentService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTemplateService(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',');

                var service = this.Container.Resolve<IDomainService<InfoAboutReductionPayment>>();

                // получаем у контроллера услуги что бы не добавлять их повторно
                var exsistingInfoAboutReductionPayment = service.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                    .Select(x => x.BaseService.Id)
                    .ToList();

                foreach (var id in objectIds)
                {
                    if (exsistingInfoAboutReductionPayment.Contains(id.ToLong()))
                    {
                        continue;
                    }

                    var newId = id.ToLong();

                    var newInfoAboutReductionPayment = new InfoAboutReductionPayment
                    {
                        BaseService = new BaseService { Id = newId },
                        DisclosureInfoRealityObj = new DisclosureInfoRealityObj { Id = disclosureInfoRealityObjId }
                    };

                    service.Save(newInfoAboutReductionPayment);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}
