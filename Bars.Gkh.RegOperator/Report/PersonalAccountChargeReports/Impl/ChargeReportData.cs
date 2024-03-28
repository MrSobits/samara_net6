namespace Bars.Gkh.RegOperator.Report.PersonalAccountChargeReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities;
    using Gkh.Utils;

    public class ChargeReportData : IChargeReportData
    {
        public IDomainService<BasePersonalAccount> BasePersonalAccService { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryDomain { get; set; }
        public IDomainService<PersonalAccountCharge> PersonalAccountChargeDomain { get; set; }
        public IRealityObjectDecisionsService RealityObjectDecisionsService { get; set; }

        /// <summary>
        /// получаем список начислений по 1 или множеству выбранных Районов + период
        /// </summary>
        public List<RaionChargeRecord> GetRaionChargeRecords(long[] mrIds, long periodId)
        {
            var result = new List<RaionChargeRecord>();

            var listRecords = GetRecords(periodId, mrIds, null, null, null);

            // Получаем данные сгруппированные по районам
            listRecords
                .GroupBy(x => new {x.RaionId, x.RaionName})
                           .ToDictionary(x => x.Key, y => y.ToList())
                           .ForEach(
                               kvp => result.Add(new RaionChargeRecord
                               {
                                   RaionId = kvp.Key.RaionId,
                                   RaionName = kvp.Key.RaionName,
                                   MinSize = kvp.Value.Sum(x => x.MinSize),
                                   OverMinSize = kvp.Value.Sum(x => x.OverMinSize),
                                   Penalty = kvp.Value.Sum(x => x.Penalty),
                                   Total = kvp.Value.Sum(x => x.Total)
                               }));

            return result;
        }

        /// <summary>
        /// получаем список начислений по 1 району + 1 или множеству выбранных МО + период
        /// </summary>
        public List<MunicipalityChargeRecord> GetMunicipalityChargeRecords(long mrId, long[] moIds, long periodId)
        {
            var result = new List<MunicipalityChargeRecord>();

            var listRecords = GetRecords(periodId, mrId > 0 ? new[] { mrId } : null, moIds, null, null);

            // Получаем данные сгруппированные по МО
            listRecords.GroupBy(x => new { x.MunicipalityId, x.MunicipalityName })
                           .ToDictionary(x => x.Key, y => y.ToList())
                           .ForEach(
                               kvp => result.Add(new MunicipalityChargeRecord
                               {
                                   MunicipalityId = kvp.Key.MunicipalityId,
                                   MunicipalityName = kvp.Key.MunicipalityName,
                                   MinSize = kvp.Value.Sum(x => x.MinSize),
                                   OverMinSize = kvp.Value.Sum(x => x.OverMinSize),
                                   Penalty = kvp.Value.Sum(x => x.Penalty),
                                   Total = kvp.Value.Sum(x => x.Total)
                               }));

            return result;
        }

        /// <summary>
        /// получаем список начислений по 1 МО + 1 или множеству выбранных МКД + период
        /// </summary>
        public List<RealityObjectChargeRecord> GetRealityObjectChargeRecords(long moId, long[] roIds, long periodId)
        {
            var result = new List<RealityObjectChargeRecord>();

            var listRecords = GetRecords(periodId, null, moId > 0 ? new[] { moId } : null, roIds, null);

            // Получаем данные сгруппированные по Домам
            listRecords.GroupBy(x => new {x.RealityObjectId, x.Address})
                .ToDictionary(x => x.Key, y => y.ToList())
                .ForEach(kvp => result.Add(new RealityObjectChargeRecord
                {
                    RealityObjectId = kvp.Key.RealityObjectId,
                    Address = kvp.Key.Address,
                    MinSize = kvp.Value.Sum(x => x.MinSize),
                    OverMinSize = kvp.Value.Sum(x => x.OverMinSize),
                    Penalty = kvp.Value.Sum(x => x.Penalty),
                    Total = kvp.Value.Sum(x => x.Total)
                }));
            return result;
        }

        /// <summary>
        /// получаем список начислений по 1 МКД + 1 или множеству выбранных сетов + период
        /// </summary>
        public List<AccountChargeRecord> GetOwnerChargeRecords(long roId, long[] accountIds, long periodId)
        {
            return GetRecords(periodId, null, null, roId > 0 ? new[] { roId } : null, accountIds);
        }

        /// <summary>
        /// метод необходимый для других методов поулчаем все начисления для счетов
        /// </summary>
        private List<AccountChargeRecord> GetRecords(long periodId, long[] mrIds, long[] moIds, long[] roIds, long[] accountIds)
        {
            var accountQuery = BasePersonalAccService.GetAll()
                .WhereIf(mrIds != null && mrIds.Any(), x => mrIds.Contains(x.Room.RealityObject.Municipality.Id))
                .WhereIf(moIds != null && moIds.Any(), x => moIds.Contains(x.Room.RealityObject.MoSettlement.Id))
                .WhereIf(roIds != null && roIds.Any(), x => roIds.Contains(x.Room.RealityObject.Id))
                .WhereIf(accountIds != null && accountIds.Any(), x => accountIds.Contains(x.Id));

            if (RealityObjectDecisionsService == null)
            {
                throw new Exception("Не удалось получить реализацию IRealityObjectDecisionsService");
            }

            var regopDecRoIds = RealityObjectDecisionsService
                .GetRobjectsFundFormation(((IQueryable<long>)null))
                .Where(x => x.Value.FirstOrDefault() != null && x.Value.First().Item2 == CrFundFormationDecisionType.RegOpAccount)
                .Select(x => x.Key)
                .ToHashSet();

            var allPeriodSummary =
            PeriodSummaryDomain.GetAll()
                .Where(x => x.Period.Id == periodId)
                .Where(x => accountQuery.Any(y => y.Id == x.PersonalAccount.Id))
                .Where(x => x.PersonalAccount.Id == 70240)
                .Select(x => new
                {
                    x.PersonalAccount.Id,
                    roId = x.PersonalAccount.Room.RealityObject.Id,
                    ChargedByBaseTariff = x.ChargedByBaseTariff + x.RecalcByBaseTariff,
                    ChargeByDecision = x.ChargeTariff - x.ChargedByBaseTariff + x.RecalcByDecisionTariff,
                    x.Penalty,
                    ChargeTotal = x.ChargeTariff + x.RecalcByBaseTariff + x.Penalty + x.BaseTariffChange
                })
                .AsEnumerable()
                .Where(x => regopDecRoIds.Contains(x.roId))
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => new
                        {
                                                   chargedTariff = x.Sum(y => y.ChargedByBaseTariff),
                                                   chargedOwnerDecision = x.Sum(y => y.ChargeByDecision),
                                                   penalty = x.Sum(y => y.Penalty),
                                                   chargeTotal = x.Sum(y => y.ChargeTotal)
                });

            return accountQuery
                .Where(x => regopDecRoIds.Contains(x.Room.RealityObject.Id))
                .Select(x => new
                {
                    AccountId = x.Id,
                    Room = x.Room.RoomNum,
                    Owner = x.AccountOwner.Name,
                    RaionId = x.Room.RealityObject.Municipality.Id,
                    RaionName = x.Room.RealityObject.Municipality.Name,
                    MunicipalityId = x.Room.RealityObject.MoSettlement != null ? x.Room.RealityObject.MoSettlement.Id : 0L,
                    MunicipalityName = x.Room.RealityObject.MoSettlement != null ? x.Room.RealityObject.MoSettlement.Name : null,
                    RealityObjectId = x.Room.RealityObject.Id,
                    x.Room.RealityObject.Address
                })
                .ToList()
                .Select(x => new AccountChargeRecord
                {
                    AccountId = x.AccountId,
                    Room = x.Room,
                    Owner = x.Owner,
                    RaionId = x.RaionId,
                    RaionName = x.RaionName,
                    MunicipalityId = x.MunicipalityId,
                    MunicipalityName = x.MunicipalityName,
                    RealityObjectId = x.RealityObjectId,
                    Address = x.Address,
                    MinSize = allPeriodSummary.ContainsKey(x.AccountId) ? allPeriodSummary[x.AccountId].chargedTariff : 0m,
                    OverMinSize = allPeriodSummary.ContainsKey(x.AccountId) ? allPeriodSummary[x.AccountId].chargedOwnerDecision : 0m,
                    Penalty = allPeriodSummary.ContainsKey(x.AccountId) ? allPeriodSummary[x.AccountId].penalty : 0m,
                    Total = allPeriodSummary.ContainsKey(x.AccountId) ? allPeriodSummary[x.AccountId].chargeTotal : 0m
                })
                .OrderBy(x => x.RaionName)
                .ThenBy(x => x.MunicipalityName)
                .ThenBy(x => x.Address)
                .ThenBy(x => x.Room)
                .ToList();
        }
    }
}
