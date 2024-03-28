namespace Bars.Gkh.Decisions.Nso.DomainServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain.FormingOfCr;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.Dict.RealEstateType;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.DomainService;
    using Bars.Gkh.Overhaul.Entities;

    using Castle.Windsor;
    using B4.Modules.Mapping;

    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис для работы с "Протоколы и решения" (proxy к Bars.Gkh.Decisions.Nso)
    /// </summary>
    public class RealityObjectDecisionProtocolProxyService : FormingOfCrProvider, IRealityObjectDecisionProtocolProxyService
    {
        /// <summary>
        /// Домен сервис для Протокол решений собственников
        /// </summary>
        public IDomainService<RealityObjectDecisionProtocol> RealityObjectDecisionProtocolDomainService { get; set; }

        /// <summary>
        /// Домен сервис для Протокол решения органа государственной власти
        /// </summary>
        public IDomainService<GovDecision> GovDecisionDomainService { get; set; }

        /// <summary>
        /// Домен сервис для Протокол решения собственников жилья
        /// </summary>
        public IDomainService<UltimateDecision> UltimateDecisionDomainService { get; set; }

        /// <summary>
        /// Домен сервис для Сущность записи взноса на КР по муниципальному образованию
        /// </summary>
        public IDomainService<PaymentSizeMuRecord> PaymentSizeMuRecordDomainService { get; set; }

        /// <summary>
        /// Домен сервис для Муниципальное образование размера взноса на кр
        /// </summary>
        public IDomainService<PaysizeRecord> PaysizeRecordDomainService { get; set; }

        /// <summary>
        /// Домен сервис для Жилой дом
        /// </summary>
        public IDomainService<RealityObject> RealityObjectDomainService { get; set; }

        /// <summary>
        /// Домен сервис для Тип дома размера взноса на кр
        /// </summary>
        public IDomainService<PaysizeRealEstateType> PaysizeRealEstateTypeDomainService { get; set; }

        /// <summary>
        /// Сервис для Тип дома размера взноса на кр
        /// </summary>
        public IRealEstateTypeService RealEstateTypeService { get; set; }

        /// <summary>
        /// Сервис для работы с рег. оператором
        /// </summary>
        public IRegopService RegopService { get; set; }

        public IDomainService<FieldRequirement> FieldRequirementDomainService { get; set; }

        /// <summary>
        /// Получить прокси протокола или решения
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        /// <returns>Прокси протокола или решения</returns>
        public RealityObjectDecisionProtocolProxy GetBothProtocolProxy(RealityObject realityObject)
        {
            if (!realityObject.AccountFormationVariant.HasValue)
            {
                return null;
            }

            var name = string.Empty;
            var inn = string.Empty;
            if (this.RegopService.IsNotNull())
            {
                name = this.RegopService.GetCurrentRegOperator()?.Contragent.Name;
                inn = this.RegopService.GetCurrentRegOperator()?.Contragent.Inn;
            }

            var protocol = this.RealityObjectDecisionProtocolDomainService.GetAll()
                .Where(x => x.RealityObject.Id == realityObject.Id)
                .Where(x => x.State.FinalState && x.DateStart <= DateTime.Now)
                .Select(x => new
                {
                    x.Id,
                    x.ProtocolDate,
                    x.DocumentNum,
                    x.RealityObject
                })
                .AsEnumerable()
                .Select(
                    x => new RealityObjectDecisionProtocolProxy
                    {
                        Id = x.Id,
                        ProtocolDate = x.ProtocolDate,
                        ProtocolNumber = x.DocumentNum,
                        RegOpContragentName = name,
                        RegOpContragentInn = inn,
                        CrPaySize = this.GetPaysize(x.Id, x.RealityObject.Id, null),
                        DecisionType = CoreDecisionType.Owners
                    })
                .Union(this.GovDecisionDomainService.GetAll()
                        .Where(x => x.RealityObject.Id == realityObject.Id)
                        .Where(x => x.State.FinalState && x.DateStart <= DateTime.Now && x.FundFormationByRegop)
                        .Select(x => new
                        {
                            x.Id,
                            x.ProtocolDate,
                            x.ProtocolNumber,
                            x.RealityObject
                        })
                        .AsEnumerable()
                        .Select(
                            x => new RealityObjectDecisionProtocolProxy
                            {
                                Id = x.Id,
                                ProtocolDate = x.ProtocolDate,
                                ProtocolNumber = x.ProtocolNumber,
                                RegOpContragentName = name,
                                RegOpContragentInn = inn,
                                CrPaySize = this.GetPaysize(x.Id, x.RealityObject.Id, null),
                                DecisionType = CoreDecisionType.Government
                            }))
                .OrderByDescending(x => x.ProtocolDate)
                .FirstOrDefault();

            return protocol;
        }

        /// <summary>
        /// Получить прокси протокола или решения
        /// </summary>
        /// <param name="roId">Идентификатор жилого дома</param>
        /// <returns>Прокси протокола или решения</returns>
        public RealityObjectDecisionProtocolProxy GetBothProtocolProxy(long roId)
        {
            var ro = this.RealityObjectDomainService.Get(roId);

            if (ro.IsNull())
            {
                return null;
            }

            return this.GetBothProtocolProxy(ro);
        }

        /// <summary>
        /// Получить прокси протокола или решения
        /// </summary>
        /// <param name="realityObjects">Запрос домов</param>
        /// <returns>Прокси протокола или решения</returns>
        public Dictionary<long, RealityObjectDecisionProtocolProxy> GetBothProtocolProxy(IQueryable<RealityObject> realityObjects)
        {
            return this.GetInternal(realityObjects).ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.ProtocolDate).FirstOrDefault());
        }

        /// <inheritdoc />
        public Dictionary<long, List<RealityObjectDecisionProtocolProxy>> GetAllBothProtocolProxy(IQueryable<RealityObject> realityObjects)
        {
            return this.GetInternal(realityObjects).ToDictionary(x => x.Key, x => x.ToList());
        }

        /// <inheritdoc />
        public Dictionary<long, List<RealityObjectDecisionProtocolProxy>> GetAllBothProtocolByFormat(IQueryable<RealityObject> realityObjects)
        {
            return this.GetInternal(realityObjects, false).ToDictionary(x => x.Key, x => x.ToList());
        }

        private IEnumerable<IGrouping<long, RealityObjectDecisionProtocolProxy>> GetInternal(IQueryable<RealityObject> realityObjects, bool withPaySize = true)
        {
            var name = string.Empty;
            var inn = string.Empty;
            if (this.RegopService.IsNotNull())
            {
                name = this.RegopService.GetCurrentRegOperator()?.Contragent?.Name;
                inn = this.RegopService.GetCurrentRegOperator()?.Contragent?.Inn;
            }

            return this.RealityObjectDecisionProtocolDomainService.GetAll()
                .Where(x => realityObjects.Any(y => y.Id == x.RealityObject.Id))
                .Where(x => x.State.FinalState && x.DateStart <= DateTime.Now)
                .Select(x => new
                {
                    x.Id,
                    x.ExportId,
                    x.ProtocolDate,
                    x.DocumentNum,
                    x.RealityObject,
                })
                .AsEnumerable()
                .Select(x => new RealityObjectDecisionProtocolProxy
                {
                    Id = x.Id,
                    ExportId = x.ExportId,
                    ProtocolDate = x.ProtocolDate,
                    ProtocolNumber = x.DocumentNum,
                    RealityObjectId = x.RealityObject.Id,
                    RegOpContragentName = name,
                    RegOpContragentInn = inn,
                    CrPaySize = withPaySize ? this.GetPaysize(x.Id, x.RealityObject.Id, null) : default(decimal?),
                    DecisionType = CoreDecisionType.Owners
                })
                .Union(
                    this.GovDecisionDomainService.GetAll()
                        .Where(x => realityObjects.Any(y => y.Id == x.RealityObject.Id))
                        .Where(x => x.State.FinalState && x.DateStart <= DateTime.Now && x.FundFormationByRegop)
                        .Select(x => new
                        {
                            x.Id,
                            x.ExportId,
                            x.ProtocolDate,
                            x.ProtocolNumber,
                            x.RealityObject
                        })
                        .AsEnumerable()
                        .Select(x => new RealityObjectDecisionProtocolProxy
                        {
                            Id = x.Id,
                            ExportId = x.ExportId,
                            ProtocolDate = x.ProtocolDate,
                            ProtocolNumber = x.ProtocolNumber,
                            RealityObjectId = x.RealityObject.Id,
                            RegOpContragentName = name,
                            RegOpContragentInn = inn,
                            CrPaySize = withPaySize ? this.GetPaysize(x.Id, x.RealityObject.Id, null) : default(decimal?),
                            DecisionType = CoreDecisionType.Government
                        }))
                .GroupBy(x => x.RealityObjectId);
        }

        /// <summary>
        /// Получить размер взноса на КР (либо из протокола, либо из справочника взносов)
        /// </summary>
        /// <param name="protocolId">Идентификатор протокола</param>
        /// <param name="roId">Идентификатор жилого дома</param>
        /// <param name="yearStart">Дата начала периода</param>
        /// <returns>Размер взноса на КР</returns>
        public decimal? GetPaysize(long protocolId, long roId, DateTime? yearStart)
        {
            var paySize = 0m;

            var protocol = this.RealityObjectDecisionProtocolDomainService.Get(protocolId);
            if (protocol == null)
            {
                return paySize;
            }

            var monthlyFeeAmountDecision = this.UltimateDecisionDomainService.GetAll()
                .Where(x => x.Protocol.Id == protocolId)
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        Type = x.GetType(),
                        Decision = x
                    })
                .FirstOrDefault(x => x.Type == typeof(MonthlyFeeAmountDecision))
                .Return(x => x.Decision as MonthlyFeeAmountDecision);

            var paysizes = this.GetPaysizes(monthlyFeeAmountDecision, protocol, roId, yearStart);
            if (paysizes.Any())
            {
                if (paysizes.Any(x => x.To == null))
                {
                    paySize = paysizes.First(x => x.To == null).Value;
                }
                else
                {
                    paySize = paysizes.OrderByDescending(x => x.To).First().Value;
                }
            }

            return paySize;
        }

        /// <inheritdoc />
        public IDataResult CheckDecisionProtocol(RealityObject ro)
        {
            var protocols = this.RealityObjectDecisionProtocolDomainService.GetAll()
                .Where(x => x.RealityObject.Id == ro.Id)
                .Where(x => x.State.FinalState)
                .ToList();

            var govDecisions = this.GovDecisionDomainService.GetAll()
                .Where(x => x.RealityObject.Id == ro.Id)
                .Where(x => x.State.FinalState)
                .ToList();

            if (!protocols.Any() && !govDecisions.Any())
            {
                return new BaseDataResult(false, "Ни один протокол решения не добавлен");
            }

            var entityDescriptor = MappingSchema.GetEntityDescriptor<GovDecision>();

            List<GovDecsionProxy> govDecisionFields = new List<GovDecsionProxy>();

            foreach (var govDecisionField in govDecisionFields)
            {
                govDecisionFields.AddRange(
                    entityDescriptor.GetPropertyMapDescriptors()
                        .Select(
                            x => new GovDecsionProxy
                            {
                                PropertyName = x.Key.Name,
                                Description = x.Value.Name,
                                Value = x.Key.GetValue(govDecisionField) != null
                            })
                        .ToList());
            }

            var govDecisionRequirements = this.FieldRequirementDomainService.GetAll()
                .Where(x => x.RequirementId.StartsWith("Gkh.RealityObject.Protocols.GovDecision.Fields"))
                .Select(x => x.RequirementId.Replace("Gkh.RealityObject.Protocols.GovDecision.Fields.", string.Empty).Replace("_Rqrd", string.Empty))
                .ToList();

            var blankFields = govDecisionFields
                .Where(x => !x.Value)
                .Where(x => govDecisionRequirements.Contains(x.PropertyName))
                .Select(x => x.Description)
                .ToList();

            if (blankFields.Any())
            {
                var text = blankFields.AggregateWithSeparator(x => x, ", ");

                return new BaseDataResult(false, $"Заполнены не все обязательные поля: {text}");
            }

            return new BaseDataResult();
        }

        private List<PeriodMonthlyFee> GetPaysizes(MonthlyFeeAmountDecision decision, RealityObjectDecisionProtocol protocol, long roId, DateTime? year)
        {
            var results = new List<PeriodMonthlyFee>();
            if (decision != null && !decision.Decision.Any())
            {
                results.AddRange(this.GetDecisionPaysizes(decision));
            }
            else
            {
                results.AddRange(this.GetDefaultPaysizes(protocol, roId, year));
            }

            return results;
        }

        private IEnumerable<PeriodMonthlyFee> GetDecisionPaysizes(MonthlyFeeAmountDecision decision)
        {
            return decision.Decision;
        }

        private IEnumerable<PeriodMonthlyFee> GetDefaultPaysizes(RealityObjectDecisionProtocol protocol, long roId, DateTime? year)
        {
            var results = new List<PeriodMonthlyFee>();
            using (this.container.Using(this.PaymentSizeMuRecordDomainService, this.RealityObjectDomainService))
            {
                var mu = protocol.RealityObject != null ? protocol.RealityObject.Municipality.Id : 0;
                var realObj = this.RealityObjectDomainService.GetAll().Where(x => x.Id == roId);

                if (this.PaysizeRealEstateTypeDomainService.GetAll().Any())
                {
                    var realEstateType = this.RealEstateTypeService.GetRealEstateTypes(realObj);
                    var paysizeRet = this.PaysizeRealEstateTypeDomainService.GetAll()
                        .Where(x => realEstateType[roId].Contains(x.RealEstateType.Id) && x.Record.Municipality.Id == mu)
                        .WhereIf(year.HasValue, x => x.Record.Paysize.DateStart >= year.Value && x.Record.Paysize.DateEnd < year.Value.AddYears(1))
                        .Where(x => x.Value > 0);

                    var defaults = this.PaysizeRecordDomainService.GetAll()
                        .WhereIf(mu > 0, x => x.Municipality.Id == mu)
                        .WhereIf(mu == 0, x => realObj.Any(y => y.Municipality.Id == x.Municipality.Id))
                        .Where(x => !paysizeRet.Any(y => y.Record.Id == x.Id))
                        .WhereIf(year.HasValue, x => x.Paysize.DateStart >= year.Value && x.Paysize.DateEnd < year.Value.AddYears(1))
                        .ToList();

                    if (defaults.Any())
                    {
                        results.AddRange(
                            defaults.Select(
                                x => new PeriodMonthlyFee
                                {
                                    Value = x.Value.ToDecimal(),
                                    From = x.Paysize.DateStart,
                                    To = x.Paysize.DateEnd
                                }));
                    }
                    else
                    {
                        results.AddRange(
                            paysizeRet.AsEnumerable().Select(
                                x => new PeriodMonthlyFee
                                {
                                    Value = x.Value.ToDecimal(),
                                    From = x.Record.Paysize.DateStart,
                                    To = x.Record.Paysize.DateEnd
                                }).OrderByDescending(x => x.Value));
                    }
                }
                else
                {
                    var defaults = this.PaysizeRecordDomainService.GetAll()
                            .WhereIf(mu > 0, x => x.Municipality.Id == mu)
                            .WhereIf(mu == 0, x => realObj.Any(y => y.Municipality.Id == x.Municipality.Id))
                            .ToList();

                    results.AddRange(
                        defaults.Select(
                            x => new PeriodMonthlyFee
                            {
                                Value = x.Value.ToDecimal(),
                                From = x.Paysize.DateStart,
                                To = x.Paysize.DateEnd
                            }));
                }
            }

            return results;
        }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="crDomain"></param>
        /// <param name="govDecisionDomain"></param>
        /// <param name="accDomain"></param>
        public RealityObjectDecisionProtocolProxyService(
            IWindsorContainer container, 
            IDomainService<CrFundFormationDecision> crDomain, 
            IDomainService<GovDecision> govDecisionDomain, 
            IDomainService<AccountOwnerDecision> accDomain)
            : base(container, crDomain, govDecisionDomain, accDomain)
        {
        }

        private class GovDecsionProxy
        {
            public string PropertyName { get; set; }

            public string Description { get; set; }

            public bool Value { get; set; }
        }
    }
}