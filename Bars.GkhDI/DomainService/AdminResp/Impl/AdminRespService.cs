namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;

    using Castle.Windsor;
    using Entities;

    public class AdminRespService : IAdminRespService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AdminResp> AdminRespDomain { get; set; }

        public IDomainService<DisclosureInfo> DisclosureInfoDomain { get; set; }

        public IDataResult AddAdminRespByResolution(BaseParams baseParams)
        {
            var disclosureInfoId = baseParams.Params.GetAs<long>("diInfoId");

            var resolutions = baseParams.Params.GetAs<ResolutionProxy[]>("resolutions");

            var orgs =
                Container.Resolve<IDomainService<SupervisoryOrg>>().GetAll()
                    .Where(x => x.Code != null)
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, y => y.Select(x => x).First());

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var disclosureInfo = DisclosureInfoDomain.Get(disclosureInfoId);

                    foreach (var resolutionObj in resolutions)
                    {
                        if (!orgs.ContainsKey(resolutionObj.SupervisoryOrgCode))
                        {
                            continue;
                        }

                        var adminResp = new AdminResp
                        {
                            DisclosureInfo = disclosureInfo,
                            SupervisoryOrg = orgs[resolutionObj.SupervisoryOrgCode],
                            AmountViolation = resolutionObj.ProtocolViolationsCount,
                            DateImpositionPenalty = resolutionObj.DocumentDate,
                            SumPenalty = resolutionObj.PenaltyAmount,
                            DatePaymentPenalty = resolutionObj.PayFineResolutionDate
                        };

                        AdminRespDomain.Save(adminResp);
                    }

                    tr.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException exc)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, exc.Message);
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        private class ResolutionProxy
        {
            public DateTime? DocumentDate { get; set; }

            public decimal? PenaltyAmount { get; set; }

            public string SupervisoryOrgCode { get; set; }

            public int ProtocolViolationsCount { get; set; }

            public DateTime? PayFineResolutionDate { get; set; }
        }
    }
}
