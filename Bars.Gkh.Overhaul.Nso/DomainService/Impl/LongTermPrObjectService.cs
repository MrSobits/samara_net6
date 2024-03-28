namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;

    using Castle.Windsor;

    public class LongTermPrObjectService : ILongTermPrObjectService
    {
        public IWindsorContainer Container { get; set; }
        
        public IDataResult ListHasDecisionRegopAccount(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var objectId = baseParams.Params.GetAs<long>("objectId");

            var domainService = Container.Resolve<IDomainService<RealityObject>>();

            var decisionDomainService = Container.Resolve<IDomainService<RegOpAccountDecision>>();

            var data = domainService.GetAll()
                .Where(x => x.Id != objectId)
                .Where(y => decisionDomainService.GetAll().Any(x => x.RealityObject.Id == y.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Address
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult GetOrgForm(BaseParams baseParams)
        {
            var realObjId = baseParams.Params.GetAs<long>("realObjId");
            var currentDate = DateTime.Now.Date;
            var moContractRealObjDomain = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            var contract = moContractRealObjDomain.GetAll()
                    .Where(x => x.RealityObject.Id == realObjId)
                    .Select(x => new
                    {
                        x.RealityObject.NumberApartments,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.EndDate,
                        x.ManOrgContract.TypeContractManOrgRealObj,
                        manOrgId = (long?)x.ManOrgContract.ManagingOrganization.Id,
                        TypeManagement = (TypeManagementManOrg?) x.ManOrgContract.ManagingOrganization.TypeManagement
                    })
                    .Where(x => x.StartDate <= currentDate)
                    .Where(x => !x.EndDate.HasValue || x.EndDate.HasValue && x.EndDate >= currentDate)
                    .OrderByDescending(x => x.EndDate)
                    .FirstOrDefault();

            if (contract == null)
            {
                return new BaseDataResult(false, "На текущую дату отсутствует договор управления");
            }

            MoOrganizationForm orgForm;

            if (contract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
            {
                orgForm = MoOrganizationForm.DirectManag;
            }
            else if (contract.TypeManagement == TypeManagementManOrg.UK)
            {
                orgForm = MoOrganizationForm.ManOrg;
            }
            else if (contract.TypeManagement == TypeManagementManOrg.JSK)
            {
                orgForm = MoOrganizationForm.Jsk;
            }
            else
            {
                var numberApartments = moContractRealObjDomain.GetAll()
                                       .Where(x => x.ManOrgContract.ManagingOrganization.Id == contract.manOrgId && 
                                           (!x.ManOrgContract.EndDate.HasValue
                                           || x.ManOrgContract.EndDate.HasValue
                                           && x.ManOrgContract.EndDate >= currentDate))
                                       .Select(x => x.RealityObject.NumberApartments)
                                       .ToArray();

                var numberApartmentCnt = numberApartments.Any() ? numberApartments.Sum(x => x.ToInt()) : 0;

                orgForm = contract.TypeManagement == TypeManagementManOrg.TSJ && numberApartmentCnt > 30
                                                ? MoOrganizationForm.TsjOver30Apartments
                                                : MoOrganizationForm.TsjLess30Apartments;
            }

            return new BaseDataResult(orgForm);
        }

        public IDataResult SaveDecision(BaseParams baseParams)
        {
            InTransaction(() =>
            {
                var saveParams =
                    baseParams.Params.Read<SaveParam<BasePropertyOwnerDecision>>()
                        .Execute(container => Converter.ToSaveParam<BasePropertyOwnerDecision>(container, false));

                foreach (var record in saveParams.Records)
                {
                    var decision = record.AsObject();

                    if (decision.PropertyOwnerDecisionType == PropertyOwnerDecisionType.PreviouslyAccumulatedAmount)
                    {
                        var serv = Container.Resolve<IDomainService<PrevAccumulatedAmountDecision>>();
                        serv.Save(new PrevAccumulatedAmountDecision
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
                        var serv = Container.Resolve<IDomainService<MinFundSizeDecision>>();

                        serv.Save(new MinFundSizeDecision
                        {
                            MethodFormFund = decision.MethodFormFund,
                            MoOrganizationForm = decision.MoOrganizationForm,
                            PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                            SubjectMinFundSize = 40,
                            RealityObject = decision.RealityObject,
                            PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                        });
                    }
                    else if (decision.PropertyOwnerDecisionType == PropertyOwnerDecisionType.OwnerSpecialAccount)
                    {
                        var serv = Container.Resolve<IDomainService<OwnerAccountDecision>>();
                        serv.Save(new OwnerAccountDecision
                        {
                            MethodFormFund = decision.MethodFormFund,
                            MoOrganizationForm = decision.MoOrganizationForm,
                            PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                            OwnerAccountType = OwnerAccountDecisionType.OtherCooperative,
                            RealityObject = decision.RealityObject,
                            PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                        });
                    }
                    else if (decision.PropertyOwnerDecisionType == PropertyOwnerDecisionType.CreditOrganization)
                    {
                        var serv = Container.Resolve<IDomainService<CreditOrganizationDecision>>();
                        serv.Save(new CreditOrganizationDecision
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
                        var serv = Container.Resolve<IDomainService<ListServicesDecision>>();
                        serv.Save(new ListServicesDecision
                        {
                            MethodFormFund = decision.MethodFormFund,
                            MoOrganizationForm = decision.MoOrganizationForm,
                            PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                            RealityObject = decision.RealityObject,
                            PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                        });
                    }
                    else if (decision.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SetMinAmount)
                    {
                        var serv = Container.Resolve<IDomainService<MinAmountDecision>>();
                        serv.Save(new MinAmountDecision
                        {
                            MethodFormFund = decision.MethodFormFund,
                            MoOrganizationForm = decision.MoOrganizationForm,
                            PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                            RealityObject = decision.RealityObject,
                            PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                        });
                    }
                    else if (decision.MethodFormFund == MethodFormFundCr.RegOperAccount)
                    {
                        var serv = Container.Resolve<IDomainService<RegOpAccountDecision>>();
                        serv.Save(new RegOpAccountDecision
                        {
                            MethodFormFund = decision.MethodFormFund,
                            MoOrganizationForm = decision.MoOrganizationForm,
                            PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                            RealityObject = decision.RealityObject,
                            PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                        });
                    }
                    else
                    {
                        var serv = Container.Resolve<IDomainService<SpecialAccountDecision>>();
                        serv.Save(new SpecialAccountDecision
                        {
                            MethodFormFund = decision.MethodFormFund,
                            MoOrganizationForm = decision.MoOrganizationForm,
                            PropertyOwnerDecisionType = decision.PropertyOwnerDecisionType,
                            TypeOrganization = TypeOrganization.TSJ,
                            RealityObject = decision.RealityObject,
                            PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                        });
                    }
                }
            });

            return new BaseDataResult();
        }

        public IDataResult ListAccounts(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var roId = baseParams.Params.GetAs<long>("roId");

            var methodFormFund =
                Container.Resolve<IDomainService<RealityObject>>().GetAll()
                    .Where(x => x.Id == roId)
                    .Select(x => x.MethodFormFundCr)
                    .FirstOrDefault();



            if (methodFormFund == MethodFormFundCr.RegOperAccount)
            {
                var payAccdata =
                    Container.Resolve<IDomainService<PaymentAccount>>().GetAll()
                        .Where(x => x.RealityObject.Id == roId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Number,
                            x.OpenDate,
                            x.CloseDate
                        })
                        .Filter(loadParam, Container);

                return new ListDataResult(payAccdata.Order(loadParam).Paging(loadParam).ToList(), payAccdata.Count());
            }

            var specAccData =
                Container.Resolve<IDomainService<SpecialAccount>>().GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Number,
                        x.OpenDate,
                        x.CloseDate
                    })
                    .Filter(loadParam, Container);

            return new ListDataResult(specAccData.Order(loadParam).Paging(loadParam).ToList(), specAccData.Count());
        }

        protected virtual void InTransaction(Action action)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
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