namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    using Castle.Windsor;

    public class LongTermPrObjectService : ILongTermPrObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetOrgForm(BaseParams baseParams)
        {
            var realObjId = baseParams.Params.GetAs<long>("realObjId");
            var currentDate = DateTime.Now.Date;
            var moContractRealObjDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            using (this.Container.Using(moContractRealObjDomain))
            {
                var contracts = moContractRealObjDomain.GetAll()
                    .Where(x => x.RealityObject.Id == realObjId)
                    .Select(x => new
                    {
                        x.RealityObject.NumberApartments,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.EndDate,
                        x.ManOrgContract.TypeContractManOrgRealObj,
                        ManOrgId = (long?) x.ManOrgContract.ManagingOrganization.Id,
                        TypeManagement = (TypeManagementManOrg?) x.ManOrgContract.ManagingOrganization.TypeManagement
                    })
                    .Where(x => !x.StartDate.HasValue || x.StartDate <= currentDate)
                    .Where(x => !x.EndDate.HasValue || x.EndDate >= currentDate)
                    .OrderByDescending(x => x.EndDate)
                    .ToArray();

                if (!contracts.Any())
                {
                    return new BaseDataResult(false, "На текущую дату отсутствует договор управления");
                }

                MoOrganizationForm orgForm;

                var firstContract = contracts.First();

                if (contracts.Any(x => x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                    && contracts.Any(x => x.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj))
                {
                    orgForm = MoOrganizationForm.ManOrg;
                }
                else if (firstContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                {
                    orgForm = MoOrganizationForm.DirectManag;
                }
                else if (firstContract.TypeManagement == TypeManagementManOrg.UK)
                {
                    orgForm = MoOrganizationForm.ManOrg;
                }
                else if (firstContract.TypeManagement == TypeManagementManOrg.JSK)
                {
                    orgForm = MoOrganizationForm.Jsk;
                }
                else
                {
                    var numberApartments =
                        moContractRealObjDomain.GetAll()
                            .Where(x => x.ManOrgContract.ManagingOrganization.Id == firstContract.ManOrgId)
                            .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= currentDate)
                            .Select(x => new { x.RealityObject.Id, x.RealityObject.NumberApartments })
                            .AsEnumerable()
                            .Distinct(x => x.Id)
                            .Select(x => x.NumberApartments)
                            .ToArray();

                    if (numberApartments.Length == 1)
                    {
                        orgForm = MoOrganizationForm.TsjLess30Apartments;
                    }
                    else
                    {
                        var numberApartmentCnt = numberApartments.Sum().ToInt();

                        orgForm = numberApartmentCnt > 30
                            ? MoOrganizationForm.TsjOver30Apartments
                            : MoOrganizationForm.TsjLess30Apartments;
                    }
                }

                return new BaseDataResult(orgForm);
            }
        }

        public IDataResult GetManagingOrganization(BaseParams baseParams)
        {
            var realObjId = baseParams.Params.GetAs<long>("realObjId");
            var typeOrganization = baseParams.Params.GetAs<TypeOrganization>("typeOrganization");
            var currentDate = DateTime.Today;
            var manOrgContractRealityObjectDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            using (this.Container.Using(manOrgContractRealityObjectDomain))
            {
                var managingOrganization = manOrgContractRealityObjectDomain.GetAll()
                    .WhereIf(typeOrganization == TypeOrganization.JSK,
                        x => x.ManOrgContract.ManagingOrganization.TypeManagement == TypeManagementManOrg.JSK)
                    .WhereIf(typeOrganization == TypeOrganization.TSJ,
                        x => x.ManOrgContract.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ)
                    .Where(x => x.RealityObject.Id == realObjId)
                    .Where(x => !x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate <= currentDate)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= currentDate)
                    .OrderByDescending(x => x.ManOrgContract.EndDate)
                    .Select(x => new
                    {
                        x.ManOrgContract.ManagingOrganization.Id,
                        ContragentName = x.ManOrgContract.ManagingOrganization.Contragent.Name
                    })
                    .FirstOrDefault();

                return new BaseDataResult(managingOrganization);
            }
        }

        public IDataResult SaveDecision(BaseParams baseParams)
        {
            InTransaction(() =>
            {
                var saveParams =
                    baseParams.Params.Read<SaveParam<BasePropertyOwnerDecision>>()
                        .Execute(container => Converter.ToSaveParam<BasePropertyOwnerDecision>(container, false));
                var minAmountDecisionDomain = this.Container.Resolve<IDomainService<MinAmountDecision>>();
                var listServicesDecisionDomain = this.Container.Resolve<IDomainService<ListServicesDecision>>();
                var minFundSizeDecisionDomain = this.Container.Resolve<IDomainService<MinFundSizeDecision>>();
                var regOpAccountDecisionDomain = this.Container.Resolve<IDomainService<RegOpAccountDecision>>();
                var specialAccountDecisionDomain = this.Container.Resolve<IDomainService<SpecialAccountDecision>>();
                var regOperator = this.Container.Resolve<IDomainService<RegOperator>>();


                using (this.Container.Using(minAmountDecisionDomain,
                    listServicesDecisionDomain,
                    minFundSizeDecisionDomain,
                    regOpAccountDecisionDomain,
                    specialAccountDecisionDomain,
                    regOperator))
                {
                    foreach (var record in saveParams.Records)
                    {
                        var decision = record.AsObject();

                        if (decision.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SetMinAmount)
                        {
                            minAmountDecisionDomain.Save(new MinAmountDecision
                            {
                                MethodFormFund = decision.MethodFormFund,
                                MoOrganizationForm = decision.MoOrganizationForm,
                                PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                                RealityObject = decision.RealityObject,
                                PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                            });
                        }
                        else if (decision.PropertyOwnerDecisionType == PropertyOwnerDecisionType.ListOverhaulServices)
                        {
                            listServicesDecisionDomain.Save(new ListServicesDecision
                            {
                                MethodFormFund = decision.MethodFormFund,
                                MoOrganizationForm = decision.MoOrganizationForm,
                                PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                                RealityObject = decision.RealityObject,
                                PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                            });
                        }
                        else if (decision.PropertyOwnerDecisionType == PropertyOwnerDecisionType.MinCrFundSize)
                        {
                            minFundSizeDecisionDomain.Save(new MinFundSizeDecision
                            {
                                MethodFormFund = decision.MethodFormFund,
                                MoOrganizationForm = decision.MoOrganizationForm,
                                PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                                SubjectMinFundSize = 40,
                                RealityObject = decision.RealityObject,
                                PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                            });
                        }
                        else if (decision.MethodFormFund == MethodFormFundCr.RegOperAccount)
                        {
                            regOpAccountDecisionDomain.Save(new RegOpAccountDecision
                            {
                                MethodFormFund = decision.MethodFormFund,
                                MoOrganizationForm = decision.MoOrganizationForm,
                                PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                                RealityObject = decision.RealityObject,
                                PropertyOwnerProtocol = decision.PropertyOwnerProtocol,
                                RegOperator = regOperator.GetAll().FirstOrDefault()
                            });
                        }
                        else
                        {
                            specialAccountDecisionDomain.Save(new SpecialAccountDecision
                            {
                                MethodFormFund = decision.MethodFormFund,
                                MoOrganizationForm = decision.MoOrganizationForm,
                                PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                                RealityObject = decision.RealityObject,
                                TypeOrganization = TypeOrganization.RegOperator,
                                PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                            });
                        }
                    }
                }
            });

            return new BaseDataResult();
        }

        protected virtual void InTransaction(Action action)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }
}