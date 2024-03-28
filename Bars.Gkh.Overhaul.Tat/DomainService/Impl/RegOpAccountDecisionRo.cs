namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.Gkh.Enums.Decisions;
    using Enums;
    using Gkh.Entities;
    using Overhaul.DomainService;
    using Entities;
    using Enum;
    using Castle.Windsor;

    public class RegOpAccountDecisionRo : IRegOpAccountDecisionRo
    {
        public IWindsorContainer Container { get; set; }

        public IRepository<RealityObject> RobjectRepository { get; set; }

        public IDomainService<BasePropertyOwnerDecision> DecisionDomain { get; set; }
        
        public Dictionary<long, CrFundFormationDecisionType> GetRobjectFormFundCr(long[] muIds, DateTime endDate)
        {
            var result = RobjectRepository.GetAll()
                .WhereIf(muIds.Any(), x => muIds.Contains(x.Municipality.Id))
                .Select(x => x.Id)
                .AsEnumerable()
                .ToDictionary(x => x, y => CrFundFormationDecisionType.Unknown);

            DecisionDomain.GetAll()
                .WhereIf(muIds.Any(), x => muIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                .WhereIf(endDate != DateTime.MinValue, x => x.PropertyOwnerProtocol.DocumentDate <= endDate)
                .Where(x => x.MethodFormFund.HasValue)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.PropertyOwnerProtocol.DocumentDate,
                    x.MethodFormFund
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ForEach(p =>
                {
                    var latest = p.OrderByDescending(x => x.DocumentDate).First();
                    var methodFormFund = latest.MethodFormFund.GetValueOrDefault();

                    result[p.Key] = methodFormFund == MethodFormFundCr.RegOperAccount
                            ? CrFundFormationDecisionType.RegOpAccount
                            : methodFormFund == MethodFormFundCr.SpecialAccount
                            ? CrFundFormationDecisionType.SpecialAccount : CrFundFormationDecisionType.Unknown;
                });

            return result;
        }
    }
}