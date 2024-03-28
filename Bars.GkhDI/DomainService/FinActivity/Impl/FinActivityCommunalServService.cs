namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class FinActivityCommunalServService : IFinActivityCommunalServService
    {
        public IWindsorContainer Container { get; set; }
        
        public IDataResult AddWorkMode(BaseParams baseParams)
        {
            var finActivityCommunalServDomain = Container.ResolveDomain<FinActivityCommunalService>();
            var disclosureInfoDomain = Container.ResolveDomain<DisclosureInfo>();

            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var records = baseParams.Params["records"]
                    .As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>().ReadClass<FinActivityCommunalService>())
                    .ToList();

                var existingFinActivityCommunalService = finActivityCommunalServDomain.GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                    .AsEnumerable()
                    .GroupBy(x => x.TypeServiceDi)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                var disclosureInfo = disclosureInfoDomain.GetAll().FirstOrDefault(x => x.Id == disclosureInfoId);

                foreach (var rec in records)
                {
                    FinActivityCommunalService existingCommunalService = null;

                    if (existingFinActivityCommunalService.ContainsKey(rec.TypeServiceDi))
                        existingCommunalService = existingFinActivityCommunalService[rec.TypeServiceDi];

                    if (existingCommunalService != null)
                    {
                        existingCommunalService.Exact = rec.Exact;
                        existingCommunalService.IncomeFromProviding = rec.IncomeFromProviding;
                        existingCommunalService.DebtPopulationStart = rec.DebtPopulationStart;
                        existingCommunalService.DebtPopulationEnd = rec.DebtPopulationEnd;
                        existingCommunalService.DebtManOrgCommunalService = rec.DebtManOrgCommunalService;
                        existingCommunalService.PaidByMeteringDevice = rec.PaidByMeteringDevice;
                        existingCommunalService.PaidByGeneralNeeds = rec.PaidByGeneralNeeds;
                        existingCommunalService.PaymentByClaim = rec.PaymentByClaim;

                        finActivityCommunalServDomain.Update(existingCommunalService);
                    }
                    else
                    {
                        var newFinActivityCommunalService = new FinActivityCommunalService
                        {
                            DisclosureInfo = disclosureInfo,
                            TypeServiceDi = rec.TypeServiceDi,
                            Exact = rec.Exact,
                            IncomeFromProviding = rec.IncomeFromProviding,
                            DebtPopulationStart = rec.DebtPopulationStart,
                            DebtPopulationEnd = rec.DebtPopulationEnd,
                            DebtManOrgCommunalService = rec.DebtManOrgCommunalService,
                            PaidByMeteringDevice = rec.PaidByMeteringDevice,
                            PaidByGeneralNeeds = rec.PaidByGeneralNeeds,
                            PaymentByClaim = rec.PaymentByClaim
                        };

                        finActivityCommunalServDomain.Save(newFinActivityCommunalService);
                    }
                }

                return new BaseDataResult {Success = true};
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult {Success = false, Message = exc.Message};
            }
            finally
            {
                Container.Release(finActivityCommunalServDomain);
                Container.Release(disclosureInfoDomain);
            }
        }
    }
}
