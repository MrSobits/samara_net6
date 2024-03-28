namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Gkh.Utils;

    public class PaymentPenaltiesBasePersonalAccountController : BaseController
    {
        private List<long> _userMuIds;
        public IGkhUserManager UserManager { get; set; }

        public ActionResult List(BaseParams baseParams)
        {
            var basePersonalAccDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var payPenaltiesExcPersAccDomain = Container.ResolveDomain<PaymentPenaltiesExcludePersAcc>();
            var loadParams = baseParams.GetLoadParam();
            
            try
            {
                var payPenaltiesId = baseParams.Params.ContainsKey("payPenaltiesId")
                                     ? baseParams.Params.GetAs<long>("payPenaltiesId")
                                     : 0;
                var existRecs = payPenaltiesExcPersAccDomain.GetAll()
                        .Where(x => x.PaymentPenalties.Id == payPenaltiesId)
                        .Select(x => x.PersonalAccount.Id)
                        .ToHashSet();

                var query = basePersonalAccDomain.GetAll()
                    .Where(x => !existRecs.Contains(x.Id))
                    .Select(x => new
                                     {
                                         x.Id,
                                         RoomAddress = x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum,
                                         MuId = x.Room.RealityObject.Municipality.Id,
                                         SettleId = (long?)x.Room.RealityObject.MoSettlement.Id,
                                         Municipality = x.Room.RealityObject.Municipality.Name,
                                         x.PersonalAccountNum,
                                         RoId = x.Room.RealityObject.Id
                                     });

                var crFundType = baseParams.Params.ContainsKey("crFoundType") && !baseParams.Params.GetAs<string>("crFoundType").IsEmpty()
                    ? baseParams.Params.GetAs("crFoundType", CrFundFormationDecisionType.Unknown)
                    : CrFundFormationDecisionType.Unknown;

                var userMuIds = this.GetUserMuIds();
                List<long> listRoIds = null;
                if (crFundType != CrFundFormationDecisionType.Unknown)
                {
                    listRoIds = this.GetRoCrFundType(crFundType);
                }

                var outputData = query
                    .WhereIf(userMuIds.Count > 0, x => userMuIds.Contains(x.MuId) || (x.SettleId.HasValue && userMuIds.Contains(x.SettleId.Value)))
                    .WhereIf(crFundType != CrFundFormationDecisionType.Unknown && listRoIds != null, x => listRoIds.Contains(x.RoId))
                    .Filter(loadParams, Container);

                var totalCount = outputData.Count();
                var pagedData = outputData.Order(loadParams).Paging(loadParams);

                var result = pagedData
                    .ToArray()
                    .Select(x => new
                    {
                        x.Id,
                        x.Municipality,
                        x.RoomAddress,
                        x.PersonalAccountNum
                    })
                    .ToList();

                return new JsonListResult(result, totalCount);
            }
            finally
            {
                this.Container.Release(basePersonalAccDomain);
                this.Container.Release(payPenaltiesExcPersAccDomain);
            }
        }

        private List<long> GetUserMuIds()
        {
            if (this.UserManager != null)
            {
                return this._userMuIds ?? (this._userMuIds = this.UserManager.GetMunicipalityIds());
            }
            
            return new List<long>();
        }

        private List<long> GetRoCrFundType(CrFundFormationDecisionType crFundType)
        {
            var govDecisionDomain = this.Container.Resolve<IDomainService<GovDecision>>();
            var roDecisionsService = this.Container.Resolve<IRealityObjectDecisionsService>();
            var roRepo = this.Container.ResolveRepository<RealityObject>();

            try
            {
                // получаем решения для домов
                var dictRo = roDecisionsService.GetActualDecisionForCollection<CrFundFormationDecision>(roRepo.GetAll(), true);

                var govProtocols = govDecisionDomain.GetAll()
                    .Where(x => x.State.FinalState)
                    .Select(x => new
                    {
                        Ro = x.RealityObject,
                        CrFundFormationDecisionType = x.FundFormationByRegop
                            ? CrFundFormationDecisionType.RegOpAccount
                            : CrFundFormationDecisionType.SpecialAccount,
                        x.ProtocolDate
                    })
                    .ToList();

                foreach (var govProtocol in govProtocols)
                {
                    if (dictRo.ContainsKey(govProtocol.Ro))
                    {
                        var dec = dictRo[govProtocol.Ro];

                        if (dec.Protocol != null && dec.Protocol.ProtocolDate < govProtocol.ProtocolDate)
                        {
                            dec.Decision = govProtocol.CrFundFormationDecisionType;
                        }
                    }
                    else
                    {
                        var protocol = new RealityObjectDecisionProtocol { ProtocolDate = govProtocol.ProtocolDate };
                        var decision = new CrFundFormationDecision
                        {
                            Decision = govProtocol.CrFundFormationDecisionType,
                            Protocol = protocol
                        };
                        dictRo.Add(govProtocol.Ro, decision);
                    }
                }

                var listRoIds = dictRo.Where(kvp => kvp.Value != null && kvp.Value.Decision == crFundType).Select(kvp => kvp.Key.Id).ToList();

                if (crFundType == CrFundFormationDecisionType.Unknown)
                {
                    var roWithDecision = dictRo.Select(x => x.Key.Id).ToList();
                    listRoIds.AddRange(roRepo.GetAll().Where(x => !roWithDecision.Contains(x.Id)).Select(x => x.Id));
                }

                return listRoIds;
            }
            finally 
            {
                this.Container.Release(govDecisionDomain);
                this.Container.Release(roDecisionsService);
                this.Container.Release(roRepo);
            }
        }
    }
}