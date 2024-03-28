using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Decisions.Nso.Entities;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Modules.ClaimWork.Entities;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.RegOperator.Domain.ReferenceCalculation;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.RegOperator.Entities.Dict;
using Bars.Gkh.RegOperator.Entities.ValueObjects;
using Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService.ReferenceCalculation;
using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService.Impl
{
    public class AgentPIRExecuteService : IAgentPIRExecuteService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetListPersonalAccountDebtor(BaseParams baseParams)
        {
            var domain = this.Container.Resolve<IDomainService<BasePersonalAccount>>();
            var loadParams = baseParams.GetLoadParam();
            int totalCount;

            var data = domain.GetAll()
                .Where(x => x.State.StartState)
                    .Select(x => new
                    {
                        x.Id,
                        AccountOwner = x.AccountOwner != null ? x.AccountOwner.Name : "",
                        x.PersonalAccountNum
                    }).Filter(loadParams, Container);

            totalCount = data.Count();

            var result = new ListDataResult(data.Paging(loadParams).ToList(), totalCount);


            return result;
        }

        public IDataResult AddPersonalAccountDebtor(BaseParams baseParams)
        {
            //throw new System.NotImplementedException();

            try
            {
                var agentPIRId = baseParams.Params.ContainsKey("agentPIRId")
                                     ? baseParams.Params["agentPIRId"].ToLong()
                                     : 0;
                var partIds = baseParams.Params.ContainsKey("partIds")
                                  ? baseParams.Params["partIds"].ToString().Split(',')
                                  : new string[] { };

                //в этом списке будут id инспектируемых частей, которые уже связаны с этим актом обследования
                //(чтобы не добавлять несколько одинаковых экспертов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var serviceParts = Container.Resolve<IDomainService<AgentPIRDebtor>>();
                var paDomain = Container.Resolve<IDomainService<BasePersonalAccount>>();

                listIds.AddRange(
                    serviceParts.GetAll()
                        .Where(x => x.AgentPIR.Id == agentPIRId)
                        .Select(x => x.Id)
                        .Distinct()
                        .ToList());

                foreach (var id in partIds.Select(x => x.ToLong()))
                {
                    //Если среди существующих частей уже есть такая часть, то пролетаем мимо
                    if (listIds.Contains(id))
                        continue;

                    //Если такого эксперта еще нет то добалвяем
                    var newObj = new AgentPIRDebtor
                    {
                        AgentPIR = new AgentPIR { Id = agentPIRId },
                        BasePersonalAccount = new BasePersonalAccount {Id = id },
                        Status = AgentPIRDebtorStatus.ToWork
                    };

                    serviceParts.Save(newObj);
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult GetListPayment(BaseParams baseParams)
        {
            int totalCount;
            var loadParams = baseParams.GetLoadParam(); 
            var agentPIRDebtorId = loadParams.Filter.GetAs("agentPIRDebtorId", 0L);    

            var agentPIRDebtor = this.Container.Resolve<IDomainService<AgentPIRDebtor>>()
                .GetAll().Where(x => x.Id == agentPIRDebtorId)
                .FirstOrDefault();

            var persAccAllTransfers = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>().GetAll()
            .Where(x => x.Owner.Id == agentPIRDebtor.BasePersonalAccount.Id)
            .Where(x => x.Operation.IsCancelled != true)
            .Where(x => x.PaymentDate > agentPIRDebtor.AgentPIR.DateStart)
            .OrderByDescending(x => x.PaymentDate)
            .Select(x => new
             {
                 x.Id,
                DatePayment = x.PaymentDate.Date,
                SumPayment = x.Amount,
                Reason = x.Reason

            }).Filter(loadParams, Container);

            totalCount = persAccAllTransfers.Count();

            var result = new ListDataResult(persAccAllTransfers.Paging(loadParams).ToList(), totalCount);

            return result;
        }

        public IDataResult DebtStartDateCalculate(BaseParams baseParams)
        {
            var debtorDomain = this.Container.ResolveDomain<AgentPIRDebtor>();

            //Проверяем документ
            var docId = baseParams.Params.GetAs<long>("docId");
            var recIds = baseParams.Params.GetAs("recIds", string.Empty);
            var paymentConfig = baseParams.Params.GetAs<DebtCalc>("debtCalcType");
            var penaltyConfig = baseParams.Params.GetAs<PenaltyCharge>("penChargeType");

            AgentPIRDebtor debtor = debtorDomain.GetAll()
                .Where(x => x.Id == docId)
                .FirstOrDefault();

            if (debtor == null)
            {
                return BaseDataResult.Error("Не найдена информация о документе");
            }

            if (paymentConfig == 0 || penaltyConfig == 0)
            {
                return BaseDataResult.Error("Не указаны параметры расчета задолженности");
            }

            var transfers = recIds.Split(',').Select(id => id.ToLong()).ToList();

            var agentPIRDebtor = this.Container.Resolve<IDomainService<AgentPIRDebtor>>().Get(docId);

            var allCalcs = this.Container.ResolveAll<IDebtorReferenceCalculationService>();
            var calcer = allCalcs.FirstOrDefault(x => x.DebtCalc == paymentConfig && x.PenaltyCharge == penaltyConfig);
            try
            {
                return calcer.CalculateReferencePayments(debtor, transfers);
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}
