namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class InfoAboutPaymentHousingService : IInfoAboutPaymentHousingService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult SaveInfoAboutPaymentHousing(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
                var records = baseParams.Params["records"]
                    .As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>().ReadClass<InfoAboutPaymentHousing>())
                    .ToList();

                var service = this.Container.Resolve<IDomainService<InfoAboutPaymentHousing>>();

                var existingInfoAboutPaymentHousingList = service.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                foreach (var rec in records)
                {

                    InfoAboutPaymentHousing existingInfoAboutPaymentHousing = null;

                    if (existingInfoAboutPaymentHousingList.ContainsKey(rec.Id))
                        existingInfoAboutPaymentHousing = existingInfoAboutPaymentHousingList[rec.Id];

                    if (existingInfoAboutPaymentHousing != null)
                    {
                        existingInfoAboutPaymentHousing.CounterValuePeriodStart = rec.CounterValuePeriodStart;
                        existingInfoAboutPaymentHousing.CounterValuePeriodEnd = rec.CounterValuePeriodEnd;
                        existingInfoAboutPaymentHousing.GeneralAccrual = rec.GeneralAccrual;
                        existingInfoAboutPaymentHousing.Collection = rec.Collection;

                        service.Update(existingInfoAboutPaymentHousing);
                    }
                    else
                    {
                        var newInfoAboutPaymentCommunal = new InfoAboutPaymentHousing
                        {
                            DisclosureInfoRealityObj = new DisclosureInfoRealityObj { Id = disclosureInfoRealityObjId },
                            BaseService = new BaseService { Id = rec.Id },
                            CounterValuePeriodStart = rec.CounterValuePeriodStart,
                            CounterValuePeriodEnd = rec.CounterValuePeriodEnd,
                            GeneralAccrual = rec.GeneralAccrual,
                            Collection = rec.Collection
                        };

                        service.Save(newInfoAboutPaymentCommunal);
                    }
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
