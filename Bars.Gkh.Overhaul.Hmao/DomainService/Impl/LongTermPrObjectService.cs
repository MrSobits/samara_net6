using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;

    using Castle.Windsor;

    public class LongTermPrObjectService : ILongTermPrObjectService
    {
        public IWindsorContainer Container { get; set; }
        
        public IDataResult ListHasDecisionRegopAccount(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var objectId = baseParams.Params.GetAs<long>("objectId");

            var domainService = Container.Resolve<IDomainService<LongTermPrObject>>();

            var decisionDomainService = Container.Resolve<IDomainService<RegOpAccountDecision>>();

            var data = domainService.GetAll()
                .Where(x => x.Id != objectId)
                .Where(y => decisionDomainService.GetAll().Any(x => x.RealityObject.Id == y.Id))
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject.Address
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult GetOrgForm(BaseParams baseParams)
        {
            var realObjId = baseParams.Params.GetAs<long>("realObjId");
            var currentDate = DateTime.Now.Date;
            var contract =
                Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => x.RealityObject.Id == realObjId)
                    .Select(x => new
                    {
                        x.RealityObject.NumberApartments,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.EndDate,
                        x.ManOrgContract.TypeContractManOrgRealObj,
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
                orgForm = contract.TypeManagement == TypeManagementManOrg.TSJ && contract.NumberApartments > 30
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

                    if (decision.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SetMinAmount)
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
                            RealityObject = decision.RealityObject,
                            TypeOrganization = TypeOrganization.TSJ,
                            PropertyOwnerProtocol = decision.PropertyOwnerProtocol
                        });
                    }
                }
            });

            return new BaseDataResult();
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