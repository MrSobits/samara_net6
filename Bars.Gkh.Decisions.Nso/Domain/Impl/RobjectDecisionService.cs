namespace Bars.Gkh.Decisions.Nso.Domain.Impl
{
    using B4;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Castle.Windsor;
    using DomainService;
    using Entities;
    using Entities.Decisions;
    using Entities.Proxies;
    using Gkh.Domain;
    using Gkh.Entities;
    using Overhaul.DomainService;
    using Overhaul.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Domain.EntityHistory;
    using Bars.Gkh.DomainService.Dict.RealEstateType;

    using NHibernate;

    /// <summary>
    /// Сервис протокола решения жилого дома
    /// </summary>
    public class RobjectDecisionService : IRobjectDecisionService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// ManOrgService
        /// </summary>
        public IManagingOrgRealityObjectService ManOrgService { get; set; }

        private Dictionary<Type, UltimateDecision> decisionsCache;

        /// <summary>
        /// realEstateTypeService
        /// </summary>
        public IRealEstateTypeService RealEstateTypeService { get; set; }

        /// <summary>
        /// Интерфейс сервиса для актуализации способа формирования фонда дома.
        /// <remarks>Устанавливает значение свойства AccountFormationVariant</remarks>
        /// </summary>
        public IRealtyObjectAccountFormationService RealtyObjectAccountFormationService { get; set; }

        public IEntityHistoryService<RealityObjectDecisionProtocol> OwnerDecisionHistoryService { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult Get(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAs<long>("protocolId");

            var protocol = this.Container.ResolveDomain<RealityObjectDecisionProtocol>().Get(protocolId)
                           ?? new RealityObjectDecisionProtocol { ProtocolDate = DateTime.Today };

            this.TransformProtocol(protocol, baseParams);

            var proxy = Activator.CreateInstance<UltimateDecisionProxy>();

            proxy.Protocol = protocol;

            this.WarmProtocolDecisionCache(protocol);

            proxy.AccountOwnerDecision = this.Get<AccountOwnerDecision>(protocol, baseParams);
            if (proxy.AccountOwnerDecision != null && proxy.AccountOwnerDecision.DecisionType == AccountOwnerDecisionType.Custom)
            {
                proxy.AccountOwnerDecision.IsChecked = true;
            }

            proxy.AccumulationTransferDecision = this.Get<AccumulationTransferDecision>(protocol, baseParams);
            proxy.CrFundFormationDecision = this.Get<CrFundFormationDecision>(protocol, baseParams, this.FundFormationAction);
            proxy.CreditOrgDecision = this.Get<CreditOrgDecision>(protocol, baseParams, this.CreditOrgAction);
            proxy.MinFundAmountDecision = this.Get<MinFundAmountDecision>(protocol, baseParams, this.MinFundAmountAction);
            proxy.MkdManagementDecision = this.Get<MkdManagementDecision>(protocol, baseParams, this.MkdManageAction);
            proxy.MonthlyFeeAmountDecision = this.Get<MonthlyFeeAmountDecision>(protocol, baseParams, this.MonthlyFeeAction);
            proxy.JobYearDecision = this.Get<JobYearDecision>(protocol, baseParams, this.JobDecisionAction);
            proxy.AccountManagementDecision = this.Get<AccountManagementDecision>(protocol, baseParams);
            proxy.PenaltyDelayDecision = this.Get<PenaltyDelayDecision>(protocol, baseParams);

            if (protocol.RealityObject != null)
            {
                var payDecision = new PaymentAndFundDecisions();
                payDecision.Init(this.Container, protocol.RealityObject.Municipality.Id);

                proxy.PaymentAndFundDecisions = payDecision;
            }

            return new BaseDataResult(proxy);
        }

        /// <summary>
        /// SaveOrUpdateDecisions
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult SaveOrUpdateDecisions(BaseParams baseParams)
        {
            var proxy = baseParams.Params.ReadClass<UltimateDecisionProxy>();

            if (proxy == null)
            {
                return new BaseDataResult(false, "Нет данных для сохранения!");
            }

            var roId = baseParams.Params.GetAsId("roId");

            var protocol = proxy.Protocol;
            var protocolId = proxy.Protocol?.Id;
            var isCreate = protocolId.HasValue;
            var isUpdate = isCreate && protocolId.Value > 0;

            if (isUpdate)
            {
                this.OwnerDecisionHistoryService.StoreEntity(protocolId.Value);
            }

            this.SaveOrUpdateInternal(baseParams, proxy);

            if (isUpdate)
            {
                this.OwnerDecisionHistoryService.LogUpdate(protocol, new RealityObject { Id = roId });
            } else if (isCreate)
            {
                this.OwnerDecisionHistoryService.LogCreate(protocol, new RealityObject { Id = roId });
            }

            return new BaseDataResult(proxy);
        }

        private void SaveOrUpdateInternal(BaseParams baseParams, UltimateDecisionProxy proxy)
        {
            var roid = baseParams.Params.GetAsId("roId");

            var properties = proxy.GetType().GetProperties();

            dynamic protocol = null;
            var protocolProperty = properties.FirstOrDefault(x => typeof(RealityObjectDecisionProtocol) == x.PropertyType);

            if (protocolProperty != null)
            {
                properties = properties.Except(new[] { protocolProperty }).ToArray();
                protocol = protocolProperty.GetValue(proxy, new object[0]);

                protocol.RealityObject = new RealityObject { Id = roid };

                if (protocol != null)
                {
                    var domain = this.Container.ResolveDomain<RealityObjectDecisionProtocol>();

                    using (this.Container.Using(domain))
                    {
                        var file = baseParams.Files.FirstOrDefault(x => x.Key == "File").Value;
                        if (file != null)
                        {
                            var fileManager = this.Container.Resolve<IFileManager>();

                            using (this.Container.Using(fileManager))
                            {
                                protocol.File = fileManager.SaveFile(file);
                            }
                        }

                        if (protocol.Id > 0)
                        {
                            domain.Update(protocol);
                        }
                        else
                        {
                            domain.Save(protocol);
                        }
                    }
                }
            }


            foreach (var property in properties)
            {
                var value = property.GetValue(proxy, new object[0]);

                if (value == null)
                {
                    continue;
                }

                var protocolProp = value.GetType()
                    .GetProperties()
                    .FirstOrDefault(x => typeof(RealityObjectDecisionProtocol) == x.PropertyType);

                if (protocolProp.IsNotNull())
                {
                    protocolProp.SetValue(value, protocol, new object[0]);
                }

                var id = value.GetType().GetProperty("Id").GetValue(value, new object[0]).To<long>();

                if (typeof(IEntity).IsAssignableFrom(property.PropertyType))
                {
                    var domainType = typeof(IDomainService<>).MakeGenericType(property.PropertyType);
                    var domain = this.Container.Resolve(domainType);


                    using (this.Container.Using(domain))
                    {
                        var fileProp =
                            value.GetType().GetProperties().FirstOrDefault(x => x.PropertyType == typeof(FileInfo));

                        var file = baseParams.Files.FirstOrDefault(x => x.Key == fileProp.Return(p => p.Name)).Value;

                        if (fileProp != null &&
                            file != null)
                        {
                            fileProp.SetValue(value, this.Container.Resolve<IFileManager>().SaveFile(file), new object[0]);
                        }

                        var isCheckedVal =
                            value.GetType().GetProperty("IsChecked").GetValue(value, new object[0]).ToStr();

                        var isChecked = isCheckedVal.ToBool() || isCheckedVal.ToUpperInvariant() == "ON";

                        if (!isChecked)
                        {
                            if (id > 0)
                            {
                                var delete = domain.GetType().GetMethod("Delete", new[] { typeof(long) });
                                delete.Invoke(domain, new object[] { id });
                            }

                            continue;
                        }

                        if (id > 0)
                        {
                            var update = domain.GetType().GetMethod("Update", new[] { property.PropertyType });
                            update.Invoke(domain, new[] { value });
                        }
                        else
                        {
                            var save = domain.GetType().GetMethod("Save", new[] { property.PropertyType });
                            save.Invoke(domain, new[] { value });
                        }
                    }
                }
            }

            var roId = proxy.Protocol?.RealityObject?.Id;
            if (this.RealtyObjectAccountFormationService.IsNotNull() && roId.HasValue)
            {
                this.RealtyObjectAccountFormationService.ActualizeAccountFormationType(roId.Value);
            }
        }

        private MkdManagementDecision MkdManageAction(
            MkdManagementDecision decision,
            RealityObjectDecisionProtocol protocol,
            BaseParams baseParams)
        {
            if (decision != null &&
               decision.Decision != null)
            {
                var shortName = decision.Decision.Return(x => x.Contragent).Return(x => x.ShortName);

                if (shortName.IsEmpty())
                {
                    shortName = decision.Decision.Return(x => x.Contragent).Return(x => x.Name);
                }

                decision.Decision.ContragentShortName = shortName;
            }

            return decision;
        }

        private JobYearDecision JobDecisionAction(
            JobYearDecision decision,
            RealityObjectDecisionProtocol protocol,
            BaseParams baseParams)
        {
            decision = decision ?? new JobYearDecision();

            var roId = baseParams.Params.GetAs<long>("roId");

            var roJobYearsService = this.Container.Resolve<IRealityObjectDpkrDataService>();

            using (this.Container.Using(roJobYearsService))
            {
                var roJobYears =
                    roJobYearsService.GetWorkInfoByRealityObject(this.Container.ResolveDomain<RealityObject>().Load(roId));

                if (decision.IsChecked)
                {
                    var except = roJobYears.Where(x => decision.JobYears.All(y => y.Job.Id != x.Job.Id)).ToList();

                    decision.JobYears.AddRange(
                        except.Select(
                            x => new RealtyJobYear
                            {
                                Job = x.Job,
                                PlanYear = x.Year
                            }));
                }
                else
                {
                    decision.JobYears.Clear();
                    decision.JobYears.AddRange(
                        roJobYears.Select(
                            x => new RealtyJobYear
                            {
                                Job = x.Job,
                                PlanYear = x.Year
                            }));
                }

                return decision;
            }
        }

        private MinFundAmountDecision MinFundAmountAction(MinFundAmountDecision decision, RealityObjectDecisionProtocol protocol, BaseParams baseParams)
        {
            decision = decision ?? new MinFundAmountDecision();


            // Т.к. пока нет значения - отдает 40%

            decision.Default = 40;

            return decision;
        }

        private CrFundFormationDecision FundFormationAction(
            CrFundFormationDecision decision,
            RealityObjectDecisionProtocol protocol,
            BaseParams baseParams)
        {
            return decision ?? (new CrFundFormationDecision
            {
                IsChecked = true,
                Decision = CrFundFormationDecisionType.SpecialAccount
            });
        }

        private CreditOrgDecision CreditOrgAction(
            CreditOrgDecision decision,
            RealityObjectDecisionProtocol protocol,
            BaseParams baseParams)
        {
            return decision ?? (new CreditOrgDecision
            {
                IsChecked = false
            });
        }

        private MonthlyFeeAmountDecision MonthlyFeeAction(
            MonthlyFeeAmountDecision decision,
            RealityObjectDecisionProtocol protocol,
            BaseParams baseParams)
        {
            decision = decision ?? new MonthlyFeeAmountDecision();

            var paymentSizeMu = this.Container.ResolveDomain<PaymentSizeMuRecord>();
            var paysizeRecDomain = this.Container.ResolveDomain<PaysizeRecord>();
            var realObjDomain = this.Container.ResolveDomain<RealityObject>();
            var paysizeRetDomain = this.Container.ResolveDomain<PaysizeRealEstateType>();

            using (this.Container.Using(paymentSizeMu, realObjDomain))
            {
                var mu = protocol.RealityObject?.Municipality.Id ?? 0;

                var roId = baseParams.Params.GetAsId("roId");

                var realObj = realObjDomain.GetAll().Where(x => x.Id == roId);

                if (paysizeRetDomain.GetAll().Any())
                {
                    var realEstateType = this.RealEstateTypeService.GetRealEstateTypes(realObj);

                    var paysizeRet =
                        paysizeRetDomain.GetAll()
                            .Where(
                                x =>
                                    realEstateType[roId].Contains(x.RealEstateType.Id) && x.Record.Municipality.Id == mu);

                    var defaults =
                        paysizeRecDomain.GetAll()
                            .WhereIf(mu > 0, x => x.Municipality.Id == mu)
                            .WhereIf(mu == 0,
                                x => realObj.Any(y => y.Municipality.Id == x.Municipality.Id))
                            .Where(x => !paysizeRet.Any(y => y.Record.Id == x.Id && y.Record.Value == 0))
                            .ToList();

                    decision.Defaults.AddRange(
                        defaults.Select(
                            x => new PeriodMonthlyFee
                            {
                                Value = x.Value.ToDecimal(),
                                From = x.Paysize.DateStart,
                                To = x.Paysize.DateEnd
                            }));

                    decision.Defaults.AddRange(
                        paysizeRet
                        .Where(x => x.Value != 0)
                        .Select(
                            x => new PeriodMonthlyFee
                            {
                                Value = x.Value.ToDecimal(),
                                From = x.Record.Paysize.DateStart,
                                To = x.Record.Paysize.DateEnd
                            }));
                }
                else
                {
                    var defaults =
                        paysizeRecDomain.GetAll()
                            .WhereIf(mu > 0, x => x.Municipality.Id == mu)
                            .WhereIf(mu == 0,
                                x => realObj.Any(y => y.Municipality.Id == x.Municipality.Id))
                            .ToList();

                    decision.Defaults.AddRange(
                        defaults.Select(
                            x => new PeriodMonthlyFee
                            {
                                Value = x.Value.ToDecimal(),
                                From = x.Paysize.DateStart,
                                To = x.Paysize.DateEnd
                            }));
                }


                return decision;
            }
        }

        private void TransformProtocol(RealityObjectDecisionProtocol protocol, BaseParams baseParams)
        {
            protocol = protocol ?? new RealityObjectDecisionProtocol();

            var roId = baseParams.Params.GetAs<long>("roId");

            var manOrgContract = this.ManOrgService.GetManOrgOnDate(new RealityObject { Id = roId }, protocol.DateStart);

            if (manOrgContract != null)
            {
                if (manOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                {
                    protocol.ManOrgName = TypeContractManOrg.DirectManag.GetEnumMeta().Display;
                }
                else if (manOrgContract.ManagingOrganization != null)
                {
                    protocol.ManOrgName = manOrgContract.ManagingOrganization.Contragent.Name;
                }
            }
        }

        private T Get<T>(
            RealityObjectDecisionProtocol protocol,
            BaseParams baseParams,
            Func<T, RealityObjectDecisionProtocol, BaseParams, T> action = null)
            where T : UltimateDecision
        {
            var entity = (this.decisionsCache.Get(typeof(T)) as T);

            if (action != null)
            {
                return action(entity, protocol, baseParams);
            }

            return entity;
        }

        private void WarmProtocolDecisionCache(RealityObjectDecisionProtocol protocol)
        {
            var decisions = this.Container.ResolveDomain<UltimateDecision>().GetAll()
                .Where(x => x.Protocol.Id == protocol.Id)
                .ToList();

            //используется фича хибера
            //при получении сущностей базового класса загружает все сабклассы
            this.decisionsCache =
                decisions
                    .Select(x => new
                    {
                        Type = x.GetType(),
                        Decision = x
                    })
                    .GroupBy(x => x.Type)
                    .ToDictionary(x => x.Key, y => y.First().Decision);
        }
    }
}